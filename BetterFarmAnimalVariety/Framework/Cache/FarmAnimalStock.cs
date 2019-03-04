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

        public FarmAnimalStock(PariteeCore.Models.FarmAnimalCategory farmAnimalStock)
        {
            this.Name = farmAnimalStock.DisplayName;
            this.Description = farmAnimalStock.Description;
            this.Icon = this.GetDefaultIconPath(farmAnimalStock.ToString());
            this.Exclude = farmAnimalStock.ExcludeFromShop.Any()
                ? farmAnimalStock.ExcludeFromShop.Select(o => o.ToString()).ToList()
                : new List<string>();
        }

        public string GetDefaultIconPath(string category)
        {
            string fileName = $"{category.Replace(" ", "")}{Constants.Mod.AnimalShopIconExtension}";

            return Helpers.Mod.GetShortAssetPath(Path.Combine(Constants.Mod.AnimalShopIconDirectory, fileName));
        }
    }
}
