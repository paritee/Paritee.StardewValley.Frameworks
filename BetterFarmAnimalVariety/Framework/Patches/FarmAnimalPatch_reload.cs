using BetterFarmAnimalVariety.Framework.Data;
using Harmony;
using StardewValley;
using StardewValley.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    [HarmonyPatch(typeof(FarmAnimal))]
    [HarmonyPatch("reload")]
    class FarmAnimalPatch_reload : Patch
    {
        public static void Postfix(ref FarmAnimal __instance, ref Building home, ref string __result)
        {
            __instance.Sprite = Api.FarmAnimal.CreateSprite(__instance);
        }
    }
}
