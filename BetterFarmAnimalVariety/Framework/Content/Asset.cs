using StardewValley;

namespace BetterFarmAnimalVariety.Framework.Content
{
    class Asset
    {
        public static T Load<T>(string name)
        {
            return Game1.content.Load<T>(name);
        }
    }
}
