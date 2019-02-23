using System.Collections.Generic;
using System.Linq;
using Paritee.StardewValley.Core.Models;
using StardewValley;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Models
{
    public class FarmAnimalsSaveData : SaveData
    {
        public Dictionary<long, TypeLog> TypeHistory = new Dictionary<long, TypeLog>();

        public FarmAnimalsSaveData(string uniqueModId) : base (uniqueModId, Constants.Mod.FarmAnimalsSaveDataKey) { }

        public Dictionary<long, TypeLog> GetTypeHistory()
        {
            return this.TypeHistory;
        }

        public FarmAnimalsSaveData Read()
        {
            return base.Read<FarmAnimalsSaveData>() ?? this;
        }

        private void Write()
        {
            base.Write(this);
        }

        public void AddTypeHistory(Dictionary<long, TypeLog> history)
        {
            if (!history.Any())
            {
                return;
            }

            // Clean up the data file
            foreach (KeyValuePair<long, TypeLog> typeHistory in history)
            {
                // Update the existing entry or create it if it doesn't exist
                this.TypeHistory[typeHistory.Key] = typeHistory.Value;
            }

            this.Write();
        }

        public void AddTypeHistory(StardewValley.FarmAnimal animal)
        {
            this.AddTypeHistory(animal, PariteeCore.Api.FarmAnimal.GetType(animal));
        }

        public void AddTypeHistory(StardewValley.FarmAnimal animal, string originalType)
        {
            this.AddTypeHistory(PariteeCore.Api.FarmAnimal.GetId(animal), PariteeCore.Api.FarmAnimal.GetType(animal), originalType);
        }

        public void AddTypeHistory(long animalId, string currentType, string originalType)
        {
            TypeLog typeHistory = new TypeLog(currentType, originalType);

            // Update the existing entry or create it if it doesn't exist
            this.TypeHistory[animalId] = typeHistory;

            this.Write();
        }

        public void RemoveTypeHistory(List<long> keys)
        {
            if (!keys.Any())
            {
                return;
            }

            // Clean up the data file
            this.TypeHistory = this.TypeHistory.Where(o => !keys.Contains(o.Key)).ToDictionary(o => o.Key, o => o.Value);

            this.Write();
        }

        public void RemoveTypeHistory(long key)
        {
            // Clean up the data file
            this.TypeHistory.Remove(key);

            this.Write();
        }

        public bool Exists(long myId)
        {
            return this.TypeHistory.ContainsKey(myId);
        }

        public string GetSavedTypeOrDefault(FarmAnimal animal)
        {
            long myId = PariteeCore.Api.FarmAnimal.GetId(animal);

            return this.Exists(myId)
                ? this.TypeHistory[myId].SavedType
                : PariteeCore.Api.FarmAnimal.GetDefaultType(animal);
        }

        public TypeLog GetTypeHistory(long myId)
        {
            return this.TypeHistory.FirstOrDefault(kvp => kvp.Key.Equals(myId)).Value;
        }

        public void OverwriteFarmAnimal(FarmAnimal animal, string requestedType)
        {
            // ==========
            // WARNING:
            // Don't sanitize a farm animal's type by blue/void/brown cow chance
            // etc. or BFAV config existence here. These checks should be done 
            // in the menus, etc.
            // ==========

            if (!PariteeCore.Api.FarmAnimal.HasName(animal))
            {
                return;
            }

            // NOTE:
            // Even for vanilla animals we want to overwrite because the vanilla 
            // constructor could turn a "White Chicken" into a "Brown Chicken" 
            // based on chance

            // Check the save entry for reloaded animals that may have their 
            // vanilla replacements saved which can't be used
            TypeLog typeHistory = this.GetTypeHistory(PariteeCore.Api.FarmAnimal.GetId(animal));

            // If there's a save data entry, use that; otherwise this might be 
            // an animal created before being saved (ie. created in current day)
            string currentType = typeHistory == null ? (requestedType ?? PariteeCore.Api.FarmAnimal.GetType(animal)) : typeHistory.CurrentType;

            // Grab the new type's data to override if it exists
            Dictionary<string, string> contentData = PariteeCore.Api.Content.LoadData<string, string>(PariteeCore.Constants.Content.DataFarmAnimalsContentPath);
            KeyValuePair<string, string> contentDataEntry = PariteeCore.Api.Content.GetDataEntry<string, string>(contentData, currentType);

            // Always validate if the type we're trying to use exists
            if (contentDataEntry.Key == null)
            {
                // Get a default type to use
                string defaultType = PariteeCore.Api.FarmAnimal.GetDefaultType(animal);

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
            PariteeCore.Api.FarmAnimal.UpdateFromData(animal, contentDataEntry);
        }
    }
}
