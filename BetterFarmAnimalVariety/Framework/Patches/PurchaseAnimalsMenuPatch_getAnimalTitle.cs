using Harmony;
using StardewValley.Menus;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    [HarmonyPatch(typeof(PurchaseAnimalsMenu))]
    [HarmonyPatch("getAnimalTitle")]
    class PurchaseAnimalsMenuPatch_getAnimalTitle : PurchaseAnimalsMenuPatch
    {
        public static bool Prefix(ref string name, ref string __result)
        {
            if (!PurchaseAnimalsMenuPatch.TryParse(name, out string[] parts))
            {
                return true;
            }

            __result = parts[0];

            return false;
        }
    }
}
