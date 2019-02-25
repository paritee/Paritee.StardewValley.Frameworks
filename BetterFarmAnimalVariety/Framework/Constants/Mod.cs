using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Constants
{
    class Mod
    {
        // Core
        public const string Key = "paritee.betterfarmanimalvariety";

        // Config
        public const string ConfigFileName = "config.json";
        public const int AnimalShopPricePlaceholder = 1000;
        public static string AnimalShopDescriptionPlaceholder => PariteeCore.Api.Content.LoadString("Strings\\StringsFromCSFiles:BluePrint.cs.1");
        public static string AnimalShopIconDirectory = "AnimalShop";
        public static string AnimalShopIconExtension = ".png";

        // Save data
        public const string FarmAnimalsSaveDataKey = "farm-animals";

        // Assets
        public const string AssetsDirectory = "assets";
    }
}
