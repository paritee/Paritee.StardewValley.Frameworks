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
            if (__instance.Name == null)
            {
                Debug.WriteLine($"__instance.myID.Value {__instance.myID.Value}");
                Debug.WriteLine($"__instance.Name {__instance.Name}");
                Debug.WriteLine($"__instance.Sprite == null {__instance.Sprite == null}");
                Debug.WriteLine($"__instance.type.Value {__instance.type.Value}");

                // TODO: Debug why this happens on reload on save load/new day
                return true;
            }

            __instance.home = home;

            // Catch get the FarmAnimal empty constructor in a patch so need to abues this for now.
            Debug.WriteLine($"ENTERING IN FROM FarmAnimalPatch_reload");
            Helpers.GameSave.OverwriteFarmAnimal(ref __instance, null);

            return false;
        }
    }
}
