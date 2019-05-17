using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;

namespace BetterFarmAnimalVariety
{
    public class FarmAnimalAssetLoader : IAssetLoader
    {
        public string AssetName;
        public Texture2D Asset;
        public FarmAnimalAssetLoader(string assetName, Texture2D asset)
        {
            AssetName = assetName;
            Asset = asset;
        }
        public bool CanLoad<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals(AssetName);
        }

        public T Load<T>(IAssetInfo asset)
        {
            return (T) (object) Asset;
        }
    }
}
