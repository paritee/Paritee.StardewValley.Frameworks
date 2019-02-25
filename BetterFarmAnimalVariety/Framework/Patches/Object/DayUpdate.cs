using Harmony;
using StardewValley;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Patches.Object
{
    [HarmonyPatch(typeof(StardewValley.Object), "DayUpdate")]
    class DayUpdate
    {
        public static bool Prefix(ref StardewValley.Object __instance, ref GameLocation location)
        {
            if (PariteeCore.Api.Object.IsAutoGrabber(__instance))
            {
                return true;
            }

            if (location == null || !(location is StardewValley.AnimalHouse animalHouse))
            {
                return true;
            }

            Decorators.AutoGrabber autoGrabber = new Decorators.AutoGrabber(__instance);

            autoGrabber.AutoGrabFromAnimals(animalHouse);

            // Vanilla function returns after the big craftable switch statement always
            return false;
        }
    }
}
