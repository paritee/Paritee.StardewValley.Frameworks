using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Constants
{
    public class FarmAnimalType : PropertyConstant
    {
        public static FarmAnimalType WhiteChicken { get { return new FarmAnimalType("White Chicken"); } }
        public static FarmAnimalType BrownChicken { get { return new FarmAnimalType("Brown Chicken"); } }
        public static FarmAnimalType BlueChicken { get { return new FarmAnimalType("Blue Chicken"); } }
        public static FarmAnimalType VoidChicken { get { return new FarmAnimalType("Void Chicken"); } }
        public static FarmAnimalType WhiteCow { get { return new FarmAnimalType("White Cow"); } }
        public static FarmAnimalType BrownCow { get { return new FarmAnimalType("Brown Cow"); } }
        public static FarmAnimalType Goat { get { return new FarmAnimalType("Goat"); } }
        public static FarmAnimalType Duck { get { return new FarmAnimalType("Duck"); } }
        public static FarmAnimalType Sheep { get { return new FarmAnimalType("Sheep"); } }
        public static FarmAnimalType Rabbit { get { return new FarmAnimalType("Rabbit"); } }
        public static FarmAnimalType Pig { get { return new FarmAnimalType("Pig"); } }
        public static FarmAnimalType Dinosaur { get { return new FarmAnimalType("Dinosaur"); } }

        private FarmAnimalType(string name) : base(name) { }

        public static bool Exists(string str)
        {
            return PropertyConstant.Exists<FarmAnimalType>(str);
        }

        public static List<string> All()
        {
            List<FarmAnimalType> all = PropertyConstant.All<FarmAnimalType>();

            return all.Select(o => o.ToString()).ToList();
        }
    }
}
