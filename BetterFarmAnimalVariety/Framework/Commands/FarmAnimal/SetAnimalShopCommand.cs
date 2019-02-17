using StardewModdingAPI;
using System;
using System.Collections.Generic;

namespace BetterFarmAnimalVariety.Framework.Commands.FarmAnimal
{
    class SetAnimalShopCommand : Command
    {
        public SetAnimalShopCommand(IModHelper helper, IMonitor monitor, ModConfig config)
            : base("bfav_fa_setshop", $"Set the availability of this category in the animal shop.\nUsage: bfav_fa_setshop <category> <animalshop>\n- category: the unique animal category.\n- animalshop: {Command.True} or {Command.False}.", helper, monitor, config) { }

        /// <param name="command">The name of the command invoked.</param>
        /// <param name="args">The arguments received by the command. Each word after the command name is a separate argument.</param>
        public override void Callback(string command, string[] args)
        {
            try
            {
                this.AssertGameNotLoaded();
                this.AssertRequiredArgumentOrder(args.Length, 1, "category");

                string category = args[0].Trim();

                this.AssertFarmAnimalCategoryExists(category);
                this.AssertRequiredArgumentOrder(args.Length, 2, "animalshop");

                string animalShop = args[1].Trim().ToLower();

                this.AssertValidBoolean(animalShop, "animalshop", out bool result);
                this.AssertAnimalShopChange(animalShop, this.Config.FarmAnimals[category].CanBePurchased());

                Framework.Config.FarmAnimalStock configFarmAnimalAnimalShop = Framework.Helpers.Commands.GetAnimalShopConfig(category, animalShop);

                this.Config.FarmAnimals[category].AnimalShop = configFarmAnimalAnimalShop;

                this.Helper.WriteConfig(this.Config);

                string output = Helpers.Commands.DescribeFarmAnimalCategory(new KeyValuePair<string, Framework.Config.FarmAnimal>(category, this.Config.FarmAnimals[category]));

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
