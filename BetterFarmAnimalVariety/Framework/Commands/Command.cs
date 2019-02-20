using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.IO;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Commands
{
    abstract class Command
    {
        public static string True { get { return true.ToString().ToLower(); } }
        public static string False { get { return false.ToString().ToLower(); } }

        public string Name;
        public string Description;

        protected readonly IModHelper Helper;
        protected readonly IMonitor Monitor;
        protected ModConfig Config;

        public Command (string name, string description, IModHelper helper, IMonitor monitor, ModConfig config)
        {
            this.Name = name;
            this.Description = description;

            this.Helper = helper;
            this.Monitor = monitor;
            this.Config = config;
        }

        public abstract void Callback(string command, string[] args);

        protected string DescribeFarmAnimalCategory(Framework.Config.FarmAnimal animal)
        {
            string output = "";

            output += $"{animal.Category}\n";
            output += $"- Types: {String.Join(",", animal.Types)}\n";
            output += $"- Buildings: {String.Join(",", animal.Buildings)}\n";
            
            if (animal.CanBePurchased())
            {
                output += $"- AnimalShop:\n";
                output += $"-- Name: {animal.AnimalShop.Name}\n";
                output += $"-- Description: {animal.AnimalShop.Description}\n";
                output += $"-- Price: {animal.AnimalShop.Price}\n";
                output += $"-- Icon: {animal.AnimalShop.Icon}\n";
            }
            else
            {
                output += $"- AnimalShop: null\n";
            }

            return output;
        }

        protected void AssertGameNotLoaded()
        {
            if (PariteeCore.Api.Game.IsSaveLoaded())
            {
                throw new Exception($"this cannot be done after loading a save");
            }
        }

        protected void AssertNoSpaces(int total, int expected)
        {
            if (total > expected)
            {
                throw new Exception($"use quotation marks (\") around your text if you are using spaces");
            }
        }

        protected void AssertRequiredArgumentOrder(int length, int expected, string argument)
        {
            if (length < expected)
            {
                throw new Exception($"{argument} is required");
            }
        }

        protected void AssertFarmAnimalTypesExist(List<string> types)
        {
            Dictionary<string, string> contentData = PariteeCore.Api.Content.LoadData<string, string>(PariteeCore.Constants.Content.DataFarmAnimalsContentPath);

            // Check if these new types are valid
            foreach (string key in types)
            {
                if (!contentData.ContainsKey(key))
                {
                    throw new Exception($"{key} does not exist in Data/FarmAnimals");
                }
            }
        }

        protected void AssertBuildingsExist(List<string> buildings)
        {
            Dictionary<string, string> blueprintsData = PariteeCore.Api.Content.Load<Dictionary<string, string>>(PariteeCore.Constants.Content.DataBlueprintsContentPath);

            // Check if these new types are valid
            foreach (string key in buildings)
            {
                if (!blueprintsData.ContainsKey(key))
                {
                    throw new Exception($"{key} does not exist in Data/Blueprints");
                }
            }
        }

        protected void AssertValidBoolean(string strBool, string argument, out bool result)
        {
            if (!bool.TryParse(strBool, out result))
            {
                throw new Exception($"{argument} must be { Command.True } or { Command.False }");
            }
        }

        protected void AssertFarmAnimalCategoryExists(string category)
        {
            if (!this.Config.CategoryExists(category))
            {
                throw new Exception($"{category} is not a category in config.json");
            }
        }

        protected void AssertFarmAnimalCategoryTypesNotEmpty(string[] types)
        {
            if (types.Length < 1)
            {
                throw new Exception($"categories must contain at least one type");
            }
        }

        protected void AssertAnimalShopChange(string animalShop, bool canBePurchasedNow)
        {
            if (animalShop.Equals(Command.True) && canBePurchasedNow)
            {
                throw new Exception($"already available in the animal shop");
            }
            else if (animalShop.Equals(Command.False) && !canBePurchasedNow)
            {
                throw new Exception($"already not available in the animal shop");
            }
        }

        protected void AssertFarmAnimalCanBePurchased(string category)
        {
            if (!this.Config.FarmAnimals.Exists(o => o.Category.Equals(category) && o.CanBePurchased()))
            {
                throw new Exception($"{category} is not available in the animal shop");
            }
        }

        protected void AssertValidMoneyAmount(string amount)
        {
            if (!int.TryParse(amount, out int n))
            {
                throw new Exception($"price must be a positive number");
            }

            if (n < 0)
            {
                throw new Exception($"price must be a positive number");
            }
        }

        protected void AssertValidIcon(string filename)
        {
            filename = Path.GetFileName(filename);

            string pathToIcon = Path.Combine(Constants.Mod.AssetsDirectory, filename);
            string fullPathToIcon = Path.Combine(this.Helper.DirectoryPath, pathToIcon);

            if (!File.Exists(fullPathToIcon))
            {
                throw new Exception($"{fullPathToIcon} does not exist");
            }

            if (!Path.GetExtension(fullPathToIcon).ToLower().Equals(".png"))
            {
                throw new Exception($"{filename} must be a .png");
            }
        }
    }
}
