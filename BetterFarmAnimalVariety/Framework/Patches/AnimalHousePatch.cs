using Harmony;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Events;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    [HarmonyPatch(typeof(AnimalHouse))]
    [HarmonyPatch("addNewHatchedAnimal")]
    class AnimalHousePatch
    {
        public static bool Prefix(ref AnimalHouse __instance, ref string name)
        {
            if (__instance.getBuilding() is Coop coop)
            {
                AnimalHousePatch.HandleHatchling(ref __instance, name);
            }
            else if (Game1.farmEvent != null && Game1.farmEvent is QuestionEvent questionEvent)
            {
                AnimalHousePatch.HandleNewborn(ref __instance, name, ref questionEvent);
            }

            ////
            // Everything below is a rewrite of the original as it exists
            ////

            if (Game1.currentLocation.currentEvent != null)
            {
                ++Game1.currentLocation.currentEvent.CurrentCommand;
            }

            Game1.exitActiveMenu();

            // Everything in this function was handled here
            return false;
        }

        private static void HandleHatchling(ref AnimalHouse animalHouse, string name)
        {
            StardewValley.Object incubator = Api.AnimalHouse.GetIncubator(animalHouse);

            if (incubator == null)
            {
                // Can't do anything about it
                return;
            }

            string type = Api.AnimalHouse.GetRandomTypeFromIncubator(incubator);

            Building building = animalHouse.getBuilding();
            FarmAnimal animal = Api.FarmAnimal.CreateFarmAnimal(type, Game1.player.UniqueMultiplayerID, name, building);

            Api.FarmAnimal.AddToBuilding(ref animal, ref building);
            Api.AnimalHouse.ResetIncubator(incubator, animalHouse);
        }

        private static void HandleNewborn(ref AnimalHouse animalHouse, string name, ref QuestionEvent questionEvent)
        {
            string type = Api.FarmAnimal.GetRandomTypeFromParent(questionEvent.animal);
            Building building = animalHouse.getBuilding();
            FarmAnimal animal = Api.FarmAnimal.CreateFarmAnimal(type, Game1.player.UniqueMultiplayerID, name, building);

            Api.FarmAnimal.AssociateParent(ref animal, questionEvent.animal.myID.Value);
            Api.FarmAnimal.AddToBuilding(ref animal, ref building);

            questionEvent.forceProceed = true;
        }
    }
}
