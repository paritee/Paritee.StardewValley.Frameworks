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

        protected string DescribeFarmAnimalCategory(Framework.Cache.FarmAnimalCategory animal)
        {
            string output = "";

            output += $"{animal.Category}\n";
            output += $"- Types: {String.Join(",", animal.Types)}\n";
            output += $"- Buildings: {String.Join(",", animal.Buildings)}\n";
            
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

            output += $"- Asset Source Directory: {animal.AssetSourceDirectory}\n";

            return output;
        }
    }
}
