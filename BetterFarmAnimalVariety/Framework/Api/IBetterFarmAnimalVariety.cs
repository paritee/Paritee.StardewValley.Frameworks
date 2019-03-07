using System.Collections.Generic;

namespace BetterFarmAnimalVariety.Framework.Api
{
    interface IBetterFarmAnimalVariety
    {
        /// <summary>Determine if the mod is enabled.</summary>
        /// <returns>Returns bool</returns>
        bool IsEnabled();

        /// <summary>Get all farm animal categories that have been loaded.</summary>
        /// <returns>Returns List<Paritee.Core.Models.FarmAnimalCategory></returns>
        List<Paritee.StardewValley.Core.Models.FarmAnimalCategory> GetFarmAnimalCategories();

        /// <summary>Get the farm animal's types from the save data.</summary>
        /// <returns>Returns Dictionary<long, KeyValuePair<string, string>></returns>
        Dictionary<long, KeyValuePair<string, string>> GetFarmAnimalTypeHistory();
    }
}
