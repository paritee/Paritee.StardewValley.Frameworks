using Netcode;
using StardewValley;
using StardewValley.Buildings;
using System.Reflection;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    class CoopPatch
    {
        // StardewValley.Buildings.Coop.dayUpdate
        public static bool DayUpdatePrefix(ref Coop __instance, ref int dayOfMonth)
        {

            NetRef<GameLocation> indoors = Helpers.Utilities.GetReflectedValue<NetRef<GameLocation>>(__instance, "indoors");
            AnimalHouse animalHouse = indoors.Value as AnimalHouse;

            // Check if there's something ready to hatch today
            if (animalHouse.incubatingEgg.Y <= 0 || (animalHouse.incubatingEgg.X - 1) > 0)
            {
                // Will fail same checks which is what we want
                return true;
            }

            // Search for a type by the produce
            string type = Helpers.Utilities.GetRandomTypeFromProduce(animalHouse.incubatingEgg.Y);

            ////
            // Everything below is a rewrite of the original as it exists
            ////

            FarmAnimal animal = Helpers.Utilities.CreateFarmAnimal(type, __instance.owner.Value);

            Helpers.Utilities.ResetIncubator(animalHouse);

            animalHouse.map.GetLayer("Front").Tiles[1, 2].TileIndex = 45;
            animalHouse.animals.Add(animal.myID.Value, animal);

            // Always want to continue because setting the X and Y values will 
            // guarantee that it won't trigger the hatch again
            return true;
        }
    }
}
