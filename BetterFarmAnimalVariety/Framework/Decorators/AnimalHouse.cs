using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Decorators
{
    class AnimalHouse : Decorator
    {
        public AnimalHouse(StardewValley.AnimalHouse original) : base(original) { }

        public StardewValley.AnimalHouse GetOriginal()
        {
            return base.GetOriginal<StardewValley.AnimalHouse>();
        }

        public StardewValley.Object GetIncubator()
        {
            return PariteeCore.Api.AnimalHouse.GetIncubator(this.GetOriginal());
        }

        public void ResetIncubator()
        {
            PariteeCore.Api.AnimalHouse.ResetIncubator(this.GetOriginal());
        }

        public void ResetIncubator(StardewValley.Object incubator)
        {
            PariteeCore.Api.AnimalHouse.ResetIncubator(this.GetOriginal(), incubator);
        }

        public bool IsEggReadyToHatch()
        {
            return PariteeCore.Api.AnimalHouse.IsEggReadyToHatch(this.GetOriginal());
        }

        public bool IsFull()
        {
            return PariteeCore.Api.AnimalHouse.IsFull(this.GetOriginal());
        }

        public StardewValley.Buildings.Building GetBuilding()
        {
            return PariteeCore.Api.AnimalHouse.GetBuilding(this.GetOriginal());
        }
    }
}
