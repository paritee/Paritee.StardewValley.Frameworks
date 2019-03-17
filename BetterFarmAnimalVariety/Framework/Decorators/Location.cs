using StardewValley;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Decorators
{
    class Location : Decorator
    {
        public Location(StardewValley.GameLocation original) : base(original) { }

        public StardewValley.GameLocation GetOriginal()
        {
            return base.GetOriginal<StardewValley.GameLocation>();
        }

        public bool IsOutdoors()
        {
            return PariteeCore.Locations.Location.IsOutdoors(this.GetOriginal());
        }

        public bool AnyFarmers()
        {
            return PariteeCore.Locations.Location.AnyFarmers(this.GetOriginal());
        }

        public void RemoveAnimal(StardewValley.FarmAnimal animal)
        {
            if (!this.GetOriginal().IsFarm)
            {
                return;
            }

            PariteeCore.Locations.Location.RemoveAnimal(this.GetOriginal() as Farm, animal);
        }
    }
}
