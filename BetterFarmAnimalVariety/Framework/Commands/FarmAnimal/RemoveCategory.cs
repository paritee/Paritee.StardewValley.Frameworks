using StardewModdingAPI;
using StardewValley;
using System;

namespace BetterFarmAnimalVariety.Framework.Commands.FarmAnimal
{
    class RemoveCategory : Command
    {
        public RemoveCategory(IModHelper helper, IMonitor monitor, ModConfig config)
            : base("bfav_fa_removecategory", "Remove an existing category.\nUsage: bfav_fa_removecategory <category>\n- category: the unique animal category.", helper, monitor, config) { }

        /// <param name="command">The name of the command invoked.</param>
        /// <param name="args">The arguments received by the command. Each word after the command name is a separate argument.</param>
        public override void Callback(string command, string[] args)
        {
            try
            {
                Helpers.Assert.GameNotLoaded();
                Helpers.Assert.ArgumentInRange(args.Length, 1);
                Helpers.Assert.RequiredArgumentOrder(args.Length, 1, "category");

                string category = args[0];

                Helpers.Assert.FarmAnimalCategoryExists(category);

                this.Config.RemoveCategory(category);

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
