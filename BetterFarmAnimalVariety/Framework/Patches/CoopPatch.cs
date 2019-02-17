using Harmony;
using StardewValley;
using StardewValley.Buildings;
using System.Collections.Generic;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    //[HarmonyPatch(typeof(Coop))]
    //[HarmonyPatch("dayUpdate")]
    class CoopPatch
    {
        public static bool Prefix(ref Coop __instance, ref int dayOfMonth)
        {
            AnimalHouse animalHouse = Api.AnimalHouse.GetIndoors(__instance);

            if (!Api.AnimalHouse.IsEggReadyToHatch(animalHouse))
            {
                // Will fail same checks which is what we want
                return true;
            }

            // Check the config
            ModConfig config = Helpers.Config.Load();

            // Grab the types with their associated categories in string form
            Dictionary<string, List<string>> restrictions = config.GroupTypesByCategory();

            // Search for a type by the produce
            string type = Api.FarmAnimal.GetRandomTypeFromProduce(animalHouse.incubatingEgg.Y, restrictions, config.RandomizeHatchlingFromCategory) ?? Api.FarmAnimal.GetDefaultCoopDwellerType();

            ////
            // Everything below is a rewrite of the original as it exists
            ////

            FarmAnimal animal = Api.FarmAnimal.CreateFarmAnimal(type, __instance.owner.Value);

            Api.AnimalHouse.ResetIncubator(animalHouse);

            animalHouse.map.GetLayer("Front").Tiles[1, 2].TileIndex = 45; // TODO: check what this does - egg in incubator graphic?
            animalHouse.animals.Add(animal.myID.Value, animal); // TODO: check if this needs to be done...

            // Always want to continue because setting the X and Y values will 
            // guarantee that it won't trigger the hatch again
            return true;
        }
    }
}
