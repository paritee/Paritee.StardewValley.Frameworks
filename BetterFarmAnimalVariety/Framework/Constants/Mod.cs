using System.Collections.Generic;
using System.IO;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Constants
{
    class Mod
    {
        // Core
        public const string Key = "Paritee.BetterFarmAnimalVariety";

        // Config
        public const string ConfigFileName = "config.json";
        public const string AnimalShopIconDirectory = "AnimalShop";
        public const string AnimalShopIconExtension = ".png";
        public static List<PariteeCore.Models.Animal> RestrictedFarmAnimalTypes => new List<PariteeCore.Models.Animal>()
        {
            // The assets for these animals are kept in the same directory as the 
            // farm animal assets. Naming a type one of these keywords would cause 
            // the asset load to fail.
            PariteeCore.Constants.Animals.Pet.Cat,
            PariteeCore.Constants.Animals.Pet.Dog,
            PariteeCore.Constants.Animals.Mount.Horse,
        };

        // Save data
        public const string FarmAnimalsSaveDataKey = "farm-animals";

        // Cache
        public const string CacheDirectory = "cache";
        public static string CacheFullPath => Path.Combine(PariteeCore.Constants.Mod.Path, Constants.Mod.CacheDirectory);
        public static string FarmAnimalsCacheFileName = "farm-animals.json";

        // Assets
        public const string AssetsDirectory = "assets";
    }
}
