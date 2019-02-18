using Harmony;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Events;
using System.Collections.Generic;

namespace BetterFarmAnimalVariety.Framework.Patches.AnimalHouse
{
    //[HarmonyPatch(typeof(AnimalHouse))]
    //[HarmonyPatch("addNewHatchedAnimal")]
    class AddNewHatchedAnimal
    {
        public static bool Prefix(ref StardewValley.AnimalHouse __instance, ref string name)
        {
            if (__instance.getBuilding() is StardewValley.Buildings.Coop coop)
            {
                AddNewHatchedAnimal.HandleHatchling(ref __instance, name);
            }
            else if (StardewValley.Game1.farmEvent != null && StardewValley.Game1.farmEvent is QuestionEvent questionEvent)
            {
                AddNewHatchedAnimal.HandleNewborn(ref __instance, name, ref questionEvent);
            }

            ////
            // Everything below is a rewrite of the original as it exists
            ////

            if (StardewValley.Game1.currentLocation.currentEvent != null)
            {
                ++StardewValley.Game1.currentLocation.currentEvent.CurrentCommand;
            }

            StardewValley.Game1.exitActiveMenu();

            // Everything in this function was handled here
            return false;
        }

        private static void HandleHatchling(ref StardewValley.AnimalHouse animalHouse, string name)
        {
            StardewValley.Object incubator = Api.AnimalHouse.GetIncubator(animalHouse);

            if (incubator == null)
            {
                // Can't do anything about it
                return;
            }

            // Check the config
            ModConfig config = Helpers.Mod.LoadConfig<ModConfig>();

            // Grab the types with their associated categories in string form
            Dictionary<string, List<string>> restrictions = config.GroupTypesByCategory();

            string type = Api.AnimalHouse.GetRandomTypeFromIncubator(incubator, restrictions, config.RandomizeHatchlingFromCategory);

            Building building = animalHouse.getBuilding();
            FarmAnimal animal = Api.FarmAnimal.CreateFarmAnimal(type, StardewValley.Game1.player.UniqueMultiplayerID, name, building);

            Api.FarmAnimal.AddToBuilding(ref animal, ref building);
            Api.AnimalHouse.ResetIncubator(incubator, animalHouse);
        }

        private static void HandleNewborn(ref StardewValley.AnimalHouse animalHouse, string name, ref QuestionEvent questionEvent)
        {
            // Check the config
            ModConfig config = Helpers.Mod.LoadConfig<ModConfig>();

            // Grab the types with their associated categories in string form
            Dictionary<string, List<string>> restrictions = config.GroupTypesByCategory();

            string type = Api.FarmAnimal.GetRandomTypeFromParent(questionEvent.animal, restrictions, config.RandomizeNewbornFromCategory, config.IgnoreParentProduceCheck);

            Building building = animalHouse.getBuilding();
            FarmAnimal animal = Api.FarmAnimal.CreateFarmAnimal(type, StardewValley.Game1.player.UniqueMultiplayerID, name, building);

            Api.FarmAnimal.AssociateParent(ref animal, questionEvent.animal.myID.Value);
            Api.FarmAnimal.AddToBuilding(ref animal, ref building);

            questionEvent.forceProceed = true;
        }
    }
}
