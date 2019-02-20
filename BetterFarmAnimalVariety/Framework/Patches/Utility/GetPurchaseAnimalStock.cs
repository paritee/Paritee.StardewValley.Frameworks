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
            // Load the config and grab the farm animals for purchase
            __result = Helpers.Mod.ReadConfig<ModConfig>().GetPurchaseAnimalStock(PariteeCore.Api.Game.GetFarm());

            return false;
        }
    }
}
