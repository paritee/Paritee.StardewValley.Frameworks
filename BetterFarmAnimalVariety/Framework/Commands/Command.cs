using BetterFarmAnimalVariety.Framework.Commands.FarmAnimal;
using Paritee.StardewValleyAPI.FarmAnimals.Variations;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        protected void AssertGameNotLoaded()
        {
            if (Game1.hasLoadedGame)
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
            Dictionary<string, string> contentData = Api.Content.Load<Dictionary<string, string>>(Framework.Helpers.Constants.DataFarmAnimalsContentDirectory);

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
            Dictionary<string, string> blueprintsData = Api.Content.Load<Dictionary<string, string>>(Framework.Helpers.Constants.DataBlueprintsContentDirectory);

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
            if (!this.Config.FarmAnimals.ContainsKey(category))
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
            if (!this.Config.FarmAnimals[category].CanBePurchased())
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

            string pathToIcon = Path.Combine(Properties.Settings.Default.AssetsDirectory, filename);
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

        protected void AssertValidVoidInShop(string inShop, out VoidConfig.InShop flag)
        {
            if (!Enum.TryParse(inShop, true, out flag))
            {
                throw new Exception($"{inShop} is not a valid flag");
            }
        }
    }
}
