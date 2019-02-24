using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

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

        public FarmAnimalStock(PariteeCore.Models.FarmAnimalCategory farmAnimalStock)
        {
            this.Name = farmAnimalStock.DisplayName;
            this.Description = farmAnimalStock.Description;
            this.Icon = this.GetDefaultIconPath(farmAnimalStock.ToString());
            this.Price = farmAnimalStock.Price;
            this.Exclude = farmAnimalStock.ExcludeFromShop.Any() 
                ? farmAnimalStock.ExcludeFromShop.Select(o => o.ToString()).ToArray() 
                : farmAnimalStock.ExcludeFromShop;
        }

        public Texture2D GetIconTexture()
        {
            return Helpers.Mod.LoadTexture(this.Icon);
        }

        public string GetDefaultIconPath(string category)
        {
            return this.FormatIconPath($"{category.Replace(" ", "")}{Constants.Mod.AnimalShopIconExtension}");
        }

        private string FormatIconPath(string fileName)
        {
            return Helpers.Mod.GetShortAssetPath(Path.Combine(Constants.Mod.AnimalShopIconDirectory, fileName));
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

            Helpers.Assert.ValidAnimalShopIcon(placeholder.Icon);

            return placeholder;
        }
    }
}
