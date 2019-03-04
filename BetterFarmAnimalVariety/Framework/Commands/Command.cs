using StardewModdingAPI;
using System.Collections.Generic;

namespace BetterFarmAnimalVariety.Framework.Commands
{
    abstract class Command
    {
        public string Name;
        public string Description;

        protected readonly IModHelper Helper;
        protected readonly IMonitor Monitor;

        public Command (string name, string description, IModHelper helper, IMonitor monitor)
        {
            this.Name = name;
            this.Description = description;

            this.Helper = helper;
            this.Monitor = monitor;
        }

        public abstract void Callback(string command, string[] args);

        protected string DescribeFarmAnimalCategory(Cache.FarmAnimalCategory animal)
        {
            string output = "";

            output += $"{animal.Category}\n";
            output += $"- Buildings: {string.Join(",", animal.Buildings)}\n";

            if (animal.CanBePurchased())
            {
                output += $"- AnimalShop:\n";
                output += $"-- Name: {animal.AnimalShop.Name}\n";
                output += $"-- Description: {animal.AnimalShop.Description}\n";
                output += $"-- Price: {animal.AnimalShop.Price}\n";
                output += $"-- Icon: {animal.AnimalShop.Icon}\n";
            }
            else
            {
                output += $"- AnimalShop: null\n";
            }

            output += $"- Types:\n";

            foreach (Cache.FarmAnimalType type in animal.Types)
            {
                output += $"-- Type: {type.Type}\n";
                output += $"--- Data: {type.Data ?? "null"}\n";
                output += $"--- AdultSprite: {type.AdultSprite ?? "null"}\n";
                output += $"--- BabySprite: {type.BabySprite ?? "null"}\n";
                output += $"--- ShearedSprite: {type.ShearedSprite ?? "null"}\n";

                if (type.Localization != null)
                {
                    output += $"--- Localization:\n";

                    foreach (KeyValuePair<string, string[]> entry in type.Localization)
                    {
                        output += $"---- {entry.Key}: {string.Join(",", entry.Value)}\n";
                    }
                }
                else
                {
                    output += $"--- Localization: null\n";
                }
            }

            return output;
        }
    }
}
