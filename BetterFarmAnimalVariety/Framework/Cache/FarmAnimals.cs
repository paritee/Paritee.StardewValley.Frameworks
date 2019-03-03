using StardewValley;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Cache
{
    class FarmAnimals
    {
        public readonly List<Cache.FarmAnimalCategory> Categories = new List<Cache.FarmAnimalCategory>();

        public FarmAnimals() { }

        public FarmAnimals(List<Cache.FarmAnimalCategory> categories)
        {
            this.Categories = categories;
        }

        public bool CategoryExists(string category)
        {
            if (this.Categories == null)
            {
                return false;
            }

            return this.Categories.Exists(o => o.Category.Category.Equals(category));
        }

        public Dictionary<string, List<string>> GroupTypesByCategory()
        {
            return this.Categories.ToDictionary(o => o.Category.Category, o => new List<string>(o.Category.Types));
        }

        private List<string> TypesInShop(Framework.Cache.FarmAnimalCategory category)
        {
            return category.Category.CanBePurchased()
                ? category.Category.Types.Where(t => !category.Category.AnimalShop.Exclude.Contains(t)).ToList()
                : new List<string>();
        }

        public Dictionary<string, List<string>> GroupPurchaseableTypesByCategory()
        {
            return this.Categories.ToDictionary(o => o.Category.Category, this.TypesInShop)
                .Where(kvp => kvp.Value.Any()) // Filter out empty lists
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public Framework.Cache.FarmAnimalCategory GetCategory(string category)
        {
            return this.Categories.FirstOrDefault(o => o.Category.Category.Equals(category));
        }

        public void RemoveCategory(string category)
        {
            this.Categories.RemoveAll(o => o.Category.Category.Equals(category));
        }

        public List<StardewValley.Object> GetPurchaseAnimalStock(Farm farm)
        {
            return this.Categories.Where(o => o.Category.CanBePurchased())
                .Select(o => o.Category.ToAnimalAvailableForPurchase(farm))
                .ToList();
        }

        public bool CanBePurchased(string category)
        {
            return this.Categories.Exists(o => o.Category.Equals(category) && o.Category.CanBePurchased());
        }
    }
}
