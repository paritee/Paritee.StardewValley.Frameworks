using StardewModdingAPI;
using StardewModdingAPI.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Events
{
    class LoadContentPacks
    {
        public static void OnGameLaunched(GameLaunchedEventArgs e, IEnumerable<IContentPack> contentPacks, IMonitor monitor)
        {
            LoadContentPacks.SetUpContentPacks(contentPacks, monitor);
            LoadContentPacks.ValidateCachedFarmAnimals(monitor);
        }

        private static void SetUpContentPacks(IEnumerable<IContentPack> contentPacks, IMonitor monitor)
        {
            foreach (IContentPack contentPack in contentPacks)
            {
                monitor.Log($"Reading content pack: {contentPack.Manifest.Name} {contentPack.Manifest.Version} from {contentPack.DirectoryPath}", LogLevel.Trace);

                try
                {
                    // Check if the content JSON is there
                    if (!File.Exists(Path.Combine(contentPack.DirectoryPath, Constants.Mod.ContentPackFileName)))
                    {
                        throw new FileNotFoundException($"{Constants.Mod.ContentPackFileName} not found.");
                    }

                    // Read the content
                    ContentPacks.FarmAnimals content = contentPack.ReadJsonFile<ContentPacks.FarmAnimals>(Constants.Mod.ContentPackFileName);

                    content.SetUp(contentPack);
                }
                catch (Exception exception)
                {
                    monitor.Log($"{contentPack.Manifest.Name} will not load: " + exception.Message, LogLevel.Warn);
                }
            }
        }

        private static void ValidateCachedFarmAnimals(IMonitor monitor)
        {
            // Validate the cached animals
            foreach (Cache.FarmAnimalCategory category in Helpers.FarmAnimals.GetCategories())
            {
                try
                {
                    // Validate category
                    Helpers.Assert.ValidStringLength("category", category.Category.Category, 1);

                    // Validate types
                    Helpers.Assert.FarmAnimalTypesExist(category.Category.Types.ToList());

                    // Validate buildings
                    Helpers.Assert.BuildingsExist(category.Category.Buildings.ToList());

                    if (category.Category.CanBePurchased())
                    {
                        // Validate name and description
                        Helpers.Assert.ValidStringLength("name", category.Category.AnimalShop.Name, 1);
                        Helpers.Assert.ValidStringLength("description", category.Category.AnimalShop.Description, 1);

                        // Validate price
                        Helpers.Assert.ValidMoneyAmount(category.Category.AnimalShop.Price);

                        // Validate shop icon
                        Helpers.Assert.FileExists(category.GetAssetPath(category.Category.AnimalShop.Icon));
                        Helpers.Assert.ValidFileExtension(category.Category.AnimalShop.Icon, Constants.Mod.AnimalShopIconExtension);

                        // Validate excluded types
                        Helpers.Assert.FarmAnimalTypesExist(category.Category.AnimalShop.Exclude.ToList());
                    }
                }
                catch (Exception exception)
                {
                    monitor.Log($"{category.Category.Category} will not load: {exception.Message}", LogLevel.Warn);

                    // Remove it from the cache for this session
                    // i.e. Cache gets reloaded every time the game is started
                    Helpers.FarmAnimals.RemoveCategory(category.Category.Category);
                }
            }
        }
    }
}
