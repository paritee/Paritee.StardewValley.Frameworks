using Harmony;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Patches.FarmAnimal
{
    [HarmonyPatch(typeof(StardewValley.FarmAnimal), "dayUpdate")]
    class DayUpdate
    {
        // environtment (sic.)
        public static void Postfix(ref StardewValley.FarmAnimal __instance, ref StardewValley.GameLocation environtment)
        {
            Decorators.FarmAnimal moddedAnimal = new Decorators.FarmAnimal(__instance);

            // Non-producers should never have their current produce set
            // This is to make sure auto-grabbers, etc. do not grab anything
            if (!moddedAnimal.IsAProducer())
            {
                moddedAnimal.SetCurrentProduce(PariteeCore.Constants.FarmAnimal.NoProduce);
            }
        }
    }
}
