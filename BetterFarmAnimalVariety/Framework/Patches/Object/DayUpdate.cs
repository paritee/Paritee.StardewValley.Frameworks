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
            if (!PariteeCore.Api.Object.IsAutoGrabber(__instance))
            {
                return true;
            }

            if (location == null || !(location is StardewValley.AnimalHouse animalHouse))
            {
                return true;
            }

            Decorators.AutoGrabber autoGrabber = new Decorators.AutoGrabber(__instance);

            autoGrabber.AutoGrabFromAnimals(animalHouse);

            // Since AutoGrabFromAnimals sets the animals' current produce to 
            // none ONLY if they're not a "find" type and the vanilla code only 
            // accounts for Truffles, must not continue on
            return false;
        }
    }
}
