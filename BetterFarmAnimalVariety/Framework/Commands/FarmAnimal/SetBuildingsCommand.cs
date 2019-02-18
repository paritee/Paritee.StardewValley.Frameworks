using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Commands.FarmAnimal
{
    class SetBuildingsCommand : Command
    {
        public SetBuildingsCommand(IModHelper helper, IMonitor monitor, ModConfig config)
            : base("bfav_fa_setbuildings", "Set the category's buildings.\nUsage: bfav_fa_setbuildings <category> <buildings>\n- category: the unique animal category.\n- buildings: a comma separated string in quotes (ex \"Barn,Deluxe Coop\").", helper, monitor, config) { }

        /// <param name="command">The name of the command invoked.</param>
        /// <param name="args">The arguments received by the command. Each word after the command name is a separate argument.</param>
        public override void Callback(string command, string[] args)
        {
            try
            {
                this.AssertGameNotLoaded();
                this.AssertNoSpaces(args.Length, 2);
                this.AssertRequiredArgumentOrder(args.Length, 1, "category");

                string category = args[0].Trim();

                this.AssertFarmAnimalCategoryExists(category);
                this.AssertRequiredArgumentOrder(args.Length, 2, "buildings");

                List<string> buildings = args[1].Split(',').Select(i => i.Trim()).ToList();

                this.AssertBuildingsExist(buildings);

                Framework.Config.FarmAnimal animal = this.Config.GetCategory(category);

                animal.Buildings = buildings.ToArray();

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
