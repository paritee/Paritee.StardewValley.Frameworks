using BetterFarmAnimalVariety.Models;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using PariteeAnimalHouse = Paritee.StardewValleyAPI.Buildings.AnimalHouses.AnimalHouse;

namespace BetterFarmAnimalVariety.Framework.Commands.FarmAnimal
{
    class AddCategoryCommand : Command
    {
        public AddCategoryCommand(IModHelper helper, IMonitor monitor, ModConfig config)
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

                string building = args.Length < 3 ? Framework.Helpers.Constants.Barn : args[2].Trim();
                List<string> buildings = new List<string>();

                if (building.ToLower().Equals(Framework.Helpers.Constants.Coop.ToLower()))
                {
                    foreach (PariteeAnimalHouse.Size size in Enum.GetValues(typeof(Paritee.StardewValleyAPI.Buildings.AnimalHouses.Coop.Size)))
                    {
                        buildings.Add(PariteeAnimalHouse.FormatBuilding(Framework.Helpers.Constants.Coop, size));
                    }
                }
                else if (building.ToLower().Equals(Framework.Helpers.Constants.Barn.ToLower()))
                {
                    foreach (PariteeAnimalHouse.Size size in Enum.GetValues(typeof(Paritee.StardewValleyAPI.Buildings.AnimalHouses.Barn.Size)))
                    {
                        buildings.Add(PariteeAnimalHouse.FormatBuilding(Framework.Helpers.Constants.Barn, size));
                    }
                }
                else
                {
                    buildings = building.Split(',').Select(i => i.Trim()).ToList();
                    
                    this.AssertBuildingsExist(buildings);
                }

                string animalShop = (args.Length < 4 ? Command.False : args[3].Trim()).ToLower();

                this.AssertValidBoolean(animalShop, "animalShop", out bool result);

                ConfigFarmAnimalAnimalShop configFarmAnimalAnimalShop;

                configFarmAnimalAnimalShop = Framework.Helpers.Commands.GetAnimalShopConfig(category, animalShop);

                this.Config.FarmAnimals.Add(category, new ConfigFarmAnimal());

                this.Config.FarmAnimals[category].Category = category;
                this.Config.FarmAnimals[category].Types = types.Distinct().ToArray();
                this.Config.FarmAnimals[category].Buildings = buildings.ToArray();
                this.Config.FarmAnimals[category].AnimalShop = configFarmAnimalAnimalShop;

                this.Helper.WriteConfig(this.Config);

                string output = Helpers.Commands.DescribeFarmAnimalCategory(new KeyValuePair<string, ConfigFarmAnimal>(category, this.Config.FarmAnimals[category]));

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
            if (this.Config.FarmAnimals.ContainsKey(category))
            {
                throw new Exception($"{category} already exists in config.json");
            }
        }
    }
}
