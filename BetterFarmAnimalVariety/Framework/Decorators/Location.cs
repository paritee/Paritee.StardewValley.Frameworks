using Microsoft.Xna.Framework;
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
            return PariteeCore.Api.Location.IsOutdoors(this.GetOriginal());
        }

        public bool AnyFarmers()
        {
            return PariteeCore.Api.Location.AnyFarmers(this.GetOriginal());
        }

        public void RemoveAnimal(StardewValley.FarmAnimal animal)
        {
            if (!this.GetOriginal().IsFarm)
            {
                return;
            }

            PariteeCore.Api.Location.RemoveAnimal(this.GetOriginal() as Farm, animal);
        }

        public void SpawnObject(Vector2 tileLocation, StardewValley.Object obj)
        {
            PariteeCore.Api.Location.SpawnObject(this.GetOriginal(), tileLocation, obj);
        }
    }
}
