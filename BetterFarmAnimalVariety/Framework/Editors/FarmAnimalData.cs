using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Editors
{
    class FarmAnimalData : IAssetEditor
    {
        private readonly IModHelper Helper;
        private readonly IMonitor Monitor;

        public FarmAnimalData(IModHelper helper, IMonitor monitor)
        {
            this.Helper = helper;
            this.Monitor = monitor;
        }

        /// <summary>Get whether this instance can edit the given asset.</summary>
        /// <param name="asset">Basic metadata about the asset being loaded.</param>
        public bool CanEdit<T>(IAssetInfo asset)
        {
            // Check if trying to access the Data/FarmAnimals
            if (asset.AssetNameEquals(PariteeCore.Utilities.Content.DataFarmAnimalsContentPath))
            {
                return true;
            }

            return false;
        }

        /// <summary>Edit a matched asset.</summary>
        /// <param name="asset">A helper which encapsulates metadata about an asset and enables changes to it.</param>
        public void Edit<T>(IAssetData asset)
        {
            if (asset.AssetNameEquals(PariteeCore.Utilities.Content.DataFarmAnimalsContentPath))
            {
                IDictionary<string, string> data = asset.AsDictionary<string, string>().Data;
                
                // Always use the first lang parameter (ex. zh-CN)
                string locale = asset.Locale.Split('-')[0];

                foreach (Cache.FarmAnimalType type in Helpers.FarmAnimals.GetCategories().SelectMany(o => o.Types))
                {
                    if (type.Data == null)
                    {
                        continue;
                    }

                    // Adjust for localization and add to the dictionary
                    data[type.Type] = type.LocalizeData(locale);

                    // Integrate with JsonAssets
                    data[type.Type] = this.SanitizeData(type.Data);
                }

                this.Monitor.Log($"Data/FarmAnimals:\n{string.Join("\n", data.Select(o => $"{o.Key}: {o.Value}"))}", LogLevel.Trace);
            }
        }

        private string SanitizeData(string data)
        {
            string[] values = PariteeCore.Utilities.Content.ParseDataValue(data);

            values[(int)PariteeCore.Characters.FarmAnimal.DataValueIndex.DefaultProduce] = this.SanitizeItems(values[(int)PariteeCore.Characters.FarmAnimal.DataValueIndex.DefaultProduce]);
            values[(int)PariteeCore.Characters.FarmAnimal.DataValueIndex.DeluxeProduce] = this.SanitizeItems(values[(int)PariteeCore.Characters.FarmAnimal.DataValueIndex.DeluxeProduce]);
            values[(int)PariteeCore.Characters.FarmAnimal.DataValueIndex.MeatIndex] = this.SanitizeItems(values[(int)PariteeCore.Characters.FarmAnimal.DataValueIndex.MeatIndex]);

            return string.Join(PariteeCore.Utilities.Content.DataValueDelimiter.ToString(), values);
        }

        private string SanitizeItems(string indexStr)
        {
            string noIndex = PariteeCore.Characters.FarmAnimal.NoProduce.ToString();

            try
            {
                // Assert that the item is a valid object ...
                Helpers.Assert.ValidObject(this.Helper, indexStr, out int index);

                indexStr = index.ToString();
            }
            catch (Exceptions.SaveNotLoadedException e)
            {
                this.Monitor.Log($"Cannot replace \"{indexStr}\" produce: {e.Message}. Will be temporarily set to \"none\" ({noIndex}).", LogLevel.Trace);

                // ... otherwise; default to "no produce"
                indexStr = noIndex;
            }
            catch (Exception e)
            {
                this.Monitor.Log($"Cannot replace \"{indexStr}\" produce: {e.Message}. Will be set to \"none\" ({noIndex}).", LogLevel.Debug);

                // ... otherwise; default to "no produce"
                indexStr = noIndex;
            }

            return indexStr;
        }
    }
}
