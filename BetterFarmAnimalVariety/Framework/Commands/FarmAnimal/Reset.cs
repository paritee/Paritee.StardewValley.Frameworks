using StardewModdingAPI;
using StardewValley;
using System;

namespace BetterFarmAnimalVariety.Framework.Commands.FarmAnimal
{
    class Reset : Command
    {
        public Reset(IModHelper helper, IMonitor monitor, ModConfig config)
            : base("bfav_fa_reset", "Reset the farm animals in config.json to vanilla default.\nUsage: bfav_fa_reset", helper, monitor, config) { }

        /// <param name="command">The name of the command invoked.</param>
        /// <param name="args">The arguments received by the command. Each word after the command name is a separate argument.</param>
        public override void Callback(string command, string[] args)
        {
            try
            {
                Helpers.Assert.GameNotLoaded();

                ModConfig config = new ModConfig();

                this.Config.FarmAnimals = config.FarmAnimals;

                this.Helper.WriteConfig(this.Config);

                this.Helper.ConsoleCommands.Trigger("bfav_fa_list", new string[] { });
            }
            catch (Exception e)
            {
                this.Monitor.Log(e.Message, LogLevel.Error);
            }
        }
    }
}
