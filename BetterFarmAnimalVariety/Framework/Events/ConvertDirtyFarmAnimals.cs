using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BetterFarmAnimalVariety.Framework.SaveData;
using static StardewValley.Menus.LoadGameMenu;

namespace BetterFarmAnimalVariety.Framework.Events
{
    class ConvertDirtyFarmAnimals
    {
        public static void OnSaving(SavingEventArgs e)
        {
            FarmAnimalsSaveData saveData = FarmAnimalsSaveData.Deserialize();

            // Track the animal ID because we're going to remove 
            // animals that no longer exist from the save data 
            // (ex. sold, sound in the night event, etc.)
            List<long> animalIds = new List<long>();

            // Need to go through each locations(farm)/buildings(ah indoors)/animals 
            // to convert any "dirty" types to their vanilla version prior to save. 
            // All types (vanilla or dirty) are logged in the save data to be 
            // restored during the "Saved" event.
            for (int index = 0; index < Game1.locations.Count; ++index)
            {
                if (!(Game1.locations[index] is Farm farm))
                {
                    continue;
                }

                for (int j = 0; j < farm.buildings.Count; ++j)
                {
                    if (!(farm.buildings[j].indoors.Value is AnimalHouse animalHouse))
                    {
                        continue;
                    }

                    for (int k = 0; k < animalHouse.animalsThatLiveHere.Count(); ++k)
                    {
                        long id = animalHouse.animalsThatLiveHere.ElementAt(k);
                        FarmAnimal animal = animalHouse.animals[id];

                        animalIds.Add(id);

                        // Only non-vanilla animals need to be migrated, but...
                        if (Framework.Api.FarmAnimal.IsVanilla(animal.type.Value))
                        {
                            // ... always log the animal's type in the history 
                            // for convenience
                            if (!saveData.Exists(animal.myID.Value))
                            {
                                saveData.AddTypeHistory(animal.myID.Value, animal.type.Value, animal.type.Value);
                            }

                            continue;
                        }

                        // Return the type that is logged for saves or 
                        // automatically default the coop/barn dwellers
                        string savedType = saveData.GetSavedTypeOrDefault(animal);

                        // Convert it to the proper vanilla animal
                        KeyValuePair<string, string> contentDataEntry = Api.Content.LoadDataEntry<string, string>(Constants.Content.DataFarmAnimalsContentPath, savedType);

                        // Kill everything if for some reason the user removed 
                        // the default dweller information from the game
                        if (contentDataEntry.Key == null)
                        {
                            throw new KeyNotFoundException($"Could not find {savedType} to overwrite custom farm animal for saving. This is a fatal error. Please make sure you have {savedType} in the game.");
                        }

                        // Make sure this animal exists in the save data and 
                        // has the most updated information. Could have been 
                        // created /purchased today and not saved yet.
                        saveData.AddTypeHistory(animal.myID.Value, animal.type.Value, savedType);

                        // Overwrite the animal
                        // animal.reload() will be called in the "Saved" event
                        Framework.Api.FarmAnimal.UpdateFromData(ref animal, contentDataEntry);
                    }
                }

                break;
            }

            if (saveData.TypeHistory.Any())
            {
                // Remove any ids from the save data that should not be there
                List<long> keysToBeRemoved = saveData.TypeHistory.Keys
                    .Where(key => !animalIds.Contains(key))
                    .ToList();

                saveData.RemoveTypeHistory(keysToBeRemoved);
            }
        }

        public static void OnSaved(SavedEventArgs e)
        {
            FarmAnimalsSaveData saveData = FarmAnimalsSaveData.Deserialize();

            // Need to reload each animal after save because the game and 
            // SMAPI does a strange thing where the reload happens prior to 
            // the "Saving" event
            for (int index = 0; index < Game1.locations.Count; ++index)
            {
                if (!(Game1.locations[index] is Farm farm))
                {
                    continue;
                }

                for (int j = 0; j < farm.buildings.Count; ++j)
                {
                    if (!(farm.buildings[j].indoors.Value is AnimalHouse animalHouse))
                    {
                        continue;
                    }

                    for (int k = 0; k < animalHouse.animalsThatLiveHere.Count(); ++k)
                    {
                        long id = animalHouse.animalsThatLiveHere.ElementAt(k);
                        FarmAnimal animal = animalHouse.animals[id];

                        animal.reload(animal.home);
                    }
                }

                break;
            }
        }

        public static bool OnButtonPressed(ButtonPressedEventArgs e, out Dictionary<long, TypeLog> typesToBeMigrated)
        {
            typesToBeMigrated = new Dictionary<long, TypeLog>();

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
            if (!(Api.Game.GetActiveMenu() is TitleMenu titleMenu))
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

                int currentItemIndex = Framework.Helpers.Reflection.GetFieldValue<int>(loadGameMenu, "currentItemIndex");
                SaveFileSlot saveFileSlot = Framework.Helpers.Reflection.GetFieldValue<List<MenuSlot>>(loadGameMenu, "menuSlots")[currentItemIndex + index] as SaveFileSlot;

                // Need to manually parse the XML since casting to a FarmAnimal 
                // triggers the data search crash that this command aims to avoid
                if (!Directory.Exists(StardewModdingAPI.Constants.SavesPath))
                {
                    throw new DirectoryNotFoundException($"cannot find saves path directory");
                }

                // Scan only the most recent save if it can be found
                Framework.Helpers.GameSave.CleanFarmAnimals(Path.Combine(StardewModdingAPI.Constants.SavesPath, saveFileSlot.Farmer.slotName), out typesToBeMigrated);

                return true;
            }

            return false;
        }
    }
}
