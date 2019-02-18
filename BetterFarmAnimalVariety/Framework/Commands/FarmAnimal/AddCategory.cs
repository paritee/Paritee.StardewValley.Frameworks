using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Commands.FarmAnimal
{
    class AddCategory : Command
    {
        public AddCategory(IModHelper helper, IMonitor monitor, ModConfig config)
            : base("bfav_fa_addcategory", $"Add a unique category.\nUsage: bfav_fa_addcategory <category> <types> <buildings> <animalshop>\n- category: the unique animal category.\n- types: a comma separated string in quotes (ex \"White Cow,Brown Cow\").\n- buildings: a comma separated string in quotes (ex \"Barn,Deluxe Coop\").\n- animalshop: { Command.True} or { Command.False }.", helper, monitor, config) { }

        /// <param name="command">The name of the command invoked.</param>
        /// <param name="args">The arguments received by the command. Each word after the command name is a separate argument.</param>
        public override void Callback(string command, string[] args)
        {
            try
            {
                this.AssertGameNotLoaded();
                this.AssertNoSpaces(args.Length, 1);
                this.AssertRequiredArgumentOrder(args.Length, 1, "category");

                string category = args[0].Trim();

                this.AssertUniqueFarmAnimalCategory(category);
                this.AssertRequiredArgumentOrder(args.Length, 2, "type");

                List<string> types = args[1].Split(',').Select(i => i.Trim()).ToList();
                
                this.AssertFarmAnimalTypesExist(types);

                string building = args.Length < 3 ? Constants.AnimalHouse.Barn : args[2].Trim();
                List<string> buildings = new List<string>();

                if (building.ToLower().Equals(Constants.AnimalHouse.Coop.ToLower()))
                {
                    foreach (Constants.AnimalHouse.Size size in Enum.GetValues(typeof(Constants.AnimalHouse.Size)))
                    {
                        buildings.Add(Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Coop, size));
                    }
                }
                else if (building.ToLower().Equals(Constants.AnimalHouse.Barn.ToLower()))
                {
                    foreach (Constants.AnimalHouse.Size size in Enum.GetValues(typeof(Constants.AnimalHouse.Size)))
                    {
                        buildings.Add(Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Barn, size));
                    }
                }
                else
                {
                    buildings = building.Split(',').Select(i => i.Trim()).ToList();
                    
                    this.AssertBuildingsExist(buildings);
                }

                string animalShop = (args.Length < 4 ? Command.False : args[3].Trim()).ToLower();

                this.AssertValidBoolean(animalShop, "animalShop", out bool result);

                Framework.Config.FarmAnimalStock farmAnimalStock = result
                    ? Framework.Config.FarmAnimalStock.CreateWithPlaceholders(category)
                    : null;

                Framework.Config.FarmAnimal animal = new Framework.Config.FarmAnimal
                {
                    Category = category,
                    Types = types.Distinct().ToArray(),
                    Buildings = buildings.ToArray(),
                    AnimalShop = farmAnimalStock
                };

                this.Config.FarmAnimals.Add(animal);

                this.Helper.WriteConfig(this.Config);

                string output = this.DescribeFarmAnimalCategory(animal);

                this.Monitor.Log(output, LogLevel.Info);
            }
            catch (Exception e)
            {
                this.Monitor.Log(e.Message, LogLevel.Error);

                return;
            }
        }

        private void AssertUniqueFarmAnimalCategory(string category)
        {
            if (this.Config.CategoryExists(category))
            {
                throw new Exception($"{category} already exists in config.json");
            }
        }
    }
}
