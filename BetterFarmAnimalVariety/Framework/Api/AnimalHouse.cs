using Netcode;
using StardewValley;
using StardewValley.Buildings;
using System.Collections.Generic;

namespace BetterFarmAnimalVariety.Framework.Api
{
    class AnimalHouse
    {
        public static string FormatSize(string buildingName, Constants.AnimalHouse.Size size)
        {
            if (size.Equals(Constants.AnimalHouse.Size.Small))
            {
                return buildingName;
            }

            return $"{size.ToString()} {buildingName}";
        }

        public static StardewValley.Object GetIncubator(StardewValley.AnimalHouse animalHouse)
        {
            foreach (StardewValley.Object @object in animalHouse.objects.Values)
            {
                if (@object.bigCraftable.Value && @object.Name.Contains(Constants.AnimalHouse.Incubator) && (@object.heldObject.Value != null && @object.MinutesUntilReady <= 0) && !animalHouse.isFull())
                {
                    return @object;
                }
            }

            return null;
        }

        public static string GetRandomTypeFromIncubator(StardewValley.Object incubator, Dictionary<string, List<string>> restrictions)
        {
            // Search for a type by the produce
            return incubator.heldObject.Value == null
                ? null
                : Api.FarmAnimal.GetRandomTypeFromProduce(incubator.heldObject.Value.ParentSheetIndex, restrictions);
        }

        public static void ResetIncubator(StardewValley.AnimalHouse animalHouse)
        {
            animalHouse.incubatingEgg.X = 0;
            animalHouse.incubatingEgg.Y = -1;
        }

        public static void ResetIncubator(StardewValley.AnimalHouse animalHouse, StardewValley.Object incubator)
        {
            incubator.heldObject.Value = (StardewValley.Object)null;
            incubator.ParentSheetIndex = Constants.AnimalHouse.DefaultIncubatorItem;

            Api.AnimalHouse.ResetIncubator(animalHouse);
        }

        public static StardewValley.AnimalHouse GetIndoors(Building building)
        {
            NetRef<GameLocation> indoors = Helpers.Reflection.GetFieldValue<NetRef<GameLocation>>(building, "indoors");
            return indoors.Value as StardewValley.AnimalHouse;
        }

        public static bool IsFull(StardewValley.AnimalHouse animalHouse)
        {
            return animalHouse.isFull();
        }

        public static bool IsFull(Building building)
        {
            return Api.AnimalHouse.IsFull(Api.AnimalHouse.GetIndoors(building));
        }

        public static bool IsEggReadyToHatch(StardewValley.AnimalHouse animalHouse)
        {
            return animalHouse.incubatingEgg.Y > 0 || (animalHouse.incubatingEgg.X - 1) <= 0;
        }

        public static void AddAnimal(ref Building building, StardewValley.FarmAnimal animal)
        {
            StardewValley.AnimalHouse animalHouse = Api.AnimalHouse.GetIndoors(building);

            Api.AnimalHouse.AddAnimal(ref animalHouse, animal);

        }

        public static void AddAnimal(ref StardewValley.AnimalHouse animalHouse, StardewValley.FarmAnimal animal)
        {
            animalHouse.animals.Add(animal.myID.Value, animal);
            animalHouse.animalsThatLiveHere.Add(animal.myID.Value);
        }
    }
}
