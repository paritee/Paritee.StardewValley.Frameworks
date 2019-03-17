using Harmony;
using System.Collections.Generic;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Patches.Utility
{
    [HarmonyPatch(typeof(StardewValley.Utility), "getPurchaseAnimalStock")]
    class GetPurchaseAnimalStock : Patch
    {
        public static bool Prefix(ref List<StardewValley.Object> __result)
        {
            // Grab the farm animals for purchase
            __result = Helpers.FarmAnimals.GetPurchaseAnimalStock(PariteeCore.Utilities.Game.GetFarm());

            return false;
        }
    }
}
