using StardewModdingAPI;

namespace BetterFarmAnimalVariety
{
    public class FarmAnimalAssetEditor : IAssetEditor
    {
        public string AssetName;
        public string AssetData;

        public FarmAnimalAssetEditor(string assetName, string assetData)
        {
            AssetName = assetName;
            AssetData = assetData;
        }
        public bool CanEdit<T>(IAssetInfo asset)
        {
           return asset.AssetNameEquals("Data/FarmAnimals");
        }

        public void Edit<T>(IAssetData asset)
        {
            asset.AsDictionary<string,string>().Data.Add(AssetName, AssetData);
        }
    }
}
