using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.IO;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Commands
{
    abstract class Command
    {
        public string Name;
        public string Description;

        protected readonly IModHelper Helper;
        protected readonly IMonitor Monitor;
        protected ModConfig Config;

        public Command (string name, string description, IModHelper helper, IMonitor monitor, ModConfig config)
        {
            this.Name = name;
            this.Description = description;

            this.Helper = helper;
            this.Monitor = monitor;
            this.Config = config;
        }

        public abstract void Callback(string command, string[] args);

        protected string DescribeFarmAnimalCategory(Framework.Config.FarmAnimal animal)
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

            return output;
        }
    }
}
