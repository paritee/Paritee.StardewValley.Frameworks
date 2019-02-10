using BetterFarmAnimalVariety.Models;
using StardewModdingAPI;
using System.Collections.Generic;

namespace BetterFarmAnimalVariety.Framework.Commands.FarmAnimal
{
    class ListCommand : Command
    {
        public ListCommand(IModHelper helper, IMonitor monitor, ModConfig config) 
            : base("bfav_fa_list", "List the farm animal categories and types.\nUsage: bfav_fa_list", helper, monitor, config) { }

        /// <param name="command">The name of the command invoked.</param>
        /// <param name="args">The arguments received by the command. Each word after the command name is a separate argument.</param>
        public override void Callback(string command, string[] args)
        {
            string output = "Listing BFAV farm animals\n";

            foreach (KeyValuePair<string, ConfigFarmAnimal> entry in this.Config.FarmAnimals)
            {
                output += Helpers.Commands.DescribeFarmAnimalCategory(entry);
            }

            this.Monitor.Log(output, LogLevel.Info);
        }
    }
}
