using BetterFarmAnimalVariety.Models;
using Paritee.StardewValleyAPI.FarmAnimals;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterFarmAnimalVariety
{
    public class ModCommand
    {
        private readonly ModConfig Config;
        private readonly IModHelper Helper;
        private readonly IMonitor Monitor;

        public ModCommand(ModConfig config, IModHelper helper, IMonitor monitor)
        {
            this.Config = config;
            this.Helper = helper;
            this.Monitor = monitor;
        }

        public void SetUp()
        {
            (new Commands.FarmAnimalsCommand(this.Config, this.Helper, this.Monitor)).SetUp();
        }
    }
}
