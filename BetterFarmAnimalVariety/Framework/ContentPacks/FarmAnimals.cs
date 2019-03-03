using StardewModdingAPI;
using System;
using System.Collections.Generic;
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
            foreach (Framework.ContentPacks.FarmAnimalCategory category in this.Categories)
            {
                // Handle the actions
                switch (category.Action)
                {
                    case Framework.ContentPacks.FarmAnimalCategory.Actions.Create:
                        this.HandleCreateAction(contentPack, category);
                        break;

                    case Framework.ContentPacks.FarmAnimalCategory.Actions.Update:
                        this.HandleUpdateAction(contentPack, category);
                        break;

                    case Framework.ContentPacks.FarmAnimalCategory.Actions.Remove:
                        this.HandleRemoveAction(contentPack, category);
                        break;

                    default:
                        throw new NotSupportedException($"{category.Action} is not a valid action");
                }
            }
        }

        public void HandleCreateAction(IContentPack contentPack, ContentPacks.FarmAnimalCategory category)
        {
            // Assert unique category
            Helpers.Assert.UniqueFarmAnimalCategory(category.Category);

            // Set up the animal shop properties
            Config.FarmAnimalStock animalShop;

            if (category.AnimalShop != null)
            {
                animalShop = new Config.FarmAnimalStock
                {
                    Name = category.AnimalShop.Name,
                    Description = category.AnimalShop.Description,
                    Icon = category.AnimalShop.Icon,
                    Price = category.AnimalShop.Price,
                    Exclude = category.AnimalShop.Exclude
                };
            }
            else
            {
                animalShop = null;
            }

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
                cacheCategory.Category.Types = category.ForceOverrideTypes
                    ? category.Types
                    : cacheCategory.Category.Types.Union(category.Types).ToArray();
            }

            // Add the missing buildings
            if (category.Buildings != null)
            {
                cacheCategory.Category.Buildings = category.ForceOverrideBuildings 
                    ? category.Buildings
                    : cacheCategory.Category.Buildings.Union(category.Buildings).ToArray();
            }

            // Check if the force remove from shop flag was used
            if (category.ForceRemoveFromShop)
            {
                cacheCategory.Category.AnimalShop = null;
            }
            // Only update the animal shop properties if it was explicitly stated
            else if (category.AnimalShop != null)
            {
                // If the category couldn't be purchased before, set it to be purchased
                if (!cacheCategory.Category.CanBePurchased())
                {
                    cacheCategory.Category.AnimalShop = new Config.FarmAnimalStock();
                }

                if (category.AnimalShop.Name != null)
                {
                    cacheCategory.Category.AnimalShop.Name = category.AnimalShop.Name;
                }

                if (category.AnimalShop.Description != null)
                {
                    cacheCategory.Category.AnimalShop.Description = category.AnimalShop.Description;
                }

                if (category.AnimalShop.Icon != null)
                {
                    cacheCategory.Category.AnimalShop.Icon = category.AnimalShop.Icon;

                    // Also update the asset source
                    cacheCategory.AssetSourceDirectory = contentPack.DirectoryPath;
                }

                if (category.AnimalShop.Price != default(int))
                {
                    cacheCategory.Category.AnimalShop.Price = category.AnimalShop.Price;
                }

                if (category.AnimalShop.Exclude != null)
                {
                    cacheCategory.Category.AnimalShop.Exclude = category.ForceOverrideExclude
                        ? category.AnimalShop.Exclude
                        : cacheCategory.Category.Types.Union(category.AnimalShop.Exclude).ToArray();
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
