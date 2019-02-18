using StardewModdingAPI;
using System;

namespace BetterFarmAnimalVariety.Framework.Commands.Config
{
    class RandomizeFromCategory : Command
    {
        public RandomizeFromCategory(IModHelper helper, IMonitor monitor, ModConfig config)
            : base("bfav_conf_randfromcategory", $"Set newbown and hatchling randomization settings.\nUsage: bfav_conf_randfromcategory <newborn> <hatchling> <ignoreparentproduce>\n- newborn: true or false\n- hatchling: true or false\n- ignoreparentproduce: true or false", helper, monitor, config) { }

        /// <param name="command">The name of the command invoked.</param>
        /// <param name="args">The arguments received by the command. Each word after the command name is a separate argument.</param>
        public override void Callback(string command, string[] args)
        {
            try
            {
                this.AssertGameNotLoaded();
                this.AssertRequiredArgumentOrder(args.Length, 3, "newbown, hatchling, ignoreparentproduce");
                this.AssertValidBoolean(args[0], "newbown", out bool newbown);
                this.AssertValidBoolean(args[1], "hatchling", out bool hatchling);
                this.AssertValidBoolean(args[2], "ignoreparentproduce", out bool ignoreParentProduce);

                this.Config.RandomizeNewbornFromCategory = newbown;
                this.Config.RandomizeHatchlingFromCategory = hatchling;
                this.Config.IgnoreParentProduceCheck = ignoreParentProduce;

                this.Helper.WriteConfig(this.Config);

                this.Monitor.Log($"Config successfully updated.", LogLevel.Info);
            }
            catch (Exception e)
            {
                this.Monitor.Log(e.Message, LogLevel.Error);

                return;
            }
        }
    }
}
