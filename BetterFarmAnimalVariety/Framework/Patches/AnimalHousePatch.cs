using StardewValley;
using StardewValley.Buildings;
using StardewValley.Events;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    class AnimalHousePatch
    {
        // StardewValley.Buildings.AnimalHouse.addNewHatchedAnimal
        public static bool AddNewHatchedAnimalPrefix(ref AnimalHouse __instance, ref string name)
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
            StardewValley.Object incubator = Helpers.Utilities.GetIncubator(animalHouse);

            if (incubator == null)
            {
                // Can't do anything about it
                return;
            }

            // Search for a type by the produce
            string type = incubator.heldObject.Value == null
                ? Helpers.Utilities.GetDefaultCoopDwellerType()
                : Helpers.Utilities.GetRandomTypeFromProduce(incubator.heldObject.Value.ParentSheetIndex);

            Building building = animalHouse.getBuilding();

            FarmAnimal animal = Helpers.Utilities.CreateFarmAnimal(type, Game1.player.UniqueMultiplayerID, name, building);

            Helpers.Utilities.AddFarmAnimalToBuilding(ref animal, ref building);
            Helpers.Utilities.ResetIncubator(incubator, animalHouse);
        }

        private static void HandleNewborn(ref AnimalHouse animalHouse, string name, ref QuestionEvent questionEvent)
        {
            Multiplayer multiplayer = Helpers.Utilities.Multiplayer();
            Building building = animalHouse.getBuilding();

            // TODO: randomize on bfav categories
            string type = Helpers.Utilities.GetRandomTypeFromParent(questionEvent.animal);

            FarmAnimal animal = new FarmAnimal(type, multiplayer.getNewID(), Game1.player.UniqueMultiplayerID)
            {
                Name = name,
                displayName = name,
                home = building
            };

            animal.parentId.Value = questionEvent.animal.myID.Value;

            Helpers.Utilities.AddFarmAnimalToBuilding(ref animal, ref building);

            questionEvent.forceProceed = true;
        }
    }
}
