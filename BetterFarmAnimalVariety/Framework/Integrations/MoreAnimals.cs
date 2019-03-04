using System;
using System.Collections.Generic;
using StardewModdingAPI;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Integrations
{
    class MoreAnimals : Integration
    {
        public static string Key => Constants.Integration.MoreAnimals;
        public static Type ApiInterface => typeof(IMoreAnimalsAPI);

        private readonly IMoreAnimalsAPI Api;

        public MoreAnimals(IMoreAnimalsAPI api)
        {
            this.Api = api;
        }

        public override void SetUp(IMonitor monitor)
        {
            // Register the animal types from Data/FarmAnimals
            Dictionary<string, string> entries = PariteeCore.Api.Content.LoadData<string, string>(PariteeCore.Constants.Content.DataFarmAnimalsContentPath);

            foreach (KeyValuePair<string, string> entry in entries)
            {
                this.RegisterAnimalType(entry, out bool hasBaby, out bool canShear);

                monitor.Log($"Registered {entry.Key} (hasBaby:{hasBaby}, canShear:{canShear}) with {Constants.Integration.MoreAnimals}", LogLevel.Trace);
            }
        }

        private bool HasBabyAsset(string type, string[] dataValues)
        {
            return PariteeCore.Api.FarmAnimal.TryBuildSpriteAssetName(type, true, false, out string assetName);
        }

        private bool HasShearedAsset(string type, string[] dataValues)
        {
            if (!bool.TryParse(dataValues[(int)PariteeCore.Constants.FarmAnimal.DataValueIndex.ShowDifferentTextureWhenReadyForHarvest], out bool showDifferentTextureWhenReadyForHarvest) || !showDifferentTextureWhenReadyForHarvest)
            {
                return false;
            }

            return PariteeCore.Api.FarmAnimal.TryBuildSpriteAssetName(type, false, true, out string assetName);
        }

        private void RegisterAnimalType(KeyValuePair<string, string> entry, out bool hasBaby, out bool canShear)
        {
            string[] values = PariteeCore.Api.Content.ParseDataValue(entry.Value);

            // Check if the baby asset exists
            hasBaby = this.HasBabyAsset(entry.Key, values);

            // Check if the sheared asset exists
            canShear = this.HasShearedAsset(entry.Key, values);

            // Register the type with MoreAnimals
            this.Api.RegisterAnimalType(entry.Key, hasBaby, canShear);
        }
    }
}
