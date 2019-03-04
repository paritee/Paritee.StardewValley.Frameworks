using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.ContentPacks
{
    class FarmAnimals
    {
        public List<ContentPacks.FarmAnimalCategory> Categories;

        public FarmAnimals() { }

        public FarmAnimals(List<ContentPacks.FarmAnimalCategory> categories)
        {
            this.Categories = categories;
        }

        public void SetUp(IContentPack contentPack)
        {
            // Go through each category
            foreach (ContentPacks.FarmAnimalCategory category in this.Categories)
            {
                // Handle the actions
                switch (category.Action)
                {
                    case ContentPacks.FarmAnimalCategory.Actions.Create:
                        this.HandleCreateAction(contentPack, category);
                        break;

                    case ContentPacks.FarmAnimalCategory.Actions.Update:
                        this.HandleUpdateAction(contentPack, category);
                        break;

                    case ContentPacks.FarmAnimalCategory.Actions.Remove:
                        this.HandleRemoveAction(contentPack, category);
                        break;

                    default:
                        throw new NotSupportedException($"{category.Action} is not a valid action");
                }
            }
        }

        private Cache.FarmAnimalType CastSpritesToFullPaths(Cache.FarmAnimalType type, string directoryPath)
        {
            if (type.AdultSprite != null)
            {
                type.AdultSprite = Path.Combine(directoryPath, type.AdultSprite);
            }

            if (type.BabySprite != null)
            {
                type.BabySprite = Path.Combine(directoryPath, type.BabySprite);
            }

            if (type.ShearedSprite != null)
            {
                type.ShearedSprite = Path.Combine(directoryPath, type.ShearedSprite);
            }

            return type;
        }

        public void HandleCreateAction(IContentPack contentPack, ContentPacks.FarmAnimalCategory category)
        {
            // Assert unique category
            Helpers.Assert.UniqueFarmAnimalCategory(category.Category);

            // Need to modify the sprite paths
            category.Types = category.Types.Select(o => this.CastSpritesToFullPaths(o, contentPack.DirectoryPath)).ToList();

            Helpers.FarmAnimals.AddOrReplaceCategory(new Cache.FarmAnimalCategory(contentPack.DirectoryPath, category));
        }

        public void HandleUpdateAction(IContentPack contentPack, ContentPacks.FarmAnimalCategory category)
        {
            // Assert existing category
            Helpers.Assert.FarmAnimalCategoryExists(category.Category);

            Cache.FarmAnimalCategory cacheCategory = Helpers.FarmAnimals.GetCategory(category.Category);

            // Add the missing types
            if (category.Types != null)
            {
                // Need to modify the sprite paths
                List<Cache.FarmAnimalType> types = category.Types.Select(o => this.CastSpritesToFullPaths(o, contentPack.DirectoryPath)).ToList();

                if (category.ForceOverrideTypes)
                {
                    cacheCategory.Types = types;
                }
                else
                {
                    foreach (Cache.FarmAnimalType type in types)
                    {
                        Cache.FarmAnimalType cacheCategoryType = cacheCategory.Types.FirstOrDefault(o => o.Type == type.Type);

                        if (cacheCategoryType != null)
                        {
                            cacheCategoryType = type;
                        }
                        else
                        {
                            cacheCategory.Types.Add(type);
                        }
                    }
                }
            }

            // Add the missing buildings
            if (category.Buildings != null)
            {
                cacheCategory.Buildings = category.ForceOverrideBuildings 
                    ? category.Buildings
                    : cacheCategory.Buildings.Union(category.Buildings).ToList();
            }

            // Check if the force remove from shop flag was used
            if (category.ForceRemoveFromShop)
            {
                cacheCategory.AnimalShop = null;
            }
            // Only update the animal shop properties if it was explicitly stated
            else if (category.AnimalShop != null)
            {
                // If the category couldn't be purchased before, set it to be purchased
                if (!cacheCategory.CanBePurchased())
                {
                    cacheCategory.AnimalShop = new Cache.FarmAnimalStock();
                }

                if (category.AnimalShop.Name != null)
                {
                    cacheCategory.AnimalShop.Name = category.AnimalShop.Name;
                }

                if (category.AnimalShop.Description != null)
                {
                    cacheCategory.AnimalShop.Description = category.AnimalShop.Description;
                }

                if (category.AnimalShop.Icon != null)
                {
                    cacheCategory.AnimalShop.Icon = Path.Combine(contentPack.DirectoryPath, category.AnimalShop.Icon);
                }

                if (category.AnimalShop.Price != default(int))
                {
                    cacheCategory.AnimalShop.Price = category.AnimalShop.Price;
                }

                if (category.AnimalShop.Exclude != null)
                {
                    cacheCategory.AnimalShop.Exclude = category.ForceOverrideExclude
                        ? category.AnimalShop.Exclude
                        : cacheCategory.AnimalShop.Exclude.Union(category.AnimalShop.Exclude).ToList();
                }
            }

            Helpers.FarmAnimals.AddOrReplaceCategory(cacheCategory);
        }

        public void HandleRemoveAction(IContentPack contentPack, ContentPacks.FarmAnimalCategory category)
        {
            // Assert unique category
            Helpers.Assert.FarmAnimalCategoryExists(category.Category);

            Helpers.FarmAnimals.RemoveCategory(category.Category);
        }
    }
}
