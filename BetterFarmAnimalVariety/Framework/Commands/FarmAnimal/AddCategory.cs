using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Commands.FarmAnimal
{
    class AddCategory : Command
    {
        public AddCategory(IModHelper helper, IMonitor monitor, ModConfig config)
            : base("bfav_fa_addcategory", $"Add a unique category.\nUsage: bfav_fa_addcategory <category> <types> <buildings> <animalshop>\n- category: the unique animal category.\n- types: a comma separated list in quotes (ex \"White Cow,Brown Cow\").\n- buildings: a comma separated list in quotes (ex \"Barn,Deluxe Coop\").\n- animalshop: {true.ToString().ToLower()} or {false.ToString().ToLower()}.", helper, monitor, config) { }

        /// <param name="command">The name of the command invoked.</param>
        /// <param name="args">The arguments received by the command. Each word after the command name is a separate argument.</param>
        public override void Callback(string command, string[] args)
        {
            try
            {
                Helpers.Assert.GameNotLoaded();
                Helpers.Assert.ArgumentInRange(args.Length, 1);
                Helpers.Assert.RequiredArgumentOrder(args.Length, 1, "category");

                string category = args[0].Trim();

                Helpers.Assert.UniqueFarmAnimalCategory(category);
                Helpers.Assert.RequiredArgumentOrder(args.Length, 2, "type");

                List<string> types = args[1].Split(',').Select(i => i.Trim()).ToList();

                Helpers.Assert.FarmAnimalTypesExist(types);

                string building = args.Length < 3 ? PariteeCore.Constants.AnimalHouse.Barn : args[2].Trim();
                List<string> buildings = new List<string>();

                if (building.ToLower().Equals(PariteeCore.Constants.AnimalHouse.Coop.ToLower()))
                {
                    foreach (PariteeCore.Constants.AnimalHouse.Size size in Enum.GetValues(typeof(PariteeCore.Constants.AnimalHouse.Size)))
                    {
                        buildings.Add(PariteeCore.Api.AnimalHouse.FormatSize(PariteeCore.Constants.AnimalHouse.Coop, size));
                    }
                }
                else if (building.ToLower().Equals(PariteeCore.Constants.AnimalHouse.Barn.ToLower()))
                {
                    foreach (PariteeCore.Constants.AnimalHouse.Size size in Enum.GetValues(typeof(PariteeCore.Constants.AnimalHouse.Size)))
                    {
                        buildings.Add(PariteeCore.Api.AnimalHouse.FormatSize(PariteeCore.Constants.AnimalHouse.Barn, size));
                    }
                }
                else
                {
                    buildings = building.Split(',').Select(i => i.Trim()).ToList();
                    
                    Helpers.Assert.BuildingsExist(buildings);
                }

                string animalShop = (args.Length < 4 ? Command.False : args[3].Trim()).ToLower();

                Helpers.Assert.ValidBoolean(animalShop, "animalShop", out bool result);

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
    }
}
