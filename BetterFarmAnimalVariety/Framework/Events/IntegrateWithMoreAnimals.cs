using StardewModdingAPI;
using System.Collections.Generic;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Events
{
    class IntegrateWithMoreAnimals
    {
        public static void RegisterAnimals(IModHelper helper, IMonitor monitor)
        {
            Helpers.Assert.ApiExists(helper, Constants.Integration.MoreAnimals, out Api.IMoreAnimals api);

            // Register the animal types from Data/FarmAnimals
            Dictionary<string, string> entries = PariteeCore.Api.Content.LoadData<string, string>(PariteeCore.Constants.Content.DataFarmAnimalsContentPath);

            foreach (KeyValuePair<string, string> entry in entries)
            {
                IntegrateWithMoreAnimals.RegisterAnimalType(api, entry, out bool hasBaby, out bool canShear);

                monitor.Log($"Registered {entry.Key} (hasBaby:{hasBaby}, canShear:{canShear}) with {Constants.Integration.MoreAnimals}", LogLevel.Trace);
            }
        }

        private static bool HasBabyAsset(string type, string[] dataValues)
        {
            return PariteeCore.Api.FarmAnimal.TryBuildSpriteAssetName(type, true, false, out string assetName);
        }

        private static bool HasShearedAsset(string type, string[] dataValues)
        {
            if (!bool.TryParse(dataValues[(int)PariteeCore.Constants.FarmAnimal.DataValueIndex.ShowDifferentTextureWhenReadyForHarvest], out bool showDifferentTextureWhenReadyForHarvest) || !showDifferentTextureWhenReadyForHarvest)
            {
                return false;
            }

            return PariteeCore.Api.FarmAnimal.TryBuildSpriteAssetName(type, false, true, out string assetName);
        }

        private static void RegisterAnimalType(Api.IMoreAnimals api, KeyValuePair<string, string> entry, out bool hasBaby, out bool canShear)
        {
            string[] values = PariteeCore.Api.Content.ParseDataValue(entry.Value);

            // Check if the baby asset exists
            hasBaby = IntegrateWithMoreAnimals.HasBabyAsset(entry.Key, values);

            // Check if the sheared asset exists
            canShear = IntegrateWithMoreAnimals.HasShearedAsset(entry.Key, values);

            // Register the type with MoreAnimals
            api.RegisterAnimalType(entry.Key, hasBaby, canShear);
        }
    }
}
