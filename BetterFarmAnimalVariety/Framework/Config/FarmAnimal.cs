using Newtonsoft.Json;
using System.Linq;
using StardewValley;

namespace BetterFarmAnimalVariety.Framework.Config
{
    public class FarmAnimal
    {
        [JsonProperty(Order = 1)]
        public string Category;

        [JsonProperty(Order = 2)]
        public string[] Types;

        [JsonProperty(Order = 3)]
        public string[] Buildings;

        [JsonProperty(Order = 4)]
        public FarmAnimalStock AnimalShop;

        [JsonConstructor]
        public FarmAnimal()
        {
            // Do nothing; this is for loading an existing config
        }

        public FarmAnimal(Constants.FarmAnimalCategory farmAnimalStock)
        {
            this.Category = farmAnimalStock.ToString();
            this.Types = farmAnimalStock.Types.Select(o => o.ToString()).ToArray();
            this.Buildings = farmAnimalStock.Buildings;

            this.AnimalShop = farmAnimalStock.CanBePurchased()
                ? new FarmAnimalStock(farmAnimalStock)
                : null;
        }

        public bool CanBePurchased()
        {
            return this.AnimalShop != null;
        }

        public bool CanBePurchased(string type)
        {
            if (!this.CanBePurchased())
            {
                return false;
            }

            return !this.AnimalShop.Exclude.Contains(type);
        }

        public StardewValley.Object ToAnimalAvailableForPurchase(Farm farm)
        {
            if (!this.CanBePurchased())
            {
                return null;
            }

            return Api.AnimalShop.FormatAsAnimalAvailableForPurchase(farm, this.Category, this.AnimalShop.Name, this.AnimalShop.Price, this.Buildings);
        }
    }
}
