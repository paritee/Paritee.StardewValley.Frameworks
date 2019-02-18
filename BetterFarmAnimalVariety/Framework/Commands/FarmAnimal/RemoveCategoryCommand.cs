using StardewModdingAPI;
using StardewValley;
using System;

namespace BetterFarmAnimalVariety.Framework.Commands.FarmAnimal
{
    class RemoveCategoryCommand : Command
    {
        public RemoveCategoryCommand(IModHelper helper, IMonitor monitor, ModConfig config)
            : base("bfav_fa_removecategory", "Remove an existing category.\nUsage: bfav_fa_removecategory <category>\n- category: the unique animal category.", helper, monitor, config) { }

        /// <param name="command">The name of the command invoked.</param>
        /// <param name="args">The arguments received by the command. Each word after the command name is a separate argument.</param>
        public override void Callback(string command, string[] args)
        {
            try
            {
                this.AssertGameNotLoaded();
                this.AssertNoSpaces(args.Length, 1);
                this.AssertRequiredArgumentOrder(args.Length, 1, "category");

                string category = args[0];

                if (!this.Config.FarmAnimals.Exists(o => o.Category.Equals(category)))
                {
                    throw new Exception($"{category} is not a category in config.json");
                }

                this.Config.FarmAnimals.RemoveAll(o => o.Category.Equals(category));

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
