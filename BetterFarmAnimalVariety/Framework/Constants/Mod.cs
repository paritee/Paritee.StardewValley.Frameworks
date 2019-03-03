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
        public const int AnimalShopPricePlaceholder = 1000;
        public static string AnimalShopDescriptionPlaceholder => PariteeCore.Api.Content.LoadString("Strings\\StringsFromCSFiles:BluePrint.cs.1");
        public static string AnimalShopIconDirectory = "AnimalShop";
        public static string AnimalShopIconExtension = ".png";

        // Content packs
        public const string ContentPackFileName = "content.json";

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
