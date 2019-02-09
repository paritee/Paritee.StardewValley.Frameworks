using StardewValley;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Data
{
    class FarmAnimalsSaveData : SaveData
    {
        public Dictionary<long, TypeHistory> TypeHistory = new Dictionary<long, TypeHistory>();

        public static string GetPath()
        {
            string saveDataDir = Path.Combine(StardewModdingAPI.Constants.DataPath, Helpers.Constants.ModDataKey);

            return Path.Combine(saveDataDir, Helpers.Constants.FarmAnimalsSaveDataFileName);
        }

        public static FarmAnimalsSaveData Deserialize()
        {
            FarmAnimalsSaveData data = Data.SaveData.Deserialize<FarmAnimalsSaveData>(FarmAnimalsSaveData.GetPath());

            return data ?? new FarmAnimalsSaveData();
        }

        private void WriteChanges()
        {
            base.WriteChanges(this, FarmAnimalsSaveData.GetPath());
        }

        public void AddTypeHistory(List<TypeHistory> history)
        {
            // Clean up the data file
            foreach (TypeHistory typeHistory in history)
            {
                if (this.TypeHistory.ContainsKey(typeHistory.FarmAnimalId))
                {
                    this.TypeHistory[typeHistory.FarmAnimalId] = typeHistory;
                }
                else
                {
                    this.TypeHistory.Add(typeHistory.FarmAnimalId, typeHistory);
                }
            }

            this.WriteChanges();
        }

        public void AddTypeHistory(long key, string currentType, string originalType)
        {
            TypeHistory typeHistory = new TypeHistory(key, currentType, originalType);

            // Clean up the data file
            if (this.TypeHistory.ContainsKey(key))
            {
                this.TypeHistory[key] = typeHistory;
            }
            else
            {
                this.TypeHistory.Add(key, typeHistory);
            }

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

        public void CleanTypeHistory()
        {
            if (this.TypeHistory.Count < 1)
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

        public string GetSavedTypeOrDefault(FarmAnimal animal)
        {
            if (this.TypeHistory.ContainsKey(animal.myID.Value))
            {
                return this.TypeHistory[animal.myID.Value].SavedType;
            }

            return Api.FarmAnimal.GetDefaultType(animal);
        }
    }
}
