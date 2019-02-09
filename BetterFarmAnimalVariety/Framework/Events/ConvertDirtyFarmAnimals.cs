using BetterFarmAnimalVariety.Framework.Content;
using BetterFarmAnimalVariety.Framework.Data;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Menus;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static StardewValley.Menus.LoadGameMenu;

namespace BetterFarmAnimalVariety.Framework.Events
{
    class ConvertDirtyFarmAnimals
    {
        public static void OnSaving(SavingEventArgs e)
        {
            FarmAnimalsSaveData saveData = FarmAnimalsSaveData.Deserialize();

            // Convert FarmAnimals to defaults when saving the game, search 
            // through all AnimalHouses and overwrite to not contaminate the saves
            foreach (Building building in Game1.getFarm().buildings)
            {
                // We only care about animal houses (ie. barns and coops)
                if (!(building.indoors.Value is AnimalHouse animalHouse))
                {
                    continue;
                }

                foreach (long id in animalHouse.animalsThatLiveHere)
                {
                    FarmAnimal animal = Utility.getAnimal(id);

                    // Only non-vanilla animals need to be updated before being saved
                    if (Framework.Api.FarmAnimal.IsVanillaType(animal.type.Value))
                    {
                        continue;
                    }

                    string savedType = saveData.GetSavedTypeOrDefault(animal);

                    // Convert it to the proper vanilla
                    KeyValuePair<string, string> contentDataEntry = FarmAnimalsData.Load()
                        .First(o => o.Key.Equals(savedType));

                    // Kill everything if for some reason the user removed the default dweller information from the game
                    if (contentDataEntry.Key == null)
                    {
                        throw new KeyNotFoundException($"Could not find {savedType} to overwrite custom farm animal for saving. This is a fatal error. Please make sure you have {savedType} in the game.");
                    }

                    FarmAnimal animalToBeConverted = (animal.home.indoors.Value as AnimalHouse).animals.Values
                        .First(o => o.myID.Value.Equals(animal.myID.Value));

                    // Overwrite the animal
                    Framework.Api.FarmAnimal.UpdateFromData(ref animalToBeConverted, contentDataEntry);
                }
            }
        }

        public static bool OnButtonPressed(ButtonPressedEventArgs e, out List<TypeHistory> typesToBeMigrated)
        {
            typesToBeMigrated = new List<TypeHistory>();

            // Ignore if player has loaded a save
            if (Context.IsWorldReady)
            {
                return false;
            }

            // We only care about left mouse clicks right now
            if (e.Button != SButton.MouseLeft)
            {
                return false;
            }

            // Always attempt to clean up the animal types to prevent on save load crashes
            // if the patch mod had been removed without the animals being sold/deleted.
            // This will now also migrate users from bfav 2.x where types were saved directly 
            // to 3.x where they are not.
            if (!(Game1.activeClickableMenu is TitleMenu titleMenu))
            {
                return false;
            }

            if (!(TitleMenu.subMenu is LoadGameMenu loadGameMenu))
            {
                return false;
            }

            for (int index = 0; index < loadGameMenu.slotButtons.Count; index++)
            {
                if (!loadGameMenu.slotButtons[index].containsPoint((int)e.Cursor.ScreenPixels.X, (int)e.Cursor.ScreenPixels.Y))
                {
                    continue;
                }

                int currentItemIndex = Framework.Helpers.Utilities.GetFieldValue<int>(loadGameMenu, "currentItemIndex");
                SaveFileSlot saveFileSlot = Framework.Helpers.Utilities.GetFieldValue<List<MenuSlot>>(loadGameMenu, "menuSlots")[currentItemIndex + index] as SaveFileSlot;

                // Need to manually parse the XML since casting to a FarmAnimal 
                // triggers the data search crash that this command aims to avoid
                if (!Directory.Exists(Constants.SavesPath))
                {
                    throw new DirectoryNotFoundException($"cannot find saves path directory");
                }

                // Scan only the most recent save if it can be found
                Framework.Helpers.Utilities.FixGameSave(Path.Combine(Constants.SavesPath, Game1.player.slotName), out typesToBeMigrated);

                return true;
            }

            return false;
        }
    }
}
