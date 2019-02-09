using StardewValley;
using System.Reflection;

namespace BetterFarmAnimalVariety.Framework.Helpers
{
    class Multiplayer
    {
        private static StardewValley.Multiplayer GetInstance()
        {
            return Helpers.Reflection.GetFieldValue<StardewValley.Multiplayer>(typeof(Game1), "multiplayer", BindingFlags.Static | BindingFlags.NonPublic);
        }

        public static long GetNewId()
        {
            return Helpers.Multiplayer.GetInstance().getNewID();
        }
    }
}
