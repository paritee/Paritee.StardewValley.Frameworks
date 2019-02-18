namespace BetterFarmAnimalVariety.Framework.Models
{
    public class FarmAnimalCategory : PropertyConstant
    {
        protected readonly int Order;
        public readonly string[] Types;
        public readonly string[] ExcludeFromShop;
        public readonly string[] Buildings;
        public readonly string DisplayName;
        public readonly string Description;
        public readonly int Price;

        public FarmAnimalCategory(string name, int order, string[] types, string[] buildings) : base(name)
        {
            // Cannot be purchased
            this.Order = order;
            this.Types = types;
            this.Buildings = buildings;
        }

        public FarmAnimalCategory(string name, int order, string displayName, string description, int price, string[] types, string[] buildings, string[] excludeFromShop = null) : base(name)
        {
            this.Order = order;
            this.DisplayName = displayName;
            this.Description = description;
            this.Price = price;
            this.Types = types;
            this.ExcludeFromShop = excludeFromShop ?? new string[0];
            this.Buildings = buildings;
        }

        public bool CanBePurchased()
        {
            return this.Price != default(int);
        }
    }
}
