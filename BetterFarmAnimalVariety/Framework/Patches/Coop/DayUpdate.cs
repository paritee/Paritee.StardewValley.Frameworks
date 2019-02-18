using Harmony;
using StardewValley;
using System.Collections.Generic;

namespace BetterFarmAnimalVariety.Framework.Patches.Coop
{
    //[HarmonyPatch(typeof(Coop))]
    //[HarmonyPatch("dayUpdate")]
    class DayUpdate
    {
        public static bool Prefix(ref StardewValley.Buildings.Coop __instance, ref int dayOfMonth)
        {
            StardewValley.AnimalHouse animalHouse = Api.AnimalHouse.GetIndoors(__instance);

            if (!Api.AnimalHouse.IsEggReadyToHatch(animalHouse))
            {
                // Will fail same checks which is what we want
                return true;
            }

            FarmAnimal animal = DayUpdate.GetRandomAnimal(animalHouse, __instance.owner.Value);

            DayUpdate.PrepareForAnimal(animalHouse, animal);

            // Always want to continue because setting the X and Y values will 
            // guarantee that it won't trigger the hatch again
            return true;
        }

        private static string GetRandomType(StardewValley.AnimalHouse animalHouse)
        {
            // Check the config
            ModConfig config = Helpers.Config.Load<ModConfig>();

            // Grab the types with their associated categories in string form
            Dictionary<string, List<string>> restrictions = config.GroupTypesByCategory();

            // Search for a type by the produce
            return Api.FarmAnimal.GetRandomTypeFromProduce(animalHouse.incubatingEgg.Y, restrictions, config.RandomizeHatchlingFromCategory) 
                ?? Api.FarmAnimal.GetDefaultCoopDwellerType();
        }

        private static FarmAnimal GetRandomAnimal(StardewValley.AnimalHouse animalHouse, long ownerId)
        {
            string type = DayUpdate.GetRandomType(animalHouse);

            return Api.FarmAnimal.CreateFarmAnimal(type, ownerId);
        }

        private static void PrepareForAnimal(StardewValley.AnimalHouse animalHouse, FarmAnimal animal)
        {
            ////
            // Everything below is a rewrite of the original as it exists
            ////
            ///
            Api.AnimalHouse.ResetIncubator(animalHouse);

            animalHouse.map.GetLayer("Front").Tiles[1, 2].TileIndex = 45; // TODO: check what this does - egg in incubator graphic?
            animalHouse.animals.Add(animal.myID.Value, animal); // TODO: check if this needs to be done...
        }
    }
}
