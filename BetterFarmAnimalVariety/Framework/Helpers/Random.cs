using StardewValley;

namespace BetterFarmAnimalVariety.Framework.Helpers
{
    class Random
    {
        public static System.Random GetNumberGenerator()
        {
            return Game1.random;
        }

        public static double NextDouble()
        {
            return Helpers.Random.GetNumberGenerator().NextDouble();
        }

        public static int Next()
        {
            return Helpers.Random.GetNumberGenerator().Next();
        }

        public static int Next(int maxValue)
        {
            return Helpers.Random.GetNumberGenerator().Next(maxValue);
        }

        public static int Next(int minValue, int maxValue)
        {
            return Helpers.Random.GetNumberGenerator().Next(minValue, maxValue);
        }
    }
}
