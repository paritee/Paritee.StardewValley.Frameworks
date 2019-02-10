using Harmony;
using StardewValley;
using StardewValley.Buildings;
using System.Diagnostics;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    [HarmonyPatch(typeof(FarmAnimal), "reload")]
    class FarmAnimalPatch_reload : Patch
    {
        public static bool Prefix(ref FarmAnimal __instance, ref Building home)
        {
            __instance.home = home;

            // Catch get the FarmAnimal empty constructor in a patch so need to abues this for now.
            Debug.WriteLine($"ENTERING IN FROM FarmAnimalPatch_reload");
            Helpers.GameSave.OverwriteFarmAnimal(ref __instance, null);

            return false;
        }
    }
}
