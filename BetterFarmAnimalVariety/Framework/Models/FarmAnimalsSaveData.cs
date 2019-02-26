using System.Collections.Generic;
using System.Linq;
using StardewValley;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Models
{
    public class FarmAnimalsSaveData : SaveData
    {
        public Dictionary<long, TypeLog> TypeHistory = new Dictionary<long, TypeLog>();

        public FarmAnimalsSaveData() { }

        public Dictionary<long, TypeLog> GetTypeHistory()
        {
            return this.TypeHistory;
        }

        private void Write()
        {
            base.Write<FarmAnimalsSaveData>(Constants.Mod.FarmAnimalsSaveDataKey, this);
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
            Decorators.FarmAnimal moddedAnimal = new Decorators.FarmAnimal(animal);

            this.AddTypeHistory(moddedAnimal);
        }

        public void AddTypeHistory(Decorators.FarmAnimal moddedAnimal)
        {
            this.AddTypeHistory(moddedAnimal, moddedAnimal.GetTypeString());
        }

        public void AddTypeHistory(StardewValley.FarmAnimal animal, string originalType)
        {
            Decorators.FarmAnimal moddedAnimal = new Decorators.FarmAnimal(animal);

            this.AddTypeHistory(moddedAnimal, originalType);
        }

        public void AddTypeHistory(Decorators.FarmAnimal moddedAnimal, string originalType)
        {
            this.AddTypeHistory(moddedAnimal.GetUniqueId(), moddedAnimal.GetTypeString(), originalType);
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
            Decorators.FarmAnimal moddedAnimal = new Decorators.FarmAnimal(animal);
            return this.GetSavedTypeOrDefault(moddedAnimal);
        }

        public string GetSavedTypeOrDefault(Decorators.FarmAnimal moddedAnimal)
        {
            return this.GetSavedTypeOrDefault(moddedAnimal.GetUniqueId(), moddedAnimal.IsCoopDweller());
        }

        public string GetSavedTypeOrDefault(long animalId, bool isCoop)
        {
            return this.Exists(animalId)
                ? this.TypeHistory[animalId].SavedType
                : PariteeCore.Api.FarmAnimal.GetDefaultType(isCoop);
        }

        public TypeLog GetTypeHistory(long myId)
        {
            return this.TypeHistory.FirstOrDefault(kvp => kvp.Key.Equals(myId)).Value;
        }

        public void OverwriteFarmAnimal(ref Decorators.FarmAnimal moddedAnimal, string requestedType)
        {
            // WARNING!
            // Don't sanitize a farm animal's type by blue/void/brown cow chance
            // etc. or BFAV config existence here. These checks should be done 
            // in the menus, etc.

            if (!moddedAnimal.HasName())
            {
                return;
            }

            // NOTE:
            // Even for vanilla animals we want to overwrite because the vanilla 
            // constructor could turn a "White Chicken" into a "Brown Chicken" 
            // based on chance

            // Check the save entry for reloaded animals that may have their 
            // vanilla replacements saved which can't be used
            TypeLog typeHistory = this.GetTypeHistory(moddedAnimal.GetUniqueId());

            // If there's a save data entry, use that; otherwise this might be 
            // an animal created before being saved (ie. created in current day)
            string currentType = typeHistory == null ? (requestedType ?? moddedAnimal.GetTypeString()) : typeHistory.CurrentType;

            // Set the animal with the new type's data values
            moddedAnimal.UpdateFromData(currentType);

            // Make sure we always reset the current produce of 
            // non -producers at every chance we get
            if (!moddedAnimal.IsAProducer())
            {
                moddedAnimal.SetCurrentProduce(PariteeCore.Constants.FarmAnimal.NoProduce);
            }
        }
    }
}
