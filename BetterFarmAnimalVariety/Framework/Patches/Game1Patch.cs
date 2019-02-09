using BetterFarmAnimalVariety.Framework.Data;
using StardewValley;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    class Game1Patch
    {
        // StardewValley.Game1.parseDebugInput
        public static void ParseDebugInputPostfix(ref Game1 __instance, ref string debugInput)
        {
            if (!debugInput.Equals("fixAnimals"))
            {
                return;
            }

            FarmAnimalsSaveData saveData = FarmAnimalsSaveData.Deserialize();

            saveData.CleanTypeHistory();
        }
    }
}
