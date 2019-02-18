using StardewValley;

namespace BetterFarmAnimalVariety.Framework.Helpers
{
    class Random
    {
        public static double NextDouble()
        {
            return Game1.random.NextDouble();
        }

        public static int Next()
        {
            return Game1.random.Next();
        }

        public static int Next(int maxValue)
        {
            return Game1.random.Next(maxValue);
        }

        public static int Next(int minValue, int maxValue)
        {
            return Game1.random.Next(minValue, maxValue);
        }
    }
}
