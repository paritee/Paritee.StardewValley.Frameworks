using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Cache
{
    class FarmAnimalStock
    {
        [JsonProperty(Order = 0)]
        public string Name;

        [JsonProperty(Order = 1)]
        public string Description;

        [JsonProperty(Order = 2)]
        public string Icon;

        [JsonProperty(Order = 3)]
        public List<string> Exclude;

        public FarmAnimalStock()
        {
            // Do nothing; this is for loading an existing config
        }

        public FarmAnimalStock(PariteeCore.Characters.LivestockCategory livestockCategory)
        {
            this.Name = livestockCategory.AnimalShop.Name;
            this.Description = livestockCategory.AnimalShop.Description;
            this.Icon = this.GetDefaultIconPath(livestockCategory.ToString());
            this.Exclude = livestockCategory.AnimalShop.Exclude != null && livestockCategory.AnimalShop.Exclude.Any()
                ? livestockCategory.AnimalShop.Exclude.Select(o => o.ToString()).ToList()
                : new List<string>();
        }

        public string GetDefaultIconPath(string category)
        {
            string fileName = $"{category.Replace(" ", "")}{Constants.Mod.AnimalShopIconExtension}";

            return Helpers.Mod.GetShortAssetPath(Path.Combine(Constants.Mod.AnimalShopIconDirectory, fileName));
        }
    }
}
