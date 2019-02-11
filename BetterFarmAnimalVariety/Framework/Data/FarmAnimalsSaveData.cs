using StardewValley;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Data
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
            string saveDataDir = Path.Combine(StardewModdingAPI.Constants.DataPath, Helpers.Constants.ModKey);

            return Path.Combine(saveDataDir, Helpers.Constants.FarmAnimalsSaveDataFileName);
        }

        public static FarmAnimalsSaveData Deserialize()
        {
            FarmAnimalsSaveData data = Data.SaveData.Deserialize<FarmAnimalsSaveData>(Data.FarmAnimalsSaveData.GetPath());

            return data ?? new FarmAnimalsSaveData();
        }

        private void WriteChanges()
        {
            base.WriteChanges(this, Data.FarmAnimalsSaveData.GetPath());
        }

        public void AddTypeHistory(Dictionary<long, TypeLog> history)
        {
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

        public void Clean()
        {
            if (!this.TypeHistory.Any())
            {
                // Nothing to do
                return;
            }

            // Validate if any animals exist in the save data, but not in the game
            List<long> remainingAnimals = Game1.getFarm().buildings
                .Where(o => o.indoors.Value is AnimalHouse)
                .SelectMany(o => (o.indoors.Value as AnimalHouse).animals.Keys.Cast<long>())
                .ToList();

            // Find the difference
            List<long> animalsToBeRemoved = this.TypeHistory.Keys
                .Where(o => !remainingAnimals.Contains(o))
                .ToList();

            this.RemoveTypeHistory(animalsToBeRemoved);
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
