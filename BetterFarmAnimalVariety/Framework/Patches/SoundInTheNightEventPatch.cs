using BetterFarmAnimalVariety.Framework.Data;
using StardewValley.Events;
using System.Reflection;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    class SoundInTheNightEventPatch
    {
        private const int ANIMAL_EATEN_BEHAVIOUR = 2;

        // StardewValley.Events.SoundInTheNightEvent.makeChangesToLocation
        public static void MakeChangesToLocationPostfix(ref SoundInTheNightEvent __instance)
        {
            int behavior = Helpers.Utilities.GetReflectedValue<int>(__instance, "behavior");

            if (!behavior.Equals(SoundInTheNightEventPatch.ANIMAL_EATEN_BEHAVIOUR))
            {
                return;
            }

            FarmAnimalsSaveData saveData = FarmAnimalsSaveData.Deserialize();

            saveData.CleanTypeHistory();
        }
    }
}
