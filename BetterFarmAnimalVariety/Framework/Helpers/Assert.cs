using System;
using System.Collections.Generic;
using System.IO;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Helpers
{
    class Assert
    {
        /// <param name="version">string</param>
        /// <param name="target">string</param>
        /// <exception cref="NotSupportedException"></exception>
        public static void VersionIsSupported(string version, string target)
        {
            if (version == null || !version.Equals(target))
            {
                throw new NotSupportedException($"Version {version} is not supported.");
            }
        }

        /// <exception cref="ApplicationException"></exception>
        public static void GameLoaded()
        {
            if (!PariteeCore.Api.Game.IsSaveLoaded())
            {
                throw new ApplicationException($"Save has not been loaded.");
            }
        }

        /// <exception cref="ApplicationException"></exception>
        public static void GameNotLoaded()
        {
            if (PariteeCore.Api.Game.IsSaveLoaded())
            {
                throw new ApplicationException($"Save has been loaded.");
            }
        }

        /// <param name="total">int</param>
        /// <param name="expected">int</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ArgumentInRange(int total, int expected)
        {
            if (total > expected)
            {
                throw new ArgumentOutOfRangeException($"Use quotation marks (\") around your text if you are using spaces");
            }
        }

        /// <param name="length">int</param>
        /// <param name="expected">int</param>
        /// <exception cref="ArgumentException"></exception>
        public static void RequiredArgumentOrder(int length, int expected, string argument)
        {
            if (length < expected)
            {
                throw new ArgumentException($"\"{argument}\" is required");
            }
        }

        /// <param name="types">List<string></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void FarmAnimalTypesExist(List<string> types)
        {
            Dictionary<string, string> contentData = PariteeCore.Api.Content.LoadData<string, string>(PariteeCore.Constants.Content.DataFarmAnimalsContentPath);

            // Check if these new types are valid
            foreach (string key in types)
            {
                if (!contentData.ContainsKey(key))
                {
                    throw new NotImplementedException($"\"{key}\" does not exist in Data/FarmAnimals");
                }
            }
        }

        /// <param name="buildings">List<string></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void BuildingsExist(List<string> buildings)
        {
            Dictionary<string, string> blueprintsData = PariteeCore.Api.Content.Load<Dictionary<string, string>>(PariteeCore.Constants.Content.DataBlueprintsContentPath);

            // Check if these new types are valid
            foreach (string key in buildings)
            {
                if (!blueprintsData.ContainsKey(key))
                {
                    throw new NotImplementedException($"\"{key}\" does not exist in Data/Blueprints");
                }
            }
        }

        /// <param name="strBool">string</param>
        /// <param name="argument">string</param>
        /// <param name="result">out bool</param>
        /// <exception cref="FormatException"></exception>
        public static void ValidBoolean(string strBool, string argument, out bool result)
        {
            if (!bool.TryParse(strBool, out result))
            {
                throw new FormatException($"{argument} must be { true.ToString().ToLower() } or { false.ToString().ToLower() }");
            }
        }

        /// <param name="category">string</param>
        /// <exception cref="NotImplementedException"></exception>
        public static void FarmAnimalCategoryExists(string category)
        {
            // Check the config
            ModConfig config = Helpers.Mod.ReadConfig<ModConfig>();

            if (!config.CategoryExists(category))
            {
                throw new NotImplementedException($"{category} is not a category in config.json");
            }
        }

        /// <param name="types">string[]</param>
        /// <exception cref="ArgumentException"></exception>
        public static void AtLeastOneTypeRequired(string[] types)
        {
            if (types.Length < 1)
            {
                throw new ArgumentException($"At least one type is required");
            }
        }

        /// <param name="animalShop">string</param>
        /// <param name="canBePurchasedNow">bool</param>
        /// <param name="result">out bool</param>
        /// <exception cref="ArgumentException"></exception>
        public static void ChangeInPurchaseState(string animalShop, bool canBePurchasedNow, out bool result)
        {
            Assert.ValidBoolean(animalShop, "animalShop", out result);

            if (result.Equals(true) && canBePurchasedNow)
            {
                throw new ArgumentException($"Already available in the animal shop");
            }
            else if (result.Equals(false) && !canBePurchasedNow)
            {
                throw new ArgumentException($"Already not available in the animal shop");
            }
        }

        /// <param name="category">string</param>
        /// <exception cref="NotImplementedException"></exception>
        public static void FarmAnimalCanBePurchased(string category)
        {
            // Check the config
            ModConfig config = Helpers.Mod.ReadConfig<ModConfig>();

            if (!config.CanBePurchased(category))
            {
                throw new NotImplementedException($"\"{category}\" is not available in the animal shop");
            }
        }

        /// <param name="amount">string</param>
        /// <exception cref="FormatException"></exception>
        public static void ValidMoneyAmount(string amount)
        {
            if (!int.TryParse(amount, out int n) || n < 0)
            {
                throw new FormatException($"Amount must be a positive number");
            }
        }

        /// <param name="fileName">string</param>
        /// <exception cref="FileNotFoundException"></exception>
        public static void ValidAnimalShopIcon(string fileName)
        {
            string filePath = Path.Combine(Constants.Mod.AnimalShopIconDirectory, Path.GetFileName(fileName));

            if (Helpers.Mod.TryGetFullAssetPath(filePath, out string fullPathToIcon))
            {
                throw new FileNotFoundException($"{fullPathToIcon} does not exist");
            }

            Helpers.Assert.ValidFileExtension(fileName, Constants.Mod.AnimalShopIconExtension);
        }

        /// <param name="fileName">string</param>
        /// <param name="extension">string</param>
        /// <exception cref="FormatException"></exception>
        public static void ValidFileExtension(string fileName, string extension)
        {
            if (!Path.GetExtension(fileName).ToLower().Equals(extension))
            {
                throw new FormatException($"{fileName} must be a {extension}");
            }
        }

        /// <param name="category">string</param>
        /// <exception cref="ArgumentException"></exception>
        public static void UniqueFarmAnimalCategory(string category)
        {
            // Check the config
            ModConfig config = Helpers.Mod.ReadConfig<ModConfig>();

            if (config.CategoryExists(category))
            {
                throw new ArgumentException($"\"{category}\" already exists in config.json");
            }
        }

        /// <param name="moddedLocation">Decorators.Location</param>
        /// <exception cref="ApplicationException"></exception>
        public static void Outdoors(Decorators.Location moddedLocation)
        {
            if (!moddedLocation.IsOutdoors())
            {
                throw new ApplicationException($"Location is not outdoors.");
            }
        }

        /// <exception cref="ApplicationException"></exception>
        public static void NotRaining()
        {
            if (PariteeCore.Api.Weather.IsRaining())
            {
                throw new ApplicationException($"It is raining.");
            }
        }

        /// <exception cref="ApplicationException"></exception>
        public static void NotWinter()
        {
            if (PariteeCore.Api.Season.IsWinter())
            {
                throw new ApplicationException($"It is winter.");
            }
        }

        /// <param name="produceIndex">int</param>
        /// <exception cref="KeyNotFoundException"></exception>
        public static void ProduceIsAnItem(int produceIndex)
        {
            if (!PariteeCore.Api.FarmAnimal.IsProduceAnItem(produceIndex))
            {
                throw new KeyNotFoundException($"\"{produceIndex}\" is not produce.");
            }
        }

        /// <param name="moddedAnimal">Decorators.FarmAnimal</param>
        /// <exception cref="ApplicationException"></exception>
        public static void CanFindProduce(Decorators.FarmAnimal moddedAnimal)
        {
            if (!moddedAnimal.CanFindProduce())
            {
                throw new ApplicationException($"{moddedAnimal.GetType()} cannot find produce.");
            }
        }
    }
}
