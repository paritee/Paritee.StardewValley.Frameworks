using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Decorators
{
    class AutoGrabber : Decorator
    {
        public AutoGrabber(StardewValley.Object original) : base(original) { }

        public StardewValley.Object GetOriginal()
        {
            return base.GetOriginal<StardewValley.Object>();
        }

        public void AutoGrabFromAnimals(StardewValley.AnimalHouse animalHouse)
        {
            PariteeCore.Locations.AnimalHouse.AutoGrabFromAnimals(animalHouse, this.GetOriginal());
        }
    }
}
