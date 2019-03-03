using Microsoft.Xna.Framework.Graphics;
using System.IO;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Cache
{
    class FarmAnimalCategory
    {
        public string AssetSourceDirectory;
        public Config.FarmAnimalCategory Category;

        public FarmAnimalCategory(string assetSourceDirectory, Config.FarmAnimalCategory category)
        {
            this.AssetSourceDirectory = assetSourceDirectory;
            this.Category = category;
        }

        public string GetAssetPath(string asset)
        {
            return Path.Combine(this.AssetSourceDirectory, asset);
        }

        public Texture2D GetAnimalShopIconTexture()
        {
            return this.Category.CanBePurchased() ? PariteeCore.Api.Mod.LoadTexture(this.GetAssetPath(this.Category.AnimalShop.Icon)) : null;
        }
    }
}
