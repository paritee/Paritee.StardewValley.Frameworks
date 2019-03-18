using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Events
{
    class RefreshCache
    {
        public static void ValidateCachedFarmAnimals(IModHelper helper, IMonitor monitor)
        {
            // Get the cache
            Cache.FarmAnimals cache = Helpers.FarmAnimals.ReadCache();

            List<string> categoriesToBeRemoved = new List<string>();

            // Validate the cached animals
            foreach (Cache.FarmAnimalCategory category in cache.Categories)
            {
                try
                {
                    // Validate category
                    Helpers.Assert.ValidStringLength("category", category.Category, 1);

                    // Validate types
                    List<string> typesToBeRemoved = new List<string>();

                    foreach (Cache.FarmAnimalType type in category.Types)
                    {
                        try
                        {
                            Helpers.Assert.FarmAnimalTypeIsNotRestricted(type.Type);
                            Helpers.Assert.FarmAnimalTypeExists(type.Type);
                        }
                        catch (Exception exception)
                        {
                            monitor.Log($"{type.Type} type will not load: {exception.Message}", LogLevel.Warn);

                            typesToBeRemoved.Add(type.Type);
                        }
                    }

                    if (typesToBeRemoved.Any())
                    {
                        // Remove any types that failed the validation check to 
                        // allow for the category to potentially still exist
                        category.Types.RemoveAll(o => typesToBeRemoved.Contains(o.Type));
                    }

                    Helpers.Assert.AtLeastOneTypeRequired(category.Types.Select(o => o.Type).ToList());

                    // Validate buildings
                    Helpers.Assert.AtLeastOneBuildingRequired(category.Buildings);
                    Helpers.Assert.BuildingsExist(category.Buildings);

                    if (category.CanBePurchased())
                    {
                        // Validate name and description
                        Helpers.Assert.ValidStringLength("name", category.AnimalShop.Name, 1);
                        Helpers.Assert.ValidStringLength("description", category.AnimalShop.Description, 1);

                        // Validate shop icon
                        Helpers.Assert.FileExists(category.AnimalShop.Icon);
                        Helpers.Assert.ValidFileExtension(category.AnimalShop.Icon, Constants.Mod.AnimalShopIconExtension);

                        // Validate excluded types
                        if (category.AnimalShop.Exclude != null)
                        {
                            Helpers.Assert.FarmAnimalTypesExist(category.AnimalShop.Exclude);
                        }
                    }
                }
                catch (Exception exception)
                {
                    monitor.Log($"{category.Category} category will not load: {exception.Message}", LogLevel.Warn);

                    categoriesToBeRemoved.Add(category.Category);
                }
            }

            if (categoriesToBeRemoved.Any())
            {
                // Remove it from the cache for this session
                // i.e. Cache gets reloaded every time the game is started
                cache.Categories.RemoveAll(o => categoriesToBeRemoved.Contains(o.Category));
            }

            // Write back the cache
            Helpers.FarmAnimals.Write(cache);
        }
        
        public static void SeedCacheWithVanillaFarmAnimals()
        {
            // Seed with all of the vanilla farm animals
            List<Cache.FarmAnimalCategory> categories = Helpers.FarmAnimals.GetVanillaCategories();

            // Reset the cache
            Cache.FarmAnimals cache = new Cache.FarmAnimals(categories);

            // Commit the seed
            Helpers.FarmAnimals.Write(cache);
        }
    }
}
