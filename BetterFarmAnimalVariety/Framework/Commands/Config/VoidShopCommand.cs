using Paritee.StardewValleyAPI.FarmAnimals.Variations;
using StardewModdingAPI;
using System;

namespace BetterFarmAnimalVariety.Framework.Commands.Config
{
    class VoidShopCommand : Command
    {
        public VoidShopCommand(IModHelper helper, IMonitor monitor, ModConfig config)
            : base("bfav_conf_voidshop", $"Set presence of void animals in shop.\nUsage: bfav_conf_voidshop <flag>\n- flag: {VoidConfig.InShop.Never}, {VoidConfig.InShop.QuestOnly}, {VoidConfig.InShop.Always}", helper, monitor, config) { }

        /// <param name="command">The name of the command invoked.</param>
        /// <param name="args">The arguments received by the command. Each word after the command name is a separate argument.</param>
        public override void Callback(string command, string[] args)
        {
            try
            {
                this.AssertGameNotLoaded();
                this.AssertRequiredArgumentOrder(args.Length, 1, "flag");
                this.AssertValidVoidInShop(args[0], out VoidConfig.InShop flag);

                this.Config.VoidFarmAnimalsInShop = flag;

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
