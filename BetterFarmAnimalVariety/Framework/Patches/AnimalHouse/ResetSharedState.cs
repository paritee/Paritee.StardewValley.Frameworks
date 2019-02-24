using Harmony;

namespace BetterFarmAnimalVariety.Framework.Patches.AnimalHouse
{
    class ResetSharedState
    {
        [HarmonyPatch(typeof(StardewValley.AnimalHouse), "resetSharedState")]
        public static void Postfix(ref StardewValley.AnimalHouse __instance)
        {
            Decorators.AnimalHouse moddedAnimalHouse = new Decorators.AnimalHouse(__instance);

            moddedAnimalHouse.SetIncubatorHatchEvent();
        }
    }
}
