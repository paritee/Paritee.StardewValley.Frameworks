using StardewValley;
using System.Collections.Generic;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Decorators
{
    public class Farmer : Decorator
    {
        public Farmer(StardewValley.Farmer original) : base(original) { }

        public StardewValley.Farmer GetOriginal()
        {
            return base.GetOriginal<StardewValley.Farmer>();
        }

        public long GetUniqueId()
        {
            return PariteeCore.Characters.Farmer.GetUniqueId(this.GetOriginal());
        }

        public List<string> SanitizeBlueChickens(List<string> types)
        {
            return PariteeCore.Characters.FarmAnimal.SanitizeBlueChickens(types, this.GetOriginal());
        }

        public List<string> SanitizeAffordableTypes(List<string> types)
        {
            return PariteeCore.Characters.FarmAnimal.SanitizeAffordableTypes(types, this.GetOriginal());
        }

        public StardewValley.FarmAnimal CreateFarmAnimal(string type, string name = null, StardewValley.Buildings.Building building = null)
        {
            return PariteeCore.Characters.FarmAnimal.CreateFarmAnimal(type, this.GetUniqueId(), name, building);
        }

        public bool IsCurrentLocation(GameLocation location)
        {
            return PariteeCore.Characters.Farmer.IsCurrentLocation(this.GetOriginal(), location);
        }

        public bool CanAfford(int amount)
        {
            return PariteeCore.Characters.Farmer.CanAfford(this.GetOriginal(), amount);
        }

        public void SpendMoney(int amount)
        {
            PariteeCore.Characters.Farmer.SpendMoney(this.GetOriginal(), amount);
        }

        public bool HasProfession(PariteeCore.Characters.Farmer.Profession profession)
        {
            return PariteeCore.Characters.Farmer.HasProfession(this.GetOriginal(), profession);
        }
    }
}
