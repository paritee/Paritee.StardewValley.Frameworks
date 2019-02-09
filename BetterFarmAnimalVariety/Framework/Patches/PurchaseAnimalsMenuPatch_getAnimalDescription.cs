using Harmony;
using StardewValley.Menus;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    [HarmonyPatch(typeof(PurchaseAnimalsMenu))]
    [HarmonyPatch("getAnimalDescription")]
    class PurchaseAnimalsMenuPatch_getAnimalDescription : PurchaseAnimalsMenuPatch
    {
        public static bool Prefix(ref string name, ref string __result)
        {
            if (!PurchaseAnimalsMenuPatch.TryParse(name, out string[] parts))
            {
                return true;
            }

            __result = parts[1];

            return false;
        }
    }
}
