using Harmony;
using StardewValley;
using StardewValley.Buildings;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    [HarmonyPatch(typeof(FarmAnimal), "reload")]
    class Reload : Patch
    {
        public static bool Prefix(ref FarmAnimal __instance, ref Building home)
        {
            if (__instance.Name == null)
            {
                // TODO: Debug why this happens on reload on save load/new day (coop/hatch related?)
                return true;
            }

            __instance.home = home;

            // Can't get the FarmAnimal empty constructor in a patch so need to 
            // use the reload function to handle it
            Helpers.GameSave.OverwriteFarmAnimal(ref __instance, null);

            return false;
        }
    }
}
