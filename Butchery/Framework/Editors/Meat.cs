using StardewModdingAPI;
using System.Collections.Generic;

namespace Butchery.Framework.Editors
{
    class Meat : IAssetEditor
    {
        /// <summary>Get whether this instance can edit the given asset.</summary>
        /// <param name="asset">Basic metadata about the asset being loaded.</param>
        public bool CanEdit<T>(IAssetInfo asset)
        {
            if (asset.AssetNameEquals("Data/ObjectInformation"))
            {
                return true;
            }

            return false;
        }

        /// <summary>Edit a matched asset.</summary>
        /// <param name="asset">A helper which encapsulates metadata about an asset and enables changes to it.</param>
        public void Edit<T>(IAssetData asset)
        {
            if (asset.AssetNameEquals("Data/ObjectInformation"))
            {
                IDictionary<int, string> data = asset.AsDictionary<int, string>().Data;

                // Add the vanilla meat if it doesn't exist
                foreach (Constants.Meat meat in Constants.Meat.All())
                {
                    if (!data.ContainsKey(meat.Index))
                    {
                        data.Add(meat.Index, meat.FormatData());
                    }
                }
            }
        }
    }
}
