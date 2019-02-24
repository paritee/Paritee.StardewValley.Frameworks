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
            Decorators.AnimalHouse moddedAnimalHouse = new Decorators.AnimalHouse(__instance);
            Decorators.Farmer moddedPlayer = new Decorators.Farmer(PariteeCore.Api.Game.GetPlayer());

            if (moddedAnimalHouse.GetBuilding() is StardewValley.Buildings.Coop coop)
            {
                AddNewHatchedAnimal.HandleHatchling(ref moddedAnimalHouse, name, moddedPlayer);
            }
            else if (PariteeCore.Api.Event.IsFarmEventOccurring<QuestionEvent>(out QuestionEvent questionEvent))
            {
                AddNewHatchedAnimal.HandleNewborn(ref moddedAnimalHouse, name, ref questionEvent, moddedPlayer);
            }

            GameLocation currentLocation = PariteeCore.Api.Game.GetCurrentLocation();

            PariteeCore.Api.Event.GoToNextEventCommandInLocation(currentLocation);
            PariteeCore.Api.Game.ExitActiveMenu();

            // Everything in this function was handled here
            return false;
        }

        private static void HandleHatchling(ref Decorators.AnimalHouse moddedAnimalHouse, string name, Decorators.Farmer moddedPlayer)
        {
            StardewValley.Object incubator = moddedAnimalHouse.GetIncubator();

            if (incubator == null)
            {
                // Can't do anything about it
                return;
            }

            Decorators.Incubator moddedIncubator = new Decorators.Incubator(incubator);

            // Grab the types with their associated categories in string form
            Dictionary<string, List<string>> restrictions = Helpers.Mod.ReadConfig<ModConfig>().GroupTypesByCategory()
                .ToDictionary(kvp => kvp.Key, kvp => moddedPlayer.SanitizeBlueChickens(kvp.Value));

            // Return a matched type or user default coop dweller
            string type = moddedIncubator.GetRandomType(restrictions);

            Building building = moddedAnimalHouse.GetBuilding();
            StardewValley.FarmAnimal animal = moddedPlayer.CreateFarmAnimal(type, name, building);
            Decorators.FarmAnimal moddedAnimal = new Decorators.FarmAnimal(animal);

            moddedAnimal.AddToBuilding(building);
            moddedAnimalHouse.ResetIncubator(incubator);
        }

        private static void HandleNewborn(ref Decorators.AnimalHouse moddedAnimalHouse, string name, ref QuestionEvent questionEvent, Decorators.Farmer moddedPlayer)
        {
            // Check the config
            ModConfig config = Helpers.Mod.ReadConfig<ModConfig>();

            // Grab the types with their associated categories in string form
            Dictionary<string, List<string>> restrictions = Helpers.Mod.ReadConfig<ModConfig>().GroupTypesByCategory()
                .ToDictionary(kvp => kvp.Key, kvp => moddedPlayer.SanitizeBlueChickens(kvp.Value));

            Decorators.FarmAnimal moddedParent = new Decorators.FarmAnimal(questionEvent.animal);

            // Return a matched type or user default barn dweller
            string type = moddedParent.GetRandomTypeFromProduce(restrictions);
            Building building = moddedAnimalHouse.GetBuilding();
            StardewValley.FarmAnimal animal = moddedPlayer.CreateFarmAnimal(type, name, building);
            Decorators.FarmAnimal moddedAnimal = new Decorators.FarmAnimal(animal);

            moddedAnimal.AssociateParent(questionEvent.animal);
            moddedAnimal.AddToBuilding(building);

            PariteeCore.Api.Event.ForceQuestionEventToProceed(questionEvent);
        }
    }
}
