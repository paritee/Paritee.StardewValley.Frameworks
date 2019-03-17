using StardewModdingAPI.Events;
using StardewValley;
using System.Collections.Generic;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Events
{
    class ConvertDirtyFarmAnimals
    {
        public static void OnSaving(SavingEventArgs e)
        {
            SaveData.FarmAnimals saveData = Helpers.Mod.ReadSaveData<SaveData.FarmAnimals>(Constants.Mod.FarmAnimalsSaveDataKey);

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


                        Decorators.FarmAnimal moddedAnimal = new Decorators.FarmAnimal(animalHouse.animals[id]);

                        animalIds.Add(id);

                        // Only non-vanilla animals need to be migrated, but...
                        if (moddedAnimal.IsVanilla())
                        {
                            // ... always log the animal's type in the history 
                            // for convenience
                            if (!saveData.AnimalExists(moddedAnimal.GetUniqueId()))
                            {
                                saveData.AddAnimal(moddedAnimal);
                            }

                            continue;
                        }

                        // Save the current type before it gets overwritten from the data
                        string currentType = moddedAnimal.GetTypeString();

                        // Return the type that is logged for saves or 
                        // automatically default the coop/barn dwellers
                        string savedType = saveData.GetSavedTypeOrDefault(moddedAnimal);
                        
                        // Overwrite the animal
                        // animal.reload() will be called in the "Saved" event
                        moddedAnimal.UpdateFromData(savedType);

                        // Make sure this animal exists in the save data and 
                        // has the most updated information. Could have been 
                        // created /purchased today and not saved yet.
                        SaveData.TypeLog typeLog = new SaveData.TypeLog(currentType, savedType);

                        saveData.AddAnimal(moddedAnimal, typeLog);
                    }
                }

                break;
            }

            if (saveData.HasAnyAnimals())
            {
                // Remove any ids from the save data that should not be there
                List<long> keysToBeRemoved = saveData.Animals
                    .Where(o => !animalIds.Contains(o.Id))
                    .Select(o => o.Id)
                    .ToList();

                saveData.RemoveAnimals(keysToBeRemoved);
            }

            // Always write the save data after
            saveData.Write();
        }

        public static void OnSaved(SavedEventArgs e)
        {
            // Need to reload each animal after save/save load because the game 
            // and SMAPI does a strange thing where the reload happens prior to 
            // the "Saving" event
            PariteeCore.Characters.FarmAnimal.ReloadAll();
        }

        public static void OnSaveLoaded(SaveLoadedEventArgs e)
        {
            // Need to reload each animal after save/save load because the game 
            // and SMAPI does a strange thing where the reload happens prior to 
            // the "Saving" event
            PariteeCore.Characters.FarmAnimal.ReloadAll();
        }
    }
}
