using BetterFarmAnimalVariety.Framework.Events;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            if (!(version == target))
            {
                throw new NotSupportedException($"Version {version} is not supported");
            }
        }

        /// <param name="api">object</param>
        /// <exception cref="Exceptions.ApiNotFoundException"></exception>
        public static void ApiExists<TInterface>(IModHelper helper, string key, out TInterface api) where TInterface: class
        {
            if (!Helpers.Mod.TryGetApi(helper, key, out api))
            {
                throw new Exceptions.ApiNotFoundException(key);
            }
        }

        /// <exception cref="Exceptions.SaveNotLoadedException"></exception>
        public static void SaveLoaded()
        {
            if (!PariteeCore.Api.Game.IsSaveLoaded())
            {
                throw new Exceptions.SaveNotLoadedException();
            }
        }

        /// <exception cref="ApplicationException"></exception>
        public static void SaveNotLoaded()
        {
            if (PariteeCore.Api.Game.IsSaveLoaded())
            {
                throw new ApplicationException($"Save has been loaded");
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
        /// <param name="argument">string</param>
        /// <exception cref="ArgumentException"></exception>
        public static void RequiredArgumentOrder(int length, int expected, string argument)
        {
            if (length < expected)
            {
                throw new ArgumentException($"\"{argument}\" is required");
            }
        }

        /// <param name="argument">string</param>
        /// <param name="str">string</param>
        /// <param name="minLength">int</param>
        /// <param name="maxLength">int</param>
        /// <exception cref="ArgumentException"></exception>
        public static void ValidStringLength(string argument, string str, int minLength, int maxLength = -1)
        {
            if (str.Length < minLength)
            {
                throw new ArgumentException($"\"{argument}\" must be at least {minLength} characters");
            }

            if (maxLength >= 0 && str.Length > maxLength)
            {
                throw new ArgumentException($"\"{argument}\" must be at most {minLength} characters");
            }
        }

        /// <param name="type">string</param>
        /// <exception cref="NotImplementedException"></exception>
        public static void FarmAnimalTypeExists(string type)
        {
            Helpers.Assert.FarmAnimalTypesExist(new List<string>() { type });
        }

        /// <param name="types">List<string></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void FarmAnimalTypesExist(List<string> types)
        {
            Dictionary<string, string> contentData = PariteeCore.Api.Content.LoadData<string, string>(PariteeCore.Constants.Content.DataFarmAnimalsContentPath);

            // Check if these new types are valid
            foreach (string type in types)
            {
                if (!contentData.ContainsKey(type))
                {
                    throw new NotImplementedException($"\"{type}\" does not exist in Data/FarmAnimals");
                }
            }
        }

        /// <param name="types">List<string></param>
        /// <exception cref="NotSupportedException"></exception>
        public static void FarmAnimalTypeIsNotRestricted(string type)
        {
            if (Constants.Mod.RestrictedFarmAnimalTypes.Select(o => o.ToString().ToLower()).Contains(type.ToLower()))
            {
                throw new NotSupportedException($"\"{type}\" is a restricted type and cannot be used");
            }
        }

        /// <param name="types">List<string></param>
        /// <exception cref="NotSupportedException"></exception>
        public static void FarmAnimalTypesAreNotRestricted(List<string> types)
        {
            // Check if these new types are valid
            foreach (string type in types)
            {
                Helpers.Assert.FarmAnimalTypeIsNotRestricted(type);
            }
        }

        /// <param name="strIndex">string</param>
        /// <exception cref="NotImplementedException"></exception>
        public static void ValidFarmAnimalProduce(IModHelper helper, string strIndex, out int produceIndex)
        {
            if (!Int32.TryParse(strIndex, out produceIndex))
            {
                // Try to find an item by name
                if (IntegrateWithJsonAssets.TryParseFarmAnimalProduceName(strIndex, out produceIndex))
                {
                    // Found an item by name
                    return;
                }

                // No item found by name and not a valid numeric index
                throw new NotImplementedException($"\"{strIndex}\" is not a known Object");
            }

            // "no produce" (-1) should not trigger the assert
            if (!PariteeCore.Api.FarmAnimal.IsProduceAnItem(produceIndex))
            {
                return;
            }

            // Check to see if this object actually exists
            if (!PariteeCore.Api.Object.ObjectExists(produceIndex))
            {
                throw new NotImplementedException($"\"{strIndex}\" is not a known Object");
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
            if (!Helpers.FarmAnimals.CategoryExists(category))
            {
                throw new NotImplementedException($"{category} category does not exist");
            }
        }

        /// <param name="types">List<string></param>
        /// <exception cref="ArgumentException"></exception>
        public static void AtLeastOneTypeRequired(List<string> types)
        {
            if (types.Count < 1)
            {
                throw new ArgumentException($"At least one type is required");
            }
        }

        /// <param name="buildings">List<string></param>
        /// <exception cref="ArgumentException"></exception>
        public static void AtLeastOneBuildingRequired(List<string> buildings)
        {
            if (buildings.Count < 1)
            {
                throw new ArgumentException($"At least one building is required");
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
            if (!Helpers.FarmAnimals.CanBePurchased(category))
            {
                throw new NotImplementedException($"\"{category}\" is not available in the animal shop");
            }
        }

        /// <param name="amount">string</param>
        /// <exception cref="FormatException"></exception>
        public static void ValidMoneyAmount(string amount)
        {
            if (!int.TryParse(amount, out int n))
            {
                throw new FormatException($"Amount must be a positive number");
            }

            Helpers.Assert.ValidMoneyAmount(n);
        }

        /// <param name="amount">int</param>
        /// <exception cref="FormatException"></exception>
        public static void ValidMoneyAmount(int amount)
        {
            if (amount < 0)
            {
                throw new FormatException($"Amount must be a positive number");
            }
        }

        /// <param name="fileName">string</param>
        /// <exception cref="FileNotFoundException"></exception>
        public static void FileExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"{filePath} does not exist");
            }
        }

        /// <param name="fileName">string</param>
        /// <exception cref="FileNotFoundException"></exception>
        public static void ValidAnimalShopIcon(string filePath)
        {
            Helpers.Assert.FileExists(filePath);
            Helpers.Assert.ValidFileExtension(filePath, Constants.Mod.AnimalShopIconExtension);
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

        /// <param name="List<T>">T</param>
        /// <exception cref="FormatException"></exception>
        public static void UniqueValues<T>(List<T> values)
        {
            HashSet<T> uniqueValues = new HashSet<T>();

            foreach (T value in values)
            {
                if (!uniqueValues.Add(value))
                {
                    throw new FormatException($"Multiple instances of \"{value}\" exists in the same set");
                }
            }
        }

        /// <param name="category">string</param>
        /// <exception cref="ArgumentException"></exception>
        public static void UniqueFarmAnimalCategory(string category)
        {
            // Load the cache
            Cache.FarmAnimals cache = Helpers.FarmAnimals.ReadCache();

            if (cache.CategoryExists(category))
            {
                throw new ArgumentException($"\"{category}\" category already exists");
            }
        }

        /// <param name="moddedLocation">Decorators.Location</param>
        /// <exception cref="ApplicationException"></exception>
        public static void Outdoors(Decorators.Location moddedLocation)
        {
            if (!moddedLocation.IsOutdoors())
            {
                throw new ApplicationException($"Location is not outdoors");
            }
        }

        /// <exception cref="ApplicationException"></exception>
        public static void NotRaining()
        {
            if (PariteeCore.Api.Weather.IsRaining())
            {
                throw new ApplicationException($"It is raining");
            }
        }

        /// <exception cref="ApplicationException"></exception>
        public static void NotWinter()
        {
            if (PariteeCore.Api.Season.IsWinter())
            {
                throw new ApplicationException($"It is winter");
            }
        }

        /// <param name="produceIndex">int</param>
        /// <exception cref="KeyNotFoundException"></exception>
        public static void ProduceIsAnItem(int produceIndex)
        {
            if (!PariteeCore.Api.FarmAnimal.IsProduceAnItem(produceIndex))
            {
                throw new KeyNotFoundException($"\"{produceIndex}\" is not produce");
            }
        }

        /// <param name="moddedAnimal">Decorators.FarmAnimal</param>
        /// <exception cref="ApplicationException"></exception>
        public static void CanFindProduce(Decorators.FarmAnimal moddedAnimal)
        {
            if (!moddedAnimal.CanFindProduce())
            {
                throw new ApplicationException($"{moddedAnimal.GetType()} cannot find produce");
            }
        }
    }
}
