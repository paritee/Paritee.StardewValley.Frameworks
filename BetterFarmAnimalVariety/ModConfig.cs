using StardewValley;
using System.Collections.Generic;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety
{
    public class ModConfig
    {
        public string Format;
        public bool IsEnabled;
        public List<Framework.Config.FarmAnimal> FarmAnimals;

        public ModConfig()
        {
            this.Format = null;
            this.IsEnabled = true;
            this.FarmAnimals = new List<Framework.Config.FarmAnimal>();
        }

        public Dictionary<string, List<string>> GroupTypesByCategory()
        {
            return this.FarmAnimals.ToDictionary(o => o.Category, o => new List<string>(o.Types));
        }

        public List<string> TypesInShop(Framework.Config.FarmAnimal animal)
        {
            return animal.CanBePurchased()
                ? animal.Types.Where(t => !animal.AnimalShop.Exclude.Contains(t)).ToList()
                : new List<string>();
        }

        public Dictionary<string, List<string>> GroupPurchaseableTypesByCategory()
        {
            return this.FarmAnimals.ToDictionary(o => o.Category, this.TypesInShop)
                .Where(kvp => kvp.Value.Any()) // Filter out empty lists
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public bool CategoryExists(string category)
        {
            if (this.FarmAnimals == null)
            {
                return false;
            }

            return this.FarmAnimals.Exists(o => o.Category.Equals(category));
        }

        public Framework.Config.FarmAnimal GetCategory(string category)
        {
            return this.FarmAnimals.FirstOrDefault(o => o.Category.Equals(category));
        }

        public void RemoveCategory(string category)
        {
            this.FarmAnimals.RemoveAll(o => o.Category.Equals(category));
        }

        public bool IsValidFormat(string targetFormat)
        {
            return this.Format != null && this.Format.Equals(targetFormat);
        }

        public void SeedVanillaFarmAnimals()
        {
            this.FarmAnimals = PariteeCore.Constants.VanillaFarmAnimalCategory.All()
                .Select(o => new Framework.Config.FarmAnimal(o))
                .ToList();
        }

        public List<StardewValley.Object> GetPurchaseAnimalStock(Farm farm)
        {
            return this.FarmAnimals.Where(o => o.CanBePurchased())
                .Select(o => o.ToAnimalAvailableForPurchase(farm))
                .ToList();
        }
    }
}