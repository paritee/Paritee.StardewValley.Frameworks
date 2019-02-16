using Harmony;
using StardewValley;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    //[HarmonyPatch(typeof(Game1))]
    //[HarmonyPatch("parseDebugInput")]
    class Game1Patch : Patch
    {
        public static void Postfix(ref Game1 __instance, ref string debugInput)
        {
            if (!debugInput.Equals("fixAnimals"))
            {
                return;
            }

            Patch.CleanSaveData();
        }
    }
}
