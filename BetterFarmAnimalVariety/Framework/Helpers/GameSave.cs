using StardewValley;
using System.Collections.Generic;
using System.Linq;
using BetterFarmAnimalVariety.Framework.SaveData;
using System.Diagnostics;

namespace BetterFarmAnimalVariety.Framework.Helpers
{
    internal class GameSave
    {
        public static void OverwriteFarmAnimal(ref FarmAnimal animal, string requestedType)
        {
            // ==========
            // WARNING:
            // Don't sanitize a farm animal's type by blue/void/brown cow chance
            // etc. or BFAV config existence here. These checks should be done 
            // in the menus, etc.
            // ==========

            if (animal.Name == null)
            {
                // TODO: Debug why this happens on reload on game save
                return;
            }

            // NOTE:
            // Even for vanilla animals we want to overwrite because the vanilla 
            // constructor could turn a "White Chicken" into a "Brown Chicken" 
            // based on chance

            // Check the save entry for reloaded animals that may have their 
            // vanilla replacements saved which can't be used
            TypeLog typeHistory = (new FarmAnimalsSaveData()).Read().GetTypeHistory(animal.myID.Value);

            Debug.WriteLine($"typeHistory == null {typeHistory == null}");

            // If there's a save data entry, use that; otherwise this might be 
            // an animal created before being saved (ie. created in current day)
            string currentType = typeHistory == null ? (requestedType ?? animal.type.Value) : typeHistory.CurrentType;

            // Grab the new type's data to override if it exists
            Dictionary<string, string> contentData = Api.Content.LoadData<string, string>(Constants.Content.DataFarmAnimalsContentPath);
            KeyValuePair<string, string> contentDataEntry = Api.Content.GetDataEntry<string, string>(contentData, currentType);

            // Always validate if the type we're trying to use exists
            if (contentDataEntry.Key == null)
            {
                // Get a default type to use
                string defaultType = Api.FarmAnimal.GetDefaultType(animal);

                // Set it to the default before we continue
                contentDataEntry = contentData.FirstOrDefault(kvp => kvp.Key.Equals(defaultType));

                // Do a final check to make sure the default exists; otherwise 
                // we need to kill everything. This should never happen unless 
                // agressive mods are being used to REMOVE vanilla animals.
                if (contentDataEntry.Key == null)
                {
                    throw new KeyNotFoundException($"Could not find {defaultType} to overwrite custom farm animal for saving. This is a fatal error. Please make sure you have {defaultType} in the game.");
                }
            }

            // Set the animal with the new type's data values
            Api.FarmAnimal.UpdateFromData(ref animal, contentDataEntry);
        }
    }
}