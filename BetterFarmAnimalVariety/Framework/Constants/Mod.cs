using System.IO;
using System.Reflection;

namespace BetterFarmAnimalVariety.Framework.Constants
{
    class Mod
    {
        public const string ModKey = "paritee.betterfarmanimalvariety";
        public static string ModPath { get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); } }
        public const string FarmAnimalsSaveDataFileName = "farmanimals.json";
        public const string AssetsDirectory = "assets";
    }
}
