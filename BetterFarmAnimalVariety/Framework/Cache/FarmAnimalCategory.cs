using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StardewValley;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Cache
{
    class FarmAnimalCategory
    {
        [JsonProperty(Order = 0)]
        public string Category;

        [JsonProperty(Order = 1)]
        public List<FarmAnimalType> Types = new List<FarmAnimalType>();

        [JsonProperty(Order = 2)]
        public List<string> Buildings = new List<string>();

        [JsonProperty(Order = 3)]
        public FarmAnimalStock AnimalShop;

        public FarmAnimalCategory() { }

        public FarmAnimalCategory(ContentPacks.Category category)
        {
            this.Category = category.Category;
            this.Types = category.Types;
            this.Buildings = category.Buildings;
            this.AnimalShop = category.AnimalShop;
        }

        public FarmAnimalCategory(string assetSourceDirectory, PariteeCore.Characters.LivestockCategory category)
        {
            this.Category = category.ToString();
            this.Types = category.Types.Select(o => new FarmAnimalType(o)).ToList();
            this.Buildings = category.Buildings.ToList();
            this.AnimalShop = category.CanBePurchased()
                ? new FarmAnimalStock(category)
                : null;

            if (this.CanBePurchased())
            {
                this.AnimalShop.Icon = Path.Combine(assetSourceDirectory, this.AnimalShop.Icon);
            }
        }

        public Texture2D GetAnimalShopIconTexture()
        {
            return this.CanBePurchased() ? PariteeCore.Utilities.Mod.LoadTexture(this.AnimalShop.Icon) : null;
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

            return this.AnimalShop.Exclude == null ? true : !this.AnimalShop.Exclude.Contains(type);
        }

        public StardewValley.Object ToAnimalAvailableForPurchase(Farm farm)
        {
            if (!this.CanBePurchased())
            {
                return null;
            }

            return PariteeCore.Locations.AnimalShop.FormatAsAnimalAvailableForPurchase(farm, this.Category, this.AnimalShop.Name, this.Types.Select(o => o.Type).ToArray(), this.Buildings.ToArray());
        }
    }
}
