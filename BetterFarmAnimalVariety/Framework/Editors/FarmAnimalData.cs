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
            }
        }

        private string SanitizeData(string data)
        {
            string[] values = PariteeCore.Api.Content.ParseDataValue(data);

            values[(int)PariteeCore.Constants.FarmAnimal.DataValueIndex.DefaultProduce] = this.SanitizeProduce(values[(int)PariteeCore.Constants.FarmAnimal.DataValueIndex.DefaultProduce]);
            values[(int)PariteeCore.Constants.FarmAnimal.DataValueIndex.DeluxeProduce] = this.SanitizeProduce(values[(int)PariteeCore.Constants.FarmAnimal.DataValueIndex.DeluxeProduce]);

            return string.Join(PariteeCore.Constants.Content.DataValueDelimiter.ToString(), values);
        }

        private string SanitizeProduce(string produceIndexStr)
        {
            try
            {
                // Assert that the produce is a valid object ...
                Helpers.Assert.ValidFarmAnimalProduce(this.Helper, produceIndexStr, out int produceIndex);

                produceIndexStr = produceIndex.ToString();
            }
            catch (Exceptions.SaveNotLoadedException e)
            {
                string noProduceIndex = PariteeCore.Constants.FarmAnimal.NoProduce.ToString();

                this.Monitor.Log($"Cannot replace \"{produceIndexStr}\" produce: {e.Message}. Produce will be temporarily set to \"none\" ({noProduceIndex}).", LogLevel.Debug);

                // ... otherwise; default to "no produce"
                produceIndexStr = noProduceIndex;
            }
            catch (Exception e)
            {
                string noProduceIndex = PariteeCore.Constants.FarmAnimal.NoProduce.ToString();

                this.Monitor.Log($"Cannot replace \"{produceIndexStr}\" produce: {e.Message}. Produce will be set to \"none\" ({noProduceIndex}).", LogLevel.Debug);

                // ... otherwise; default to "no produce"
                produceIndexStr = noProduceIndex;
            }

            return produceIndexStr;
        }
    }
}
