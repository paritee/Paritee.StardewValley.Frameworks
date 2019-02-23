using Newtonsoft.Json;

namespace BetterFarmAnimalVariety.Framework.Config.V2
{
    public class ConfigFarmAnimal
    {
        public string[] Types;
        public string[] Buildings;
        public ConfigFarmAnimalAnimalShop AnimalShop;

        [JsonConstructor]
        public ConfigFarmAnimal()
        {
            // Do nothing; this is for loading an existing config
        }

        public bool ShouldSerializeCategory()
        {
            return false;
        }

        public bool CanBePurchased()
        {
            return this.AnimalShop.Name != null && this.AnimalShop.Description != null && this.AnimalShop.Price != null && this.AnimalShop.Icon != null;
        }
    }
}
