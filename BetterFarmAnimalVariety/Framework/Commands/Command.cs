using StardewModdingAPI;
using System;

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
            output += $"- Types: {string.Join(",", animal.Types)}\n";
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

            return output;
        }
    }
}
