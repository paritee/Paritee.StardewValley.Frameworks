using StardewValley;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety
{
    public class ModConfig
    {
        public string Format;
        public bool IsEnabled;
        public bool RandomizeNewbornFromCategory;
        public bool RandomizeHatchlingFromCategory;
        public bool IgnoreParentProduceCheck;
        public List<Framework.Config.FarmAnimal> FarmAnimals;

        public ModConfig()
        {
            this.Format = null;
            this.IsEnabled = true;
            this.RandomizeNewbornFromCategory = false;
            this.RandomizeHatchlingFromCategory = false;
            this.IgnoreParentProduceCheck = false;
            this.FarmAnimals = new List<Framework.Config.FarmAnimal>();
        }

        public Dictionary<string, List<string>> GroupTypesByCategory()
        {
            return this.FarmAnimals.ToDictionary(o => o.Category, o => new List<string>(o.Types));
        }

        public bool IsValidFormat(string targetFormat)
        {
            return this.Format != null && this.Format.Equals(targetFormat);
        }

        public void SeedVanillaFarmAnimals()
        {
            this.FarmAnimals = Framework.Constants.FarmAnimalCategory.All()
                .Select(o => new Framework.Config.FarmAnimal(o))
                .ToList();
        }

        public List<StardewValley.Object> GetPurchaseAnimalStock(Farm farm)
        {
            return this.FarmAnimals.Where(o => o.CanBePurchased())
                .Select(o => o.ToAnimalAvailableForPurchase(farm))
                .ToList();
        }
    }
}