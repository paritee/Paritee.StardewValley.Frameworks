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
        public static List<string> RestrictedFarmAnimalTypes => new List<string>()
        {
            // The assets for these animals are kept in the same directory as the 
            // farm animal assets. Naming a type one of these keywords would cause 
            // the asset load to fail.
            PariteeCore.Constants.VanillaAnimalType.Cat.ToString(),
            PariteeCore.Constants.VanillaAnimalType.Dog.ToString(),
            PariteeCore.Constants.VanillaAnimalType.Horse.ToString(),
        };

        // Content packs
        public const string ContentPackContentFileName = "content.json";
        public const string ContentPackManifestFileName = "manifest.json";
        public const string ConfigMigrationContentPackPrefix = "[BFAV]";
        public static string ConfigMigrationContentPackName => $"{ConfigMigrationContentPackPrefix} My Content Pack";
        public const string ConfigMigrationContentPackAuthor = "Anonymous";
        public const string ConfigMigrationContentPackVersion = "1.0.0";
        public const string ConfigMigrationContentPackDescription = "Your custom content pack for BFAV";
        public static string ConfigMigrationContentPackFullPath => Path.Combine(Directory.GetParent(PariteeCore.Constants.Mod.Path).FullName, ConfigMigrationContentPackName);
        
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
