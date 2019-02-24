using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterFarmAnimalVariety.Framework.Commands.FarmAnimal
{
    class SetAnimalShopExclude : Command
    {
        public SetAnimalShopExclude(IModHelper helper, IMonitor monitor, ModConfig config)
            : base("bfav_fa_setexclude", $"Set the category's types to exclude from the animal shop.\nUsage: bfav_fa_setshopprice <category> <exclude>\n- category: the unique animal category.\n- exclude: a comma separated list in quotes (ex \"Void Chicken\").", helper, monitor, config) { }

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
                Helpers.Assert.FarmAnimalCanBePurchased(category);
                Helpers.Assert.RequiredArgumentOrder(args.Length, 2, "exclude");

                string[] exclude = args[1].Split(',').Select(i => i.Trim()).ToArray();

                if (exclude.Any())
                {
                    Helpers.Assert.FarmAnimalTypesExist(new List<string>(exclude));
                }

                Framework.Config.FarmAnimal animal = this.Config.GetCategory(category);

                animal.AnimalShop.Exclude = exclude;

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