using Netcode;
using StardewValley;
using StardewValley.Buildings;

namespace BetterFarmAnimalVariety.Framework.Api
{
    class AnimalHouse
    {
        public static StardewValley.Object GetIncubator(StardewValley.AnimalHouse animalHouse)
        {
            foreach (StardewValley.Object @object in animalHouse.objects.Values)
            {
                if (@object.bigCraftable.Value && @object.Name.Contains(Helpers.Constants.Incubator) && (@object.heldObject.Value != null && @object.MinutesUntilReady <= 0) && !animalHouse.isFull())
                {
                    return @object;
                }
            }

            return null;
        }

        public static string GetRandomTypeFromIncubator(StardewValley.Object incubator)
        {
            string type = null;

            // Search for a type by the produce
            if (incubator.heldObject.Value != null)
            {
                type = Api.FarmAnimal.GetRandomTypeFromProduce(incubator.heldObject.Value.ParentSheetIndex);
            }

            return type ?? Api.FarmAnimal.GetDefaultCoopDwellerType();
        }

        public static void ResetIncubator(StardewValley.AnimalHouse animalHouse)
        {
            animalHouse.incubatingEgg.X = 0;
            animalHouse.incubatingEgg.Y = -1;
        }

        public static void ResetIncubator(StardewValley.Object incubator, StardewValley.AnimalHouse animalHouse)
        {
            incubator.heldObject.Value = (StardewValley.Object)null;
            incubator.ParentSheetIndex = Helpers.Constants.DefaultIncubatorItem;

            Api.AnimalHouse.ResetIncubator(animalHouse);
        }

        public static StardewValley.AnimalHouse GetIndoors(Building building)
        {
            NetRef<GameLocation> indoors = Helpers.Reflection.GetFieldValue<NetRef<GameLocation>>(building, "indoors");
            return indoors.Value as StardewValley.AnimalHouse;
        }

        public static bool IsEggReadyToHatch(StardewValley.AnimalHouse animalHouse)
        {
            return animalHouse.incubatingEgg.Y > 0 || (animalHouse.incubatingEgg.X - 1) <= 0;
        }
    }
}
