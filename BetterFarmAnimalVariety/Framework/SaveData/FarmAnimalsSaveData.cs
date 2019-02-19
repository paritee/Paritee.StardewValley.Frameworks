using System.Collections.Generic;
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

        public static string GetKey()
        {
            return SaveData.GetKey(Constants.Mod.FarmAnimalsSaveDataKey);
        }

        public FarmAnimalsSaveData Read()
        {
            FarmAnimalsSaveData data = base.Read<FarmAnimalsSaveData>(FarmAnimalsSaveData.GetKey());

            return data ?? new FarmAnimalsSaveData();
        }

        private void Write()
        {
            base.Write(this, FarmAnimalsSaveData.GetKey());
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
