using System.IO;

namespace Butchery.Framework.Constants
{
    class Mod
    {
        public const string BetterFarmAnimalVariety = "Paritee.BetterFarmAnimalVariety";
        public static string AnimalsData => Path.Combine("data", "animals.json");
    }
}
