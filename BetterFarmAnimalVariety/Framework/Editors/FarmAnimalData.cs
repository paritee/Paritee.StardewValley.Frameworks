using StardewModdingAPI;
using System.Collections.Generic;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Editors
{
    class FarmAnimalData : IAssetEditor
    {

        /// <summary>Get whether this instance can edit the given asset.</summary>
        /// <param name="asset">Basic metadata about the asset being loaded.</param>
        public bool CanEdit<T>(IAssetInfo asset)
        {
            // Check if trying to access the Data/FarmAnimals
            if (asset.AssetNameEquals(PariteeCore.Constants.Content.DataFarmAnimalsContentPath))
            {
                return true;
            }

            return false;
        }

        /// <summary>Edit a matched asset.</summary>
        /// <param name="asset">A helper which encapsulates metadata about an asset and enables changes to it.</param>
        public void Edit<T>(IAssetData asset)
        {
            if (asset.AssetNameEquals(PariteeCore.Constants.Content.DataFarmAnimalsContentPath))
            {
                IDictionary<string, string> data = asset.AsDictionary<string, string>().Data;

                foreach (Cache.FarmAnimalType type in Helpers.FarmAnimals.GetCategories().SelectMany(o => o.Types))
                {
                    if (type.Data == null)
                    {
                        continue;
                    }

                    // Adjust for localization and add to the dictionary
                    data[type.Type] = type.LocalizeData(asset.Locale);
                }
            }
        }
    }
}
