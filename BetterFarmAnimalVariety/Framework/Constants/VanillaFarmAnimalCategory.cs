using BetterFarmAnimalVariety.Framework.Models;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Constants
{
    class VanillaFarmAnimalCategory : FarmAnimalCategory
    {
        public static VanillaFarmAnimalCategory DairyCow
        {
            get
            {
                string[] types = new string[]
                {
                    VanillaFarmAnimalType.WhiteCow.ToString(),
                    VanillaFarmAnimalType.BrownCow.ToString()
                };

                string[] buildings = new string[]
                {
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Barn, Constants.AnimalHouse.Size.Small),
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Barn, Constants.AnimalHouse.Size.Big),
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Barn, Constants.AnimalHouse.Size.Deluxe),
                };

                string displayName = VanillaFarmAnimalCategory.LoadDisplayName("5927");
                string description = VanillaFarmAnimalCategory.LoadDescription("11343");

                return new VanillaFarmAnimalCategory("Dairy Cow", 1, displayName, description, 1500, types, buildings);
            }
        }

        public static VanillaFarmAnimalCategory Chicken
        {
            get
            {
                string[] types = new string[]
                {
                    VanillaFarmAnimalType.WhiteChicken.ToString(),
                    VanillaFarmAnimalType.BrownChicken.ToString(),
                    VanillaFarmAnimalType.BlueChicken.ToString(),
                    VanillaFarmAnimalType.VoidChicken.ToString()
                };

                string[] excludeFromShop = new string[]
                {
                    VanillaFarmAnimalType.VoidChicken.ToString()
                };

                string[] buildings = new string[]
                {
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Coop, Constants.AnimalHouse.Size.Small),
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Coop, Constants.AnimalHouse.Size.Big),
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Coop, Constants.AnimalHouse.Size.Deluxe),
                };

                string displayName = VanillaFarmAnimalCategory.LoadDisplayName("5922");
                string description = VanillaFarmAnimalCategory.LoadDescription("11334");

                return new VanillaFarmAnimalCategory("Chicken", 0, displayName, description, 800, types, buildings, excludeFromShop);
            }
        }

        public static VanillaFarmAnimalCategory Sheep
        {
            get
            {
                string[] types = new string[]
                {
                    VanillaFarmAnimalType.Sheep.ToString()
                };

                string[] buildings = new string[]
                {
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Barn, Constants.AnimalHouse.Size.Deluxe),
                };

                string displayName = VanillaFarmAnimalCategory.LoadDisplayName("5942");
                string description = VanillaFarmAnimalCategory.LoadDescription("11352");

                return new VanillaFarmAnimalCategory("Sheep", 4, displayName, description, 8000, types, buildings);
            }
        }

        public static VanillaFarmAnimalCategory Goat
        {
            get
            {
                string[] types = new string[]
                {
                    VanillaFarmAnimalType.Goat.ToString()
                };

                string[] buildings = new string[]
                {
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Barn, Constants.AnimalHouse.Size.Big),
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Barn, Constants.AnimalHouse.Size.Deluxe),
                };

                string displayName = VanillaFarmAnimalCategory.LoadDisplayName("5933");
                string description = VanillaFarmAnimalCategory.LoadDescription("11349");

                return new VanillaFarmAnimalCategory("Goat", 2, displayName, description, 4000, types, buildings);
            }
        }

        public static VanillaFarmAnimalCategory Pig
        {
            get
            {
                string[] types = new string[]
                {
                    VanillaFarmAnimalType.Pig.ToString()
                };

                string[] buildings = new string[]
                {
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Barn, Constants.AnimalHouse.Size.Deluxe),
                };

                string displayName = VanillaFarmAnimalCategory.LoadDisplayName("5948");
                string description = VanillaFarmAnimalCategory.LoadDescription("11346");

                return new VanillaFarmAnimalCategory("Pig", 6, displayName, description, 16000, types, buildings);
            }
        }

        public static VanillaFarmAnimalCategory Duck
        {
            get
            {
                string[] types = new string[]
                {
                    VanillaFarmAnimalType.Duck.ToString()
                };

                string[] buildings = new string[]
                {
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Coop, Constants.AnimalHouse.Size.Big),
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Coop, Constants.AnimalHouse.Size.Deluxe),
                };

                string displayName = VanillaFarmAnimalCategory.LoadDisplayName("5937");
                string description = VanillaFarmAnimalCategory.LoadDescription("11337");

                return new VanillaFarmAnimalCategory("Duck", 3, displayName, description, 4000, types, buildings);
            }
        }

        public static VanillaFarmAnimalCategory Rabbit
        {
            get
            {
                string[] types = new string[]
                {
                    VanillaFarmAnimalType.Rabbit.ToString()
                };

                string[] buildings = new string[]
                {
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Coop, Constants.AnimalHouse.Size.Deluxe),
                };

                string displayName = VanillaFarmAnimalCategory.LoadDisplayName("5945");
                string description = VanillaFarmAnimalCategory.LoadDescription("11340");

                return new VanillaFarmAnimalCategory("Rabbit", 5, displayName, description, 8000, types, buildings);
            }
        }

        public static VanillaFarmAnimalCategory Dinosaur
        {
            get
            {
                string[] types = new string[]
                {
                    VanillaFarmAnimalType.Dinosaur.ToString()
                };

                string[] buildings = new string[]
                {
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Coop, Constants.AnimalHouse.Size.Big),
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Coop, Constants.AnimalHouse.Size.Deluxe),
                };

                return new VanillaFarmAnimalCategory("Dinosaur", 7, types, buildings);
            }
        }

        private VanillaFarmAnimalCategory(string name, int order, string[] types, string[] buildings) 
            : base(name, order, types, buildings) { }

        private VanillaFarmAnimalCategory(string name, int order, string displayName, string description, int price, string[] types, string[] buildings, string[] excludeFromShop = null) 
            : base(name, order, displayName, description, price, types, buildings, excludeFromShop) { }


        public static bool Exists(string str)
        {
            return PropertyConstant.Exists<VanillaFarmAnimalCategory>(str);
        }

        public static List<VanillaFarmAnimalCategory> All()
        {
            List<VanillaFarmAnimalCategory> all = PropertyConstant.All<VanillaFarmAnimalCategory>();

            return all.OrderBy(o => o.Order).ToList();
        }

        private static string LoadDisplayName(string id)
        {
            return Api.Content.LoadString($"Strings\\StringsFromCSFiles:Utility.cs.{id}");
        }

        private static string LoadDescription(string id)
        {
            return Api.Content.LoadString($"Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.{id}");
        }
    }
}
