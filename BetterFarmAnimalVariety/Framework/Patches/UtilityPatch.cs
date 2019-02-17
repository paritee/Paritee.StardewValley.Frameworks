using Harmony;
using StardewValley;
using System.Collections.Generic;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    [HarmonyPatch(typeof(Utility))]
    [HarmonyPatch("getPurchaseAnimalStock")]
    class UtilityPatch : Patch
    {
        public static bool Prefix(ref List<StardewValley.Object> __result)
        {
            // Load the config and grab the farm animals for purchase
            __result = Helpers.Config.Load().GetPurchaseAnimalStock(Game1.getFarm());

            return false;
        }
    }
}
