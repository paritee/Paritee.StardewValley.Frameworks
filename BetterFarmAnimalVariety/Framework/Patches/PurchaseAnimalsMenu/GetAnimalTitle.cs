using Harmony;
using System.Diagnostics;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Patches.PurchaseAnimalsMenu
{
    [HarmonyPatch(typeof(StardewValley.Menus.PurchaseAnimalsMenu))]
    [HarmonyPatch("getAnimalTitle")]
    class GetAnimalTitle : Patch
    {
        public static bool Prefix(ref string name, ref string __result)
        {
            Debug.WriteLine($"getAnimalTitle hoverText {name}");

            // Get the description from the config
            string category = name;

            __result = Helpers.Config.Load<ModConfig>().FarmAnimals
                .First(o => o.Category.Equals(category))
                .AnimalShop.Name;

            return false;
        }
    }
}
