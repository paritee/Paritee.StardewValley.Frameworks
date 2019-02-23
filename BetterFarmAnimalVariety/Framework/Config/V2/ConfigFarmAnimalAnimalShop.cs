using Newtonsoft.Json;

namespace BetterFarmAnimalVariety.Framework.Config.V2
{
    public class ConfigFarmAnimalAnimalShop
    {
        public string Name;
        public string Description;
        public string Price;
        public string Icon;

        [JsonConstructor]
        public ConfigFarmAnimalAnimalShop()
        {
            // Do nothing; this is for loading an existing config
        }
    }
}
