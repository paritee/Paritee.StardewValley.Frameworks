using Harmony;
using StardewValley.Events;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    //[HarmonyPatch(typeof(SoundInTheNightEvent))]
    //[HarmonyPatch("makeChangesToLocation")]
    class SoundInTheNightEventPatch : Patch
    {
        public static void Postfix(ref SoundInTheNightEvent __instance)
        {
            int behavior = Helpers.Reflection.GetFieldValue<int>(__instance, "behavior");

            if (!behavior.Equals(Constants.Event.SoundInTheNightAnimalEatenEvent))
            {
                return;
            }

            Patch.CleanSaveData();
        }
    }
}
