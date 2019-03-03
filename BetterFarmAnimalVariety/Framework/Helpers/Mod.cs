using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Helpers
{
    class Mod
    {
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

        public static void WriteCache<T>(string cacheFilePath, T data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            string path = Path.Combine(Constants.Mod.CacheFullPath, cacheFilePath);

            System.IO.FileInfo file = new System.IO.FileInfo(path);
            file.Directory.Create(); // If the directory already exists, this method does nothing.

            File.WriteAllText(path, json);
        }

        public static T ReadCache<T>(string cacheFilePath) where T: new()
        {
            string path = Path.Combine(Constants.Mod.CacheFullPath, cacheFilePath);

            if (!File.Exists(path))
            {
                return new T();
            }

            string json = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<T>(json);
        }

        public static Texture2D LoadTexture(string filePath)
        {
            return PariteeCore.Api.Mod.LoadTexture(Path.Combine(PariteeCore.Constants.Mod.Path, filePath));
        }

        public static string GetShortAssetPath(string filePath)
        {
            return Path.Combine(Constants.Mod.AssetsDirectory, filePath);
        }

        public static string GetFullAssetPath(string filePath)
        {
            return Path.Combine(PariteeCore.Constants.Mod.Path, Helpers.Mod.GetShortAssetPath(filePath));
        }

        public static bool TryGetFullAssetPath(string filePath, out string path)
        {
            path = Helpers.Mod.GetFullAssetPath(filePath);

            return !File.Exists(path);
        }

        public static bool MigrateDeprecatedConfigToCurrentFormat<T>(T deprecatedConfig, string targetFormat, out ModConfig config)
        {
            if (deprecatedConfig is Config.V2.ModConfig)
            {
                Config.V2.ModConfig sourceConfig = (Config.V2.ModConfig)Convert.ChangeType(deprecatedConfig, typeof(Config.V2.ModConfig));

                config = new BetterFarmAnimalVariety.ModConfig
                {
                    Format = targetFormat,
                    IsEnabled = sourceConfig.IsEnabled
                };

                string voidChicken = PariteeCore.Constants.VanillaFarmAnimalType.VoidChicken.ToString();

                foreach (KeyValuePair<string, Config.V2.ConfigFarmAnimal> oldFarmAnimals in sourceConfig.FarmAnimals)
                {
                    Config.FarmAnimalStock animalShop;

                    if (oldFarmAnimals.Value.CanBePurchased())
                    {
                        Int32.TryParse(oldFarmAnimals.Value.AnimalShop.Price, out int price);

                        string[] exclude = sourceConfig.IsChickenCategory(oldFarmAnimals.Key)
                            && oldFarmAnimals.Value.Types.Contains(voidChicken)
                            && sourceConfig.AreVoidFarmAnimalsInShopAlways()
                            ? new string[0]
                            : new string[] { voidChicken };

                        animalShop = new Config.FarmAnimalStock
                        {
                            Name = oldFarmAnimals.Value.AnimalShop.Name,
                            Description = oldFarmAnimals.Value.AnimalShop.Description,
                            Icon = oldFarmAnimals.Value.AnimalShop.Icon,
                            Price = Math.Abs(price),
                            Exclude = exclude
                        };

                        // Check to make sure the icon exists...
                        string pathToIcon = Path.Combine(PariteeCore.Constants.Mod.Path, animalShop.Icon);

                        // ... otherwise default it to the new icon assuming 
                        // the user copied them over
                        if (!File.Exists(pathToIcon))
                        {
                            animalShop.Icon = animalShop.GetDefaultIconPath(oldFarmAnimals.Key);
                        }
                    }
                    else
                    {
                        animalShop = null;
                    }

                    Config.FarmAnimalCategory animal = new Config.FarmAnimalCategory
                    {
                        Category = oldFarmAnimals.Key,
                        Types = oldFarmAnimals.Value.Types,
                        Buildings = oldFarmAnimals.Value.Buildings,
                        AnimalShop = animalShop
                    };

                    config.AddCategory(animal);
                }

                return true;
            }

            config = new ModConfig();

            return false;
        }
    }
}