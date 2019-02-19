using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Config
{
    public class FarmAnimalStock
    {
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

        public FarmAnimalStock(Models.FarmAnimalCategory farmAnimalStock)
        {
            this.Name = farmAnimalStock.DisplayName;
            this.Description = farmAnimalStock.Description;
            this.Icon = this.GetDefaultIconPath(farmAnimalStock.ToString());
            this.Price = farmAnimalStock.Price;
            this.Exclude = farmAnimalStock.ExcludeFromShop.Select(o => o.ToString()).ToArray();
        }

        public Texture2D GetIconTexture()
        {
            return Api.Mod.LoadTexture(Path.Combine(Constants.Mod.Path, this.Icon));
        }

        public string GetDefaultIconPath(string category)
        {
            return this.FormatIconPath($"{category.Replace(" ", "")}.png");
        }

        private string FormatIconPath(string fileName)
        {
            return Path.Combine(Constants.Mod.AssetsDirectory, "AnimalShop", fileName);
        }

        public static FarmAnimalStock CreateWithPlaceholders(string category)
        {
            FarmAnimalStock placeholder = new FarmAnimalStock
            {
                Name = category,
                Description = Constants.Mod.AnimalShopDescriptionPlaceholder,
                Price = Constants.Mod.AnimalShopPricePlaceholder
            };

            placeholder.Icon = placeholder.GetDefaultIconPath(category);

            string fullPathToIcon = Path.Combine(Constants.Mod.Path, placeholder.Icon);

            if (!File.Exists(fullPathToIcon))
            {
                throw new FileNotFoundException($"{fullPathToIcon} does not exist");
            }

            return placeholder;
        }
    }
}
