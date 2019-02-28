using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Integrations
{
    class MoreAnimals : Integration
    {
        public static string Key => Framework.Constants.Integration.MoreAnimals;
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
                bool hasBaby = false, canShear, isCoopDweller;
                string buildingTypeILiveIn, babySprite, shearedSprite;

                string[] values = PariteeCore.Api.Content.ParseDataValue(entry.Value);

                // Check if the baby asset exists
                buildingTypeILiveIn = values[(int)PariteeCore.Constants.FarmAnimal.DataValueIndex.BuildingTypeILiveIn];
                isCoopDweller = PariteeCore.Api.FarmAnimal.IsCoopDweller(buildingTypeILiveIn);
                babySprite = PariteeCore.Api.FarmAnimal.BuildSpriteAssetName(entry.Value, true, false, isCoopDweller);
                hasBaby = PariteeCore.Api.Content.Exists<Texture2D>(babySprite);

                // Check if the sheared asset exists
                canShear = false;

                if (bool.TryParse(values[(int)PariteeCore.Constants.FarmAnimal.DataValueIndex.ShowDifferentTextureWhenReadyForHarvest], out bool showDifferentTextureWhenReadyForHarvest) && showDifferentTextureWhenReadyForHarvest)
                {
                    shearedSprite = PariteeCore.Api.FarmAnimal.BuildSpriteAssetName(entry.Value, false, true, isCoopDweller);

                    canShear = PariteeCore.Api.Content.Exists<Texture2D>(shearedSprite);
                }

                // Register the type with MoreAnimals
                this.Api.RegisterAnimalType(entry.Key, hasBaby, canShear);

                monitor.Log($"Registered {entry.Key} (hasBaby:{hasBaby}, canShear:{canShear}) with {Framework.Constants.Integration.MoreAnimals}", LogLevel.Trace);
            }
        }

        public void RegisterAnimalType(string type, bool isBaby, bool canShear)
        {

        }
    }
}
