using Harmony;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Events;
using System.Collections.Generic;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Patches.AnimalHouse
{
    [HarmonyPatch(typeof(StardewValley.AnimalHouse), "addNewHatchedAnimal")]
    class AddNewHatchedAnimal
    {
        public static bool Prefix(ref StardewValley.AnimalHouse __instance, ref string name)
        {
            StardewValley.Farmer player = PariteeCore.Api.Game.GetPlayer();

            if (PariteeCore.Api.AnimalHouse.GetBuilding(__instance) is StardewValley.Buildings.Coop coop)
            {
                AddNewHatchedAnimal.HandleHatchling(ref __instance, name, player);
            }
            else if (PariteeCore.Api.Event.IsFarmEventOccurring<QuestionEvent>(out QuestionEvent questionEvent))
            {
                AddNewHatchedAnimal.HandleNewborn(ref __instance, name, ref questionEvent, player);
            }

            GameLocation currentLocation = PariteeCore.Api.Game.GetCurrentLocation();

            PariteeCore.Api.Event.GoToNextEventCommandInLocation(currentLocation);
            PariteeCore.Api.Game.ExitActiveMenu();

            // Everything in this function was handled here
            return false;
        }

        private static void HandleHatchling(ref StardewValley.AnimalHouse __instance, string name, StardewValley.Farmer player)
        {
            StardewValley.Object incubator = PariteeCore.Api.AnimalHouse.GetIncubator(__instance);

            if (incubator == null)
            {
                // Can't do anything about it
                return;
            }

            // Grab the types with their associated categories in string form
            Dictionary<string, List<string>> restrictions = Helpers.Mod.ReadConfig<ModConfig>().GroupTypesByCategory()
                .ToDictionary(kvp => kvp.Key, kvp => PariteeCore.Api.FarmAnimal.SanitizeBlueChickens(kvp.Value, player));

            // Return a matched type or user default coop dweller
            string type = PariteeCore.Api.AnimalHouse.GetRandomTypeFromIncubator(incubator, restrictions);

            Building building = PariteeCore.Api.AnimalHouse.GetBuilding(__instance);
            StardewValley.FarmAnimal animal = PariteeCore.Api.Farmer.CreateFarmAnimal(player, type, name, building);

            PariteeCore.Api.FarmAnimal.AddToBuilding(animal, building);
            PariteeCore.Api.AnimalHouse.ResetIncubator( __instance, incubator);
        }

        private static void HandleNewborn(ref StardewValley.AnimalHouse animalHouse, string name, ref QuestionEvent questionEvent, StardewValley.Farmer player)
        {
            // Check the config
            ModConfig config = Helpers.Mod.ReadConfig<ModConfig>();

            // Grab the types with their associated categories in string form
            Dictionary<string, List<string>> restrictions = Helpers.Mod.ReadConfig<ModConfig>().GroupTypesByCategory()
                .ToDictionary(kvp => kvp.Key, kvp => PariteeCore.Api.FarmAnimal.SanitizeBlueChickens(kvp.Value, player));

            // Return a matched type or user default barn dweller
            string type = PariteeCore.Api.FarmAnimal.GetRandomTypeFromProduce(questionEvent.animal, restrictions);
            Building building = animalHouse.getBuilding();
            StardewValley.FarmAnimal animal = PariteeCore.Api.Farmer.CreateFarmAnimal(player, type, name, building);

            PariteeCore.Api.FarmAnimal.AssociateParent(animal, questionEvent.animal);
            PariteeCore.Api.FarmAnimal.AddToBuilding(animal, building);

            PariteeCore.Api.Event.ForceQuestionEventToProceed(questionEvent);
        }
    }
}
