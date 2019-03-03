using StardewModdingAPI;
using System.Collections.Generic;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety
{
    public class ModConfig
    {
        public string Format;
        public bool IsEnabled;
        public List<Framework.Config.FarmAnimalCategory> Categories;

        public ModConfig()
        {
            this.Format = null;
            this.IsEnabled = true;
            this.Categories = new List<Framework.Config.FarmAnimalCategory>();
        }

        public void Write(IModHelper helper)
        {
            helper.WriteConfig(this);
        }

        public Framework.Config.FarmAnimalCategory GetCategory(string category)
        {
            return this.Categories.FirstOrDefault(o => o.Category.Equals(category));
        }

        public bool HasCategories()
        {
            return this.Categories.Any();
        }

        public void AddCategory(Framework.Config.FarmAnimalCategory category)
        {
            this.Categories.Add(category);
        }

        public void RemoveCategory(string category)
        {
            this.Categories.RemoveAll(o => o.Category.Equals(category));
        }

        public void AssertValidFormat(string targetFormat)
        {
            Framework.Helpers.Assert.VersionIsSupported(this.Format, targetFormat);
        }

        public void SeedVanillaFarmAnimals()
        {
            this.Categories = PariteeCore.Constants.VanillaFarmAnimalCategory.All()
                .Select(o => new Framework.Config.FarmAnimalCategory(o))
                .ToList();
        }
    }
}