using StardewValley;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety
{
    public class ModConfig
    {
        public string Format;
        public bool IsEnabled;
        public bool RandomizeNewbornFromCategory;
        public bool RandomizeHatchlingFromCategory;
        public bool IgnoreParentProduceCheck;
        public Dictionary<string, Framework.Config.FarmAnimal> FarmAnimals;

        public ModConfig()
        {
            this.Format = null;
            this.IsEnabled = true;
            this.RandomizeNewbornFromCategory = false;
            this.RandomizeHatchlingFromCategory = false;
            this.IgnoreParentProduceCheck = false;
            this.FarmAnimals = new Dictionary<string, Framework.Config.FarmAnimal>();
        }

        public Dictionary<string, List<string>> GroupTypesByCategory()
        {
            return this.FarmAnimals.ToDictionary(kvp => kvp.Key, kvp => new List<string>(kvp.Value.Types));
        }

        public bool IsValidFormat(string targetFormat)
        {
            return this.Format != null && this.Format.Equals(targetFormat);
        }

        public void InitializeFarmAnimals()
        {
            if (this.FarmAnimals.Any())
            {
                this.UpdateFarmAnimalCategories();
            }
            else
            {
                this.UpdateFarmAnimalValuesFromVanilla();
            }
        }

        public void UpdateFarmAnimalCategories()
        {
            // Need to restore the categories because they are not in the config.json
            foreach (KeyValuePair<string, Framework.Config.FarmAnimal> entry in this.FarmAnimals.ToDictionary(kvp => kvp.Key, kvp => kvp.Value))
            {
                this.FarmAnimals[entry.Key].Category = entry.Key;

                if (this.FarmAnimals[entry.Key].CanBePurchased())
                {
                    this.FarmAnimals[entry.Key].AnimalShop.Category = entry.Key;
                }
            }
        }

        public void UpdateFarmAnimalValuesFromVanilla()
        {
            foreach (Framework.Constants.FarmAnimalCategory farmAnimalStock in Framework.Constants.FarmAnimalCategory.All())
            {
                if (!this.FarmAnimals.ContainsKey(farmAnimalStock.ToString()))
                {
                    Framework.Config.FarmAnimal configFarmAnimal = new Framework.Config.FarmAnimal(farmAnimalStock);
                    this.FarmAnimals.Add(configFarmAnimal.Category, configFarmAnimal);
                }
            }
        }

        public List<StardewValley.Object> GetPurchaseAnimalStock(Farm farm)
        {
            return this.FarmAnimals.Values.Where(o => o.CanBePurchased())
                .Select(o => o.ToAnimalAvailableForPurchase(farm))
                .ToList();
        }
    }
}