using BetterFarmAnimalVariety.Framework.Models;
using Harmony;
using StardewValley.Buildings;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Patches.FarmAnimal
{
    [HarmonyPatch(typeof(StardewValley.FarmAnimal), "reload")]
    class Reload : Patch
    {
        public static bool Prefix(ref StardewValley.FarmAnimal __instance, ref Building home)
        {
            if (!PariteeCore.Api.FarmAnimal.HasName(__instance))
            {
                return true;
            }

            PariteeCore.Api.FarmAnimal.SetHome(__instance, home);

            // Can't get the FarmAnimal empty constructor in a patch so need to 
            // use the reload function to handle it
            (new FarmAnimalsSaveData(Constants.Mod.Key)).Read().OverwriteFarmAnimal(__instance, null);

            return false;
        }
    }
}
