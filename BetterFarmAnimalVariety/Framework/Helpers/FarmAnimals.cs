using StardewValley;
using System.Collections.Generic;

namespace BetterFarmAnimalVariety.Framework.Helpers
{
    class FarmAnimals
    {
        private static string CacheFileName => Constants.Mod.FarmAnimalsCacheFileName;

        public static Cache.FarmAnimals ReadCache()
        {
            return Helpers.Mod.ReadCache<Cache.FarmAnimals>(FarmAnimals.CacheFileName);
        }

        public static void Write(Cache.FarmAnimals animals)
        {
            Helpers.Mod.WriteCache<Cache.FarmAnimals>(FarmAnimals.CacheFileName, animals);
        }

        public static void RemoveCategory(string category)
        {
            // Grab the current cache
            Cache.FarmAnimals cache = Helpers.FarmAnimals.ReadCache();

            cache.RemoveCategory(category);

            // Write back to the cache
            Helpers.FarmAnimals.Write(cache);
        }

        public static void AddOrReplaceCategory(Cache.FarmAnimalCategory category)
        {
            // Load the cache
            Cache.FarmAnimals cache = Helpers.FarmAnimals.ReadCache();

            // Check if the category exists
            int index = cache.Categories.FindIndex(o => o.Category == category.Category);

            // If it does not exist, add it
            if (index == -1)
            {
                cache.Categories.Add(category);
            }
            else
            {
                // otherwise; replace it
                cache.Categories[index] = category;
            }

            // Write back to the cache
            Helpers.FarmAnimals.Write(cache);
        }

        public static bool CategoryExists(string category)
        {
            // Load the cache
            Cache.FarmAnimals cache = Helpers.FarmAnimals.ReadCache();

            return cache.CategoryExists(category);
        }

        public static List<Cache.FarmAnimalCategory> GetCategories()
        {
            // Load the cache
            Cache.FarmAnimals cache = Helpers.FarmAnimals.ReadCache();

            return cache.Categories;
        }

        public static Dictionary<string, List<string>> GroupTypesByCategory()
        {
            // Load the cache
            Cache.FarmAnimals cache = Helpers.FarmAnimals.ReadCache();

            return cache.GroupTypesByCategory();
        }

        public static Dictionary<string, List<string>> GroupPurchaseableTypesByCategory()
        {
            // Load the cache
            Cache.FarmAnimals cache = Helpers.FarmAnimals.ReadCache();

            return cache.GroupPurchaseableTypesByCategory();
        }

        public static Cache.FarmAnimalCategory GetCategory(string category)
        {
            // Load the cache
            Cache.FarmAnimals cache = Helpers.FarmAnimals.ReadCache();

            return cache.GetCategory(category);
        }

        public static List<StardewValley.Object> GetPurchaseAnimalStock(Farm farm)
        {
            // Load the cache
            Cache.FarmAnimals cache = Helpers.FarmAnimals.ReadCache();

            return cache.GetPurchaseAnimalStock(farm);
        }

        public static bool CanBePurchased(string category)
        {
            // Load the cache
            Cache.FarmAnimals cache = Helpers.FarmAnimals.ReadCache();

            return cache.CanBePurchased(category);
        }
    }
}
