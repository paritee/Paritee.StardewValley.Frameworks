using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Config
{
    public class FarmAnimalStock
    {
        public string Category;

        [JsonProperty(Order = 1)]
        public string Name;

        [JsonProperty(Order = 2)]
        public string Description;

        [JsonProperty(Order = 3)]
        public string Icon;

        [JsonProperty(Order = 4)]
        public int Price;

        [JsonProperty(Order = 5)]
        public string[] Exclude;

        [JsonConstructor]
        public FarmAnimalStock()
        {
            // Do nothing; this is for loading an existing config
        }

        public FarmAnimalStock(Constants.FarmAnimalCategory farmAnimalStock)
        {
            this.Category = farmAnimalStock.ToString();
            this.Name = farmAnimalStock.DisplayName;
            this.Description = farmAnimalStock.Description;
            this.Icon = this.GetDefaultIconPath();
            this.Price = farmAnimalStock.Price;
            this.Exclude = farmAnimalStock.ExcludeFromShop.Select(o => o.ToString()).ToArray();
        }

        public bool ShouldSerializeCategory()
        {
            return false;
        }

        public string GetDefaultIconPath()
        {
            return this.FormatIconPath($"{this.Category.Replace(" ", "")}.png");
        }

        private string FormatIconPath(string fileName)
        {
            return Path.Combine(Constants.Mod.AssetsDirectory, "AnimalShop", fileName);
        }
    }
}
