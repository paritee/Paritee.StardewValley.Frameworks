using BetterFarmAnimalVariety.Models;
using StardewModdingAPI;
using System.Collections.Generic;

namespace BetterFarmAnimalVariety.Commands
{
    class BaseCommand
    {

        protected List<Command> Commands = new List<Command>();

        protected readonly ModConfig Config;
        protected readonly IModHelper Helper;
        protected readonly IMonitor Monitor;

        public BaseCommand(ModConfig config, IModHelper helper, IMonitor monitor)
        {
            this.Config = config;
            this.Helper = helper;
            this.Monitor = monitor;
        }

        public void SetUp()
        {
            foreach (Command command in this.Commands)
            {
                this.Helper.ConsoleCommands.Add(command.Name, command.Documentation, command.Callback);
            }
        }

        protected void HandleUpdatedConfig()
        {
            this.Helper.WriteConfig<ModConfig>(this.Config);
        }
    }
}
