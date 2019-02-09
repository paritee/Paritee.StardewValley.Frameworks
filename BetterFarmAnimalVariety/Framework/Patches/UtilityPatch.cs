using Harmony;
using StardewValley;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    [HarmonyPatch(typeof(Utility))]
    [HarmonyPatch("getPurchaseAnimalStock")]
    class UtilityPatch : Patch
    {
        // TODO: StardewValley.Utility.getPurchaseAnimalStock

        // StardewValley.Utility.fixAllAnimals
        public static void Postfix()
        {
            Patch.CleanSaveData();
        }
    }
}
