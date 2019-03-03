using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StardewValley;
using System.IO;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Cache
{
    class FarmAnimalCategory
    {
        [JsonProperty(Order = 0)]
        public string Category;

        [JsonProperty(Order = 1)]
        public string[] Types;

        [JsonProperty(Order = 2)]
        public string[] Buildings;

        [JsonProperty(Order = 3)]
        public FarmAnimalStock AnimalShop;

        [JsonProperty(Order = 4)]
        public string AssetSourceDirectory;

        public FarmAnimalCategory() { }

        public FarmAnimalCategory(string assetSourceDirectory, ContentPacks.FarmAnimalCategory category)
        {
            this.AssetSourceDirectory = assetSourceDirectory;
            this.Category = category.Category;
            this.Types = category.Types;
            this.Buildings = category.Buildings;
            this.AnimalShop = category.AnimalShop;
        }

        public FarmAnimalCategory(string assetSourceDirectory, PariteeCore.Models.FarmAnimalCategory farmAnimalStock)
        {
            this.AssetSourceDirectory = assetSourceDirectory;
            this.Category = farmAnimalStock.ToString();
            this.Types = farmAnimalStock.Types.Select(o => o.ToString()).ToArray();
            this.Buildings = farmAnimalStock.Buildings;
            this.AnimalShop = farmAnimalStock.CanBePurchased()
                ? new FarmAnimalStock(farmAnimalStock)
                : null;
        }

        public string GetAssetPath(string asset)
        {
            return Path.Combine(this.AssetSourceDirectory, asset);
        }

        public Texture2D GetAnimalShopIconTexture()
        {
            return this.CanBePurchased() ? PariteeCore.Api.Mod.LoadTexture(this.GetAssetPath(this.AnimalShop.Icon)) : null;
        }

        public bool CanBePurchased()
        {
            return this.AnimalShop != null;
        }

        public bool CanBePurchased(string type)
        {
            if (!this.CanBePurchased())
            {
                return false;
            }

            return !this.AnimalShop.Exclude.Contains(type);
        }

        public StardewValley.Object ToAnimalAvailableForPurchase(Farm farm)
        {
            if (!this.CanBePurchased())
            {
                return null;
            }

            return PariteeCore.Api.AnimalShop.FormatAsAnimalAvailableForPurchase(farm, this.Category, this.AnimalShop.Name, this.AnimalShop.Price, this.Buildings);
        }
    }
}
