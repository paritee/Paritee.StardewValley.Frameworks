using Harmony;
using StardewValley.Events;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    [HarmonyPatch(typeof(SoundInTheNightEvent))]
    [HarmonyPatch("makeChangesToLocation")]
    class SoundInTheNightEventPatch : Patch
    {
        private const int AnimalWasEatenBehavior = 2;

        public static void Postfix(ref SoundInTheNightEvent __instance)
        {
            int behavior = Helpers.Utilities.GetFieldValue<int>(__instance, "behavior");

            if (!behavior.Equals(SoundInTheNightEventPatch.AnimalWasEatenBehavior))
            {
                return;
            }

            Patch.CleanSaveData();
        }
    }
}
