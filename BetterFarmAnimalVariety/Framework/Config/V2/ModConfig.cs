using System.Collections.Generic;

namespace BetterFarmAnimalVariety.Framework.Config.V2
{
    public class ModConfig
    {
        public string Format;
        public bool IsEnabled;
        public string VoidFarmAnimalsInShop;
        public Dictionary<string, ConfigFarmAnimal> FarmAnimals;

        private const string ChickenCategory = "Chicken";
        private const string VoidFarmAnimalsInShopAlways = "Always";

        public bool IsChickenCategory(string category)
        {
            return category == ModConfig.ChickenCategory;
        }

        public bool AreVoidFarmAnimalsInShopAlways()
        {
            return this.VoidFarmAnimalsInShop == ModConfig.VoidFarmAnimalsInShopAlways;
        }
    }
}
