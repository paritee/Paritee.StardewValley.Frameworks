using Harmony;
using System.Collections.Generic;

namespace BetterFarmAnimalVariety.Framework.Patches.Utility
{
    [HarmonyPatch(typeof(StardewValley.Utility))]
    [HarmonyPatch("getPurchaseAnimalStock")]
    class GetPurchaseAnimalStock : Patch
    {
        public static bool Prefix(ref List<StardewValley.Object> __result)
        {
            // Load the config and grab the farm animals for purchase
            __result = Helpers.Mod.LoadConfig<ModConfig>().GetPurchaseAnimalStock(StardewValley.Game1.getFarm());

            return false;
        }
    }
}
