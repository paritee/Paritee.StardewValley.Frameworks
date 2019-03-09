﻿using Microsoft.Xna.Framework.Graphics;
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
        public List<FarmAnimalType> Types;

        [JsonProperty(Order = 2)]
        public List<string> Buildings;

        [JsonProperty(Order = 3)]
        public FarmAnimalStock AnimalShop;

        public FarmAnimalCategory() { }

        public FarmAnimalCategory(string assetSourceDirectory, ContentPacks.FarmAnimalCategory category)
        {
            this.Category = category.Category;
            this.Types = category.Types;
            this.Buildings = category.Buildings;
            this.AnimalShop = category.AnimalShop;

            if (this.CanBePurchased())
            {
                this.AnimalShop.Icon = Path.Combine(assetSourceDirectory, this.AnimalShop.Icon);
            }
        }

        public FarmAnimalCategory(string assetSourceDirectory, PariteeCore.Models.FarmAnimalCategory category)
        {
            this.Category = category.ToString();
            this.Types = category.Types.Select(o => new FarmAnimalType(o, category.DeluxeProduceLuck)).ToList();
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
            return this.CanBePurchased() ? PariteeCore.Api.Mod.LoadTexture(this.AnimalShop.Icon) : null;
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

            return PariteeCore.Api.AnimalShop.FormatAsAnimalAvailableForPurchase(farm, this.Category, this.AnimalShop.Name, this.Types.Select(o => o.Type).ToArray(), this.Buildings.ToArray());
        }
    }
}
