using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Constants
{
    public class FarmAnimalCategory : PropertyConstant
    {
        public static FarmAnimalCategory DairyCow
        {
            get
            {
                FarmAnimalType[] types = new FarmAnimalType[]
                {
                    FarmAnimalType.WhiteCow,
                    FarmAnimalType.BrownCow
                };

                string[] buildings = new string[]
                {
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Barn, Constants.AnimalHouse.Size.Small),
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Barn, Constants.AnimalHouse.Size.Big),
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Barn, Constants.AnimalHouse.Size.Deluxe),
                };

                return new FarmAnimalCategory("Dairy Cow", 1, "5927", "11343", 1500, types, buildings);
            }
        }

        public static FarmAnimalCategory Chicken
        {
            get
            {
                FarmAnimalType[] types = new FarmAnimalType[]
                {
                    FarmAnimalType.WhiteChicken,
                    FarmAnimalType.BrownChicken,
                    FarmAnimalType.BlueChicken,
                    FarmAnimalType.VoidChicken
                };

                FarmAnimalType[] excludeFromShop = new FarmAnimalType[]
                {
                    FarmAnimalType.VoidChicken
                };

                string[] buildings = new string[]
                {
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Coop, Constants.AnimalHouse.Size.Small),
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Coop, Constants.AnimalHouse.Size.Big),
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Coop, Constants.AnimalHouse.Size.Deluxe),
                };

                return new FarmAnimalCategory("Chicken", 0, "5922", "11334", 800, types, buildings, excludeFromShop);
            }
        }

        public static FarmAnimalCategory Sheep
        {
            get
            {
                FarmAnimalType[] types = new FarmAnimalType[]
                {
                    FarmAnimalType.Sheep
                };

                string[] buildings = new string[]
                {
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Barn, Constants.AnimalHouse.Size.Deluxe),
                };

                return new FarmAnimalCategory("Sheep", 4, "5942", "11352", 8000, types, buildings);
            }
        }

        public static FarmAnimalCategory Goat
        {
            get
            {
                FarmAnimalType[] types = new FarmAnimalType[]
                {
                    FarmAnimalType.Goat
                };

                string[] buildings = new string[]
                {
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Barn, Constants.AnimalHouse.Size.Big),
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Barn, Constants.AnimalHouse.Size.Deluxe),
                };

                return new FarmAnimalCategory("Goat", 2, "5933", "11349", 4000, types, buildings);
            }
        }

        public static FarmAnimalCategory Pig
        {
            get
            {
                FarmAnimalType[] types = new FarmAnimalType[]
                {
                    FarmAnimalType.Pig
                };

                string[] buildings = new string[]
                {
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Barn, Constants.AnimalHouse.Size.Deluxe),
                };

                return new FarmAnimalCategory("Pig", 6, "5948", "11346", 16000, types, buildings);
            }
        }

        public static FarmAnimalCategory Duck
        {
            get
            {
                FarmAnimalType[] types = new FarmAnimalType[]
                {
                    FarmAnimalType.Duck
                };

                string[] buildings = new string[]
                {
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Coop, Constants.AnimalHouse.Size.Big),
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Coop, Constants.AnimalHouse.Size.Deluxe),
                };

                return new FarmAnimalCategory("Duck", 3, "5937", "11337", 4000, types, buildings);
            }
        }

        public static FarmAnimalCategory Rabbit
        {
            get
            {
                FarmAnimalType[] types = new FarmAnimalType[]
                {
                    FarmAnimalType.Rabbit
                };

                string[] buildings = new string[]
                {
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Coop, Constants.AnimalHouse.Size.Deluxe),
                };

                return new FarmAnimalCategory("Rabbit", 5, "5945", "11340", 8000, types, buildings);
            }
        }

        public static FarmAnimalCategory Dinosaur
        {
            get
            {
                FarmAnimalType[] types = new FarmAnimalType[]
                {
                    FarmAnimalType.Dinosaur
                };

                string[] buildings = new string[]
                {
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Coop, Constants.AnimalHouse.Size.Big),
                    Api.AnimalHouse.FormatSize(Constants.AnimalHouse.Coop, Constants.AnimalHouse.Size.Deluxe),
                };

                return new FarmAnimalCategory("Dinosaur", 7, types, buildings);
            }
        }

        private readonly int Order;
        public readonly FarmAnimalType[] Types;
        public readonly FarmAnimalType[] ExcludeFromShop;
        public readonly string[] Buildings;
        public readonly string DisplayName;
        public readonly string Description;
        public readonly int Price;

        private FarmAnimalCategory(string name, int order, FarmAnimalType[] types, string[] buildings) : base(name)
        {
            // Cannot be purchased
            this.Order = order;
            this.Types = types;
            this.Buildings = buildings;
        }

        private FarmAnimalCategory(string name, int order, string displayNameStringId, string descriptionStringId, int price, FarmAnimalType[] types, string[] buildings, FarmAnimalType[] excludeFromShop = null) : base(name)
        {
            this.Order = order;
            this.DisplayName = Api.Content.LoadString($"Strings\\StringsFromCSFiles:Utility.cs.{displayNameStringId}");
            this.Description = Api.Content.LoadString($"Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.{descriptionStringId}");
            this.Price = price;
            this.Types = types;
            this.ExcludeFromShop = excludeFromShop ?? new FarmAnimalType[0];
            this.Buildings = buildings;
        }

        public static bool Exists(string str)
        {
            return PropertyConstant.Exists<FarmAnimalCategory>(str);
        }

        public static List<FarmAnimalCategory> All()
        {
            List<FarmAnimalCategory> all = PropertyConstant.All<FarmAnimalCategory>();

            return all.OrderBy(o => o.Order).ToList();
        }

        public bool CanBePurchased()
        {
            return this.Price != default(int);
        }
    }
}
