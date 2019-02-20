using BetterFarmAnimalVariety.Framework.Models;
using Harmony;
using StardewValley;
using StardewValley.Buildings;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    [HarmonyPatch(typeof(FarmAnimal), "reload")]
    class Reload : Patch
    {
        public static bool Prefix(ref FarmAnimal __instance, ref Building home)
        {
            if (__instance.Name == null)
            {
                return true;
            }

            PariteeCore.Api.FarmAnimal.SetHome(ref __instance, home);

            // Can't get the FarmAnimal empty constructor in a patch so need to 
            // use the reload function to handle it
            (new FarmAnimalsSaveData(Constants.Mod.Key)).Read().OverwriteFarmAnimal(ref __instance, null);

            return false;
        }
    }
}
