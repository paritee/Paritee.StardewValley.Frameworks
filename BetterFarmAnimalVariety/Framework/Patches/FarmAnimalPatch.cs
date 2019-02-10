using BetterFarmAnimalVariety.Framework.Data;
using Harmony;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    [HarmonyPatch(typeof(FarmAnimal), MethodType.Constructor, new[] { typeof(string), typeof(long), typeof(long) })]
    class FarmAnimalPatch : Patch
    {
        public static void Postfix(ref FarmAnimal __instance, ref string type, ref long id, ref long ownerID) 
        {
            Debug.WriteLine($"ENTERING IN FROM FarmAnimalPatch");
            Helpers.GameSave.OverwriteFarmAnimal(ref __instance, type);
        }
    }
}
