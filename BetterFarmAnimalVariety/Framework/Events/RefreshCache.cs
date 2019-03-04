﻿using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Events
{
    class RefreshCache
    {
        public static void ValidateCachedFarmAnimals(IMonitor monitor)
        {
            // Validate the cached animals
            foreach (Cache.FarmAnimalCategory category in Helpers.FarmAnimals.GetCategories())
            {
                try
                {
                    // Validate category
                    Helpers.Assert.ValidStringLength("category", category.Category, 1);

                    // Validate types
                    Helpers.Assert.FarmAnimalTypesExist(category.Types.Select(o => o.Type).ToList());

                    // Validate buildings
                    Helpers.Assert.BuildingsExist(category.Buildings.ToList());

                    if (category.CanBePurchased())
                    {
                        // Validate name and description
                        Helpers.Assert.ValidStringLength("name", category.AnimalShop.Name, 1);
                        Helpers.Assert.ValidStringLength("description", category.AnimalShop.Description, 1);

                        // Validate price
                        Helpers.Assert.ValidMoneyAmount(category.AnimalShop.Price);

                        // Validate shop icon
                        Helpers.Assert.FileExists(category.AnimalShop.Icon);
                        Helpers.Assert.ValidFileExtension(category.AnimalShop.Icon, Constants.Mod.AnimalShopIconExtension);

                        // Validate excluded types
                        Helpers.Assert.FarmAnimalTypesExist(category.AnimalShop.Exclude.ToList());
                    }
                }
                catch (Exception exception)
                {
                    monitor.Log($"{category.Category} will not load: {exception.Message}", LogLevel.Warn);

                    // Remove it from the cache for this session
                    // i.e. Cache gets reloaded every time the game is started
                    Helpers.FarmAnimals.RemoveCategory(category.Category);
                }
            }
        }

        public static void SeedCacheWithVanillaFarmAnimals()
        {
            // Seed with all of the vanilla farm animals
            List<Cache.FarmAnimalCategory> categories = PariteeCore.Constants.VanillaFarmAnimalCategory.All()
                .Select(o => new Cache.FarmAnimalCategory(PariteeCore.Constants.Mod.Path, o))
                .ToList();

            // Reset the cache
            Cache.FarmAnimals cache = new Cache.FarmAnimals(categories);

            // Commit the seed
            Helpers.FarmAnimals.Write(cache);
        }
    }
}
