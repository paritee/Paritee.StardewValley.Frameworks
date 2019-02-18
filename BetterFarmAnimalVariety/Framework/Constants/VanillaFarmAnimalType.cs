using BetterFarmAnimalVariety.Framework.Models;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Constants
{
    class VanillaFarmAnimalType : FarmAnimalType
    {
        public static VanillaFarmAnimalType WhiteChicken { get { return new VanillaFarmAnimalType("White Chicken"); } }
        public static VanillaFarmAnimalType BrownChicken { get { return new VanillaFarmAnimalType("Brown Chicken"); } }
        public static VanillaFarmAnimalType BlueChicken { get { return new VanillaFarmAnimalType("Blue Chicken"); } }
        public static VanillaFarmAnimalType VoidChicken { get { return new VanillaFarmAnimalType("Void Chicken"); } }
        public static VanillaFarmAnimalType WhiteCow { get { return new VanillaFarmAnimalType("White Cow"); } }
        public static VanillaFarmAnimalType BrownCow { get { return new VanillaFarmAnimalType("Brown Cow"); } }
        public static VanillaFarmAnimalType Goat { get { return new VanillaFarmAnimalType("Goat"); } }
        public static VanillaFarmAnimalType Duck { get { return new VanillaFarmAnimalType("Duck"); } }
        public static VanillaFarmAnimalType Sheep { get { return new VanillaFarmAnimalType("Sheep"); } }
        public static VanillaFarmAnimalType Rabbit { get { return new VanillaFarmAnimalType("Rabbit"); } }
        public static VanillaFarmAnimalType Pig { get { return new VanillaFarmAnimalType("Pig"); } }
        public static VanillaFarmAnimalType Dinosaur { get { return new VanillaFarmAnimalType("Dinosaur"); } }

        private VanillaFarmAnimalType(string name) : base(name) { }

        public static bool Exists(string str)
        {
            return PropertyConstant.Exists<VanillaFarmAnimalType>(str);
        }

        public static List<string> All()
        {
            List<VanillaFarmAnimalType> all = PropertyConstant.All<VanillaFarmAnimalType>();

            return all.Select(o => o.ToString()).ToList();
        }
    }
}
