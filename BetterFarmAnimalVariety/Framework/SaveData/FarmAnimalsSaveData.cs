﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using StardewValley;

namespace BetterFarmAnimalVariety.Framework.SaveData
{
    class FarmAnimalsSaveData : SaveData
    {
        public Dictionary<long, TypeLog> TypeHistory = new Dictionary<long, TypeLog>();

        public Dictionary<long, TypeLog> GetTypeHistory()
        {
            return this.TypeHistory;
        }

        public static string GetPath()
        {
            string saveDataDir = Path.Combine(StardewModdingAPI.Constants.DataPath, Constants.Mod.Key);

            return Path.Combine(saveDataDir, Constants.Mod.FarmAnimalsSaveDataFileName);
        }

        public static FarmAnimalsSaveData Deserialize()
        {
            FarmAnimalsSaveData data = SaveData.Deserialize<FarmAnimalsSaveData>(FarmAnimalsSaveData.GetPath());

            return data ?? new FarmAnimalsSaveData();
        }

        private void WriteChanges()
        {
            base.WriteChanges(this, FarmAnimalsSaveData.GetPath());
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

            this.WriteChanges();
        }

        public void AddTypeHistory(long animalId, string currentType, string originalType)
        {
            TypeLog typeHistory = new TypeLog(currentType, originalType);

            // Update the existing entry or create it if it doesn't exist
            this.TypeHistory[animalId] = typeHistory;

            this.WriteChanges();
        }

        public void RemoveTypeHistory(List<long> keys)
        {
            if (!keys.Any())
            {
                return;
            }

            // Clean up the data file
            this.TypeHistory = this.TypeHistory.Where(o => !keys.Contains(o.Key)).ToDictionary(o => o.Key, o => o.Value);

            this.WriteChanges();
        }

        public void RemoveTypeHistory(long key)
        {
            // Clean up the data file
            this.TypeHistory.Remove(key);

            this.WriteChanges();
        }

        public bool Exists(long myId)
        {
            return this.TypeHistory.ContainsKey(myId);
        }

        public string GetSavedTypeOrDefault(FarmAnimal animal)
        {
            return this.Exists(animal.myID.Value)
                ? this.TypeHistory[animal.myID.Value].SavedType
                : Api.FarmAnimal.GetDefaultType(animal);
        }

        public TypeLog GetTypeHistory(long myId)
        {
            return this.TypeHistory.FirstOrDefault(kvp => kvp.Key.Equals(myId)).Value;
        }
    }
}