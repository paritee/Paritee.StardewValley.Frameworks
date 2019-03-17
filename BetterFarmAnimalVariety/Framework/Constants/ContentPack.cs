using System.IO;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Constants
{
    class ContentPack
    {
        // Files
        public const string ContentFileName = "content.json";
        public const string ManifestFileName = "manifest.json";

        // Config migration
        public const string ConfigMigrationPrefix = "[BFAV]";
        public static string ConfigMigrationName => $"{ConfigMigrationPrefix} My Content Pack";
        public const string ConfigMigrationAuthor = "Anonymous";
        public const string ConfigMigrationVersion = "1.0.0";
        public const string ConfigMigrationDescription = "Your custom content pack for BFAV";
        public static string ConfigMigrationFullPath => Path.Combine(Directory.GetParent(PariteeCore.Utilities.Mod.Path).FullName, ConfigMigrationName);
    }
}
