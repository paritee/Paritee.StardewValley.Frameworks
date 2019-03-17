using System.Collections.Generic;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Decorators
{
    class Incubator : Decorator
    {
        public Incubator(StardewValley.Object original) : base(original) { }

        public StardewValley.Object GetOriginal()
        {
            return base.GetOriginal<StardewValley.Object>();
        }

        public string GetRandomType(Dictionary<string, List<string>> restrictions)
        {
            return PariteeCore.Locations.AnimalHouse.GetRandomTypeFromIncubator(this.GetOriginal(), restrictions)
                ?? PariteeCore.Characters.FarmAnimal.GetDefaultCoopDwellerType();
        }
    }
}
