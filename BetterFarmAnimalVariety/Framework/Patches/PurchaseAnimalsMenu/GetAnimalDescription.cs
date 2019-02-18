using Harmony;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Patches.PurchaseAnimalsMenu
{
    [HarmonyPatch(typeof(StardewValley.Menus.PurchaseAnimalsMenu))]
    [HarmonyPatch("getAnimalDescription")]
    class GetAnimalDescription : Patch
    {
        public static bool Prefix(ref string name, ref string __result)
        {
            // Get the description from the config
            string category = name;

            __result = Helpers.Mod.LoadConfig<ModConfig>().GetCategory(category).AnimalShop.Description;

            return false;
        }
    }
}
