using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BetterFarmAnimalVariety.Framework.Api
{
    public interface IBetterFarmAnimalVariety
    {
        /// <summary>Determine if the mod is enabled.</summary>
        /// <returns>Returns bool</returns>
        bool IsEnabled();

        /// <summary>Get all farm animal categories that have been loaded.</summary>
        /// <returns>Returns Dictionary<string, List<string>></returns>
        Dictionary<string, List<string>> GetFarmAnimalCategories();

        /// <param name="farm">StardewValley.Farm</param>
        /// <summary>Get all livestock options.</summary>
        /// <returns>Returns List<StardewValley.Object></returns>
        List<StardewValley.Object> GetAnimalShopStock(StardewValley.Farm farm);

        /// <summary>Get all livestock icons.</summary>
        /// <returns>Returns Dictionary<string, Texture2D></returns>
        Dictionary<string, Texture2D> GetAnimalShopIcons();

        /// <param name="category">string</param>
        /// <param name="farmer">StardewValley.Farmer</param>
        /// <summary>Get a random farm animal type from the animal shop category.</summary>
        /// <returns>Returns string</returns>
        string GetRandomAnimalShopType(string category, StardewValley.Farmer farmer);

        /// <summary>Get the farm animal's types from the save data.</summary>
        /// <returns>Returns Dictionary<long, KeyValuePair<string, string>></returns>
        Dictionary<long, KeyValuePair<string, string>> GetFarmAnimalTypeHistory();
    }
}
