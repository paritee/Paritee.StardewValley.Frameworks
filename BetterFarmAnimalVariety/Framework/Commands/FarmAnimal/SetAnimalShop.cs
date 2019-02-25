using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Commands.FarmAnimal
{
    class SetAnimalShop : Command
    {
        public SetAnimalShop(IModHelper helper, IMonitor monitor, ModConfig config)
            : base("bfav_fa_setshop", $"Set the availability of this category in the animal shop.\nUsage: bfav_fa_setshop <category> <animalshop>\n- category: the unique animal category.\n- animalshop: {true.ToString().ToLower()} or {false.ToString().ToLower()}.", helper, monitor, config) { }

        /// <param name="command">The name of the command invoked.</param>
        /// <param name="args">The arguments received by the command. Each word after the command name is a separate argument.</param>
        public override void Callback(string command, string[] args)
        {
            try
            {
                Helpers.Assert.GameNotLoaded();
                Helpers.Assert.RequiredArgumentOrder(args.Length, 1, "category");

                string category = args[0].Trim();

                Helpers.Assert.FarmAnimalCategoryExists(category);
                Helpers.Assert.RequiredArgumentOrder(args.Length, 2, "animalshop");

                string animalShop = args[1].Trim().ToLower();
                
                Framework.Config.FarmAnimal animal = this.Config.GetCategory(category);

                Helpers.Assert.ChangeInPurchaseState(animalShop, animal.CanBePurchased(), out bool result);

                Framework.Config.FarmAnimalStock configFarmAnimalAnimalShop = result
                    ? Framework.Config.FarmAnimalStock.CreateWithPlaceholders(category)
                    : null;

                animal.AnimalShop = configFarmAnimalAnimalShop;

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
