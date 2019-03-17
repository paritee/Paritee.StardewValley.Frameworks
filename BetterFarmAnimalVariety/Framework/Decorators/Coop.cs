using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Decorators
{
    class Coop : Decorator
    {
        public Coop(StardewValley.Buildings.Coop original) : base(original) { }

        public StardewValley.Buildings.Coop GetOriginal()
        {
            return base.GetOriginal<StardewValley.Buildings.Coop>();
        }

        public StardewValley.AnimalHouse GetIndoors()
        {
            return PariteeCore.Locations.AnimalHouse.GetIndoors(this.GetOriginal());
        }
    }
}
