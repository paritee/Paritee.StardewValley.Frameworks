using BetterFarmAnimalVariety.Models;
using Paritee.StardewValleyAPI.Buidlings.AnimalShop;
using Paritee.StardewValleyAPI.Buidlings.AnimalShop.FarmAnimals;
using Paritee.StardewValleyAPI.FarmAnimals.Variations;
using Paritee.StardewValleyAPI.Players;
using Paritee.StardewValleyAPI.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety
{
    public class ModApi
    {
        private ModConfig Config;

        public ModApi(ModConfig config)
        {
            this.Config = config;
        }

        /// <returns>Returns Dictionary<string, string[]></returns>
        public Dictionary<string, string[]> GetGroupedFarmAnimals()
        {
            return this.Config.FarmAnimals.ToDictionary(entry => Enums.GetValue(entry.Key), entry => entry.Value.Types);
        }

        /// <param name="player">Paritee.StardewValleyAPI.Players</param>
        /// <returns>Returns Paritee.StardewValleyAPI.FarmAnimals.Variations.Blue</returns>
        public Blue GetBlueFarmAnimals(Player player)
        {
            BlueConfig blueConfig = new BlueConfig(player.HasSeenEvent(Blue.EVENT_ID));
            return new Blue(blueConfig);
        }

        /// <param name="player">Paritee.StardewValleyAPI.Players</param>
        /// <returns>Returns Paritee.StardewValleyAPI.FarmAnimals.Variations.Void</returns>
        public Void GetVoidFarmAnimals(Player player)
        {
            VoidConfig voidConfig = new VoidConfig(this.Config.VoidFarmAnimalsInShop, player.HasCompletedQuest(Void.QUEST_ID));
            return new Void(voidConfig);
        }

        /// <param name="player">Paritee.StardewValleyAPI.Players</param>
        /// <returns>Returns Paritee.StardewValleyAPI.Buidlings.AnimalShop</returns>
        public AnimalShop GetAnimalShop(Player player)
        {
            Dictionary<Stock.Name, string[]> available = this.Config.MapFarmAnimalsToAvailableAnimalShopStock();
            StockConfig stockConfig = new StockConfig(available, this.GetBlueFarmAnimals(player), this.GetVoidFarmAnimals(player));
            Stock stock = new Stock(stockConfig);

            return new AnimalShop(stock);
        }
    }
}
