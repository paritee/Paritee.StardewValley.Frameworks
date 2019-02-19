using BetterFarmAnimalVariety.Framework.Models;
using StardewModdingAPI.Events;
using StardewValley;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Events
{
    class ConvertDirtyFarmAnimals
    {
        public static void OnSaving(SavingEventArgs e)
        {
            FarmAnimalsSaveData saveData = (new FarmAnimalsSaveData(Constants.Mod.Key)).Read();

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
            FarmAnimalsSaveData saveData = (new FarmAnimalsSaveData(Constants.Mod.Key)).Read();

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
    }
}
