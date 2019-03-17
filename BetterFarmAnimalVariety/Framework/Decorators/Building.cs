using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Decorators
{
    class Building : Decorator
    {
        public Building(StardewValley.Buildings.Building original) : base(original) { }

        public StardewValley.Buildings.Building GetOriginal()
        {
            return base.GetOriginal<StardewValley.Buildings.Building>();
        }

        public StardewValley.AnimalHouse GetIndoors()
        {
            return PariteeCore.Locations.AnimalHouse.GetIndoors(this.GetOriginal());
        }

        public bool IsFull()
        {
            return PariteeCore.Locations.AnimalHouse.IsFull(this.GetOriginal());
        }
    }
}
