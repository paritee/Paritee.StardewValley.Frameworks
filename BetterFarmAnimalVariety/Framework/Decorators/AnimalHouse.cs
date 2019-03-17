using System.Collections.Generic;
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
        
        public StardewValley.Object GetIncubatorWithEggReadyToHatch()
        {
            return PariteeCore.Locations.AnimalHouse.GetIncubatorWithEggReadyToHatch(this.GetOriginal());
        }

        public void ResetIncubator()
        {
            PariteeCore.Locations.AnimalHouse.ResetIncubator(this.GetOriginal());
        }

        public void ResetIncubator(StardewValley.Object incubator)
        {
            PariteeCore.Locations.AnimalHouse.ResetIncubator(this.GetOriginal(), incubator);
        }

        public bool IsEggReadyToHatch()
        {
            return PariteeCore.Locations.AnimalHouse.IsEggReadyToHatch(this.GetOriginal());
        }

        public bool IsFull()
        {
            return PariteeCore.Locations.AnimalHouse.IsFull(this.GetOriginal());
        }

        public StardewValley.Buildings.Building GetBuilding()
        {
            return PariteeCore.Locations.AnimalHouse.GetBuilding(this.GetOriginal());
        }

        public void SetIncubatorHatchEvent()
        {
            PariteeCore.Locations.AnimalHouse.SetIncubatorHatchEvent(this.GetOriginal());
        }
    }
}
