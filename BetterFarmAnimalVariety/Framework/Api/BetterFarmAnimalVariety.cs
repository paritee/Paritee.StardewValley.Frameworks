using BetterFarmAnimalVariety.Framework.SaveData;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Api
{
    public class BetterFarmAnimalVariety : Api.IBetterFarmAnimalVariety
    {
        private readonly ModConfig Config;

        public BetterFarmAnimalVariety(ModConfig config)
        {
            this.Config = config;
        }

        /// <summary>Determine if the mod is enabled.</summary>
        /// <returns>Returns bool</returns>
        public bool IsEnabled()
        {
            return this.Config.IsEnabled;
        }

        /// <summary>Get all farm animal categories that have been loaded.</summary>
        /// <returns>Returns List<T></returns>
        public Dictionary<string, List<string>> GetFarmAnimalCategories()
        {
            return Framework.Helpers.FarmAnimals.GetCategories()
                .ToDictionary(o => o.Category, o => o.Types.Select(t => t.Type).ToList());
        }

        /// <param name="farm">StardewValley.Farm</param>
        /// <summary>Determine if the mod is enabled.</summary>
        /// <returns>Returns List<StardewValley.Object></returns>
        public List<StardewValley.Object> GetAnimalShopStock(StardewValley.Farm farm)
        {
            return Helpers.FarmAnimals.GetPurchaseAnimalStock(farm);
        }

        /// <summary>Determine if the mod is enabled.</summary>
        public Dictionary<string, Texture2D> GetAnimalShopIcons()
        {
            // Grab the icons from the config by category
             return Helpers.FarmAnimals.GetCategories()
                .Where(o => o.CanBePurchased())
                .ToDictionary(o => o.Category, o => o.GetAnimalShopIconTexture());
        }

        /// <param name="category">string</param>
        /// <param name="farmer">StardewValley.Farmer</param>
        /// <summary>Determine if the mod is enabled.</summary>
        /// <returns>Returns string</returns>
        public string GetRandomAnimalShopType(string category, StardewValley.Farmer farmer)
        {
            Decorators.Farmer moddedFarmer = new Decorators.Farmer(farmer);

            List<string> types = Helpers.FarmAnimals.GroupPurchaseableTypesByCategory()[category];

            // Remove blue chickens if needed
            types = moddedFarmer.SanitizeBlueChickens(types);

            // Remove any types that the player cannot afford
            types = moddedFarmer.SanitizeAffordableTypes(types);

            return types[PariteeCore.Utilities.Random.Next(types.Count)];
        }

        /// <summary>Get the farm animal's types from the save data.</summary>
        /// <returns>Returns Dictionary<long, KeyValuePair<string, string>></returns>
        public Dictionary<long, KeyValuePair<string, string>> GetFarmAnimalTypeHistory()
        {
            Framework.Helpers.Assert.SaveLoaded();

            FarmAnimals saveData = Framework.Helpers.Mod.ReadSaveData<FarmAnimals>(Framework.Constants.Mod.FarmAnimalsSaveDataKey);

            return saveData.Animals.ToDictionary(o => o.Id, o => o.TypeLog.ConvertToKeyValuePair());
        }
    }
}
