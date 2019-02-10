using System.Reflection;

namespace BetterFarmAnimalVariety.Framework.Helpers
{
    class VanillaFarmAnimal
    {
        public static VanillaFarmAnimal WhiteChicken { get { return new VanillaFarmAnimal("White Chicken"); } }
        public static VanillaFarmAnimal BrownChicken { get { return new VanillaFarmAnimal("Brown Chicken"); } }
        public static VanillaFarmAnimal BlueChicken { get { return new VanillaFarmAnimal("Blue Chicken"); } }
        public static VanillaFarmAnimal VoidChicken { get { return new VanillaFarmAnimal("Void Chicken"); } }
        public static VanillaFarmAnimal WhiteCow { get { return new VanillaFarmAnimal("White Cow"); } }
        public static VanillaFarmAnimal BrownCow { get { return new VanillaFarmAnimal("Brown Cow"); } }
        public static VanillaFarmAnimal Goat { get { return new VanillaFarmAnimal("Goat"); } }
        public static VanillaFarmAnimal Duck { get { return new VanillaFarmAnimal("Duck"); } }
        public static VanillaFarmAnimal Sheep { get { return new VanillaFarmAnimal("Sheep"); } }
        public static VanillaFarmAnimal Rabbit { get { return new VanillaFarmAnimal("Rabbit"); } }
        public static VanillaFarmAnimal Pig { get { return new VanillaFarmAnimal("Pig"); } }

        public string Type { get; set; }

        private VanillaFarmAnimal(string type)
        {
            this.Type = type;
        }

        public override string ToString()
        {
            return this.Type;
        }

        private static string Parse(string str)
        {
            return str.Replace(" ", "");
        }

        public static bool Exists(string str)
        {
            return Helpers.Reflection.GetProperty(typeof(VanillaFarmAnimal), Helpers.VanillaFarmAnimal.Parse(str), BindingFlags.Static | BindingFlags.Public) != null;
        }
    }
}
