using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Helpers
{
    class Mod
    {
        public static string SmapiSaveDataKey(string key)
        {
            return PariteeCore.Api.Mod.SmapiSaveDataKey(Constants.Mod.Key, key);
        }

        public static T ReadSaveData<T>(string saveDataKey) where T: new()
        {
            return PariteeCore.Api.Mod.ReadSaveData<T>(Constants.Mod.Key, saveDataKey);
        }

        public static void WriteSaveData<T>(string saveDataKey, T data)
        {
            PariteeCore.Api.Mod.WriteSaveData<T>(Constants.Mod.Key, saveDataKey, data);
        }

        public static T ReadConfig<T>()
        {
            return PariteeCore.Api.Mod.ReadConfig<T>(PariteeCore.Constants.Mod.Path, Constants.Mod.ConfigFileName);
        }

        public static Texture2D LoadTexture(string filePath)
        {
            return PariteeCore.Api.Mod.LoadTexture(Path.Combine(PariteeCore.Constants.Mod.Path, filePath));
        }

        public static string GetShortAssetPath(string filePath)
        {
            return Path.Combine(Constants.Mod.AssetsDirectory, filePath);
        }

        public static bool TryGetFullAssetPath(string filePath, out string path)
        {
            string assetPathToFile = Helpers.Mod.GetShortAssetPath(filePath);
            path = Path.Combine(PariteeCore.Constants.Mod.Path, assetPathToFile);

            return !File.Exists(path);
        }

        public static bool MigrateDeprecatedConfigToCurrentFormat<T>(T deprecatedConfig, string targetFormat, out ModConfig config)
        {
            if (deprecatedConfig is Framework.Config.V2.ModConfig)
            {
                Framework.Config.V2.ModConfig sourceConfig = (Framework.Config.V2.ModConfig)Convert.ChangeType(deprecatedConfig, typeof(Framework.Config.V2.ModConfig));

                config = new BetterFarmAnimalVariety.ModConfig
                {
                    Format = targetFormat,
                    IsEnabled = sourceConfig.IsEnabled
                };

                string voidChicken = PariteeCore.Constants.VanillaFarmAnimalType.VoidChicken.ToString();

                foreach (KeyValuePair<string, Framework.Config.V2.ConfigFarmAnimal> oldFarmAnimals in sourceConfig.FarmAnimals)
                {
                    Config.FarmAnimal animal = new Config.FarmAnimal
                    {
                        Category = oldFarmAnimals.Key,
                        Types = oldFarmAnimals.Value.Types,
                        Buildings = oldFarmAnimals.Value.Buildings
                    };

                    if (oldFarmAnimals.Value.CanBePurchased())
                    {
                        Int32.TryParse(oldFarmAnimals.Value.AnimalShop.Price, out int price);

                        string[] exclude = sourceConfig.IsChickenCategory(oldFarmAnimals.Key) 
                            && oldFarmAnimals.Value.Types.Contains(voidChicken) 
                            && sourceConfig.AreVoidFarmAnimalsInShopAlways()
                            ? new string[0]
                            : new string[] { voidChicken };

                        animal.AnimalShop = new Framework.Config.FarmAnimalStock
                        {
                            Name = oldFarmAnimals.Value.AnimalShop.Name,
                            Description = oldFarmAnimals.Value.AnimalShop.Description,
                            Icon = oldFarmAnimals.Value.AnimalShop.Icon,
                            Price = Math.Abs(price),
                            Exclude = exclude
                        };

                        // Check to make sure the icon exists...
                        string pathToIcon = Path.Combine(PariteeCore.Constants.Mod.Path, animal.AnimalShop.Icon);

                        // ... otherwise default it to the new icon assuming 
                        // the user copied them over
                        if (!File.Exists(pathToIcon))
                        {
                            animal.AnimalShop.Icon = animal.AnimalShop.GetDefaultIconPath(animal.Category);
                        }
                    }
                    else
                    {
                        animal.AnimalShop = null;
                    }

                    config.FarmAnimals.Add(animal);
                }

                return true;
            }

            config = new ModConfig();

            return false;
        }
    }
}