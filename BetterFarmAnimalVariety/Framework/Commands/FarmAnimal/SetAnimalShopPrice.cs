﻿using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Commands.FarmAnimal
{
    class SetAnimalShopPrice : Command
    {
        public SetAnimalShopPrice(IModHelper helper, IMonitor monitor, ModConfig config)
            : base("bfav_fa_setshopprice", $"Set the category's animal shop price.\nUsage: bfav_fa_setshopprice <category> <price>\n- category: the unique animal category.\n- price: the integer amount.", helper, monitor, config) { }

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
                this.AssertFarmAnimalCanBePurchased(category);
                this.AssertRequiredArgumentOrder(args.Length, 2, "price");
                this.AssertValidMoneyAmount(args[1]);

                Framework.Config.FarmAnimal animal = this.Config.GetCategory(category);

                animal.AnimalShop.Price = Int32.Parse(args[1].Trim());

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