using Harmony;
using System.Collections.Generic;
using System.Diagnostics;

namespace BetterFarmAnimalVariety.Framework.Patches.Utility
{
    [HarmonyPatch(typeof(StardewValley.Utility))]
    [HarmonyPatch("getPurchaseAnimalStock")]
    class GetPurchaseAnimalStock : Patch
    {
        public static bool Prefix(ref List<StardewValley.Object> __result)
        {
            Debug.WriteLine($"getPurchaseAnimalStock");

            // Load the config and grab the farm animals for purchase
            __result = Helpers.Config.Load<ModConfig>().GetPurchaseAnimalStock(StardewValley.Game1.getFarm());

            return false;
        }
    }
}
