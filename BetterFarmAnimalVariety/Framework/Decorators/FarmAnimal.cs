using Microsoft.Xna.Framework;
using StardewValley;
using System.Collections.Generic;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Decorators
{
    public class FarmAnimal : Decorator
    {
        public FarmAnimal(StardewValley.FarmAnimal original) : base(original) { }

        public StardewValley.FarmAnimal GetOriginal()
        {
            return base.GetOriginal<StardewValley.FarmAnimal>();
        }

        public void Reload(StardewValley.Buildings.Building building)
        {
            PariteeCore.Characters.FarmAnimal.Reload(this.GetOriginal(), building);
        }

        public string GetTypeString()
        {
            return PariteeCore.Characters.FarmAnimal.GetType(this.GetOriginal());
        }

        public long GetUniqueId()
        {
            return PariteeCore.Characters.FarmAnimal.GetUniqueId(this.GetOriginal());
        }
        
        public bool IsVanilla()
        {
            return PariteeCore.Characters.FarmAnimal.IsVanilla(this.GetOriginal());
        }

        public int RollProduce(int seed, StardewValley.Farmer farmer = null, double deluxeProduceLuck = default(double))
        {
            return PariteeCore.Characters.FarmAnimal.RollProduce(this.GetOriginal(), seed, farmer, deluxeProduceLuck);
        }

        public int GetCurrentProduce()
        {
            return PariteeCore.Characters.FarmAnimal.GetCurrentProduce(this.GetOriginal());
        }

        public int GetPrice()
        {
            return PariteeCore.Characters.FarmAnimal.GetPrice(this.GetOriginal());
        }

        public void SetCurrentProduce(int produceIndex)
        {
            PariteeCore.Characters.FarmAnimal.SetCurrentProduce(this.GetOriginal(), produceIndex);
        }

        public void UpdateFromData(string type)
        {
            PariteeCore.Characters.FarmAnimal.UpdateFromData(this.GetOriginal(), type);
        }

        public string GetDefaultType()
        {
            return PariteeCore.Characters.FarmAnimal.GetDefaultType(this.GetOriginal());
        }

        public void AddToBuilding(StardewValley.Buildings.Building building)
        {
            PariteeCore.Characters.FarmAnimal.AddToBuilding(this.GetOriginal(), building);
        }

        public string GetRandomTypeFromProduce(Dictionary<string, List<string>> restrictions)
        {
            return PariteeCore.Characters.FarmAnimal.GetRandomTypeFromProduce(this.GetOriginal(), restrictions)
                ?? PariteeCore.Characters.FarmAnimal.GetDefaultBarnDwellerType();
        }

        public void AssociateParent(StardewValley.FarmAnimal parent)
        {
            PariteeCore.Characters.FarmAnimal.AssociateParent(this.GetOriginal(), parent);
        }

        public bool HasHome()
        {
            return PariteeCore.Characters.FarmAnimal.HasHome(this.GetOriginal());
        }

        public bool IsEating()
        {
            return PariteeCore.Characters.FarmAnimal.IsEating(this.GetOriginal());
        }

        public byte GetFullness()
        {
            return PariteeCore.Characters.FarmAnimal.GetFullness(this.GetOriginal());
        }

        public byte GetHappiness()
        {
            return PariteeCore.Characters.FarmAnimal.GetHappiness(this.GetOriginal());
        }

        public void SetFindGrassPathController(GameLocation location)
        {
            PariteeCore.Characters.FarmAnimal.SetFindGrassPathController(this.GetOriginal(), location);
        }

        public void ReturnHome()
        {
            PariteeCore.Characters.FarmAnimal.ReturnHome(this.GetOriginal());
        }

        public void SetFindHomeDoorPathController(GameLocation location)
        {
            PariteeCore.Characters.FarmAnimal.SetFindHomeDoorPathController(this.GetOriginal(), location);
        }

        public bool IsBaby()
        {
            return PariteeCore.Characters.FarmAnimal.IsBaby(this.GetOriginal());
        }

        public bool CanFindProduce()
        {
            return PariteeCore.Characters.FarmAnimal.CanFindProduce(this.GetOriginal());
        }

        public void AnimateFindingProduce()
        {
            PariteeCore.Characters.FarmAnimal.AnimateFindingProduce(this.GetOriginal());
        }

        public void FindProduce(StardewValley.Farmer farmer)
        {
            PariteeCore.Characters.FarmAnimal.FindProduce(this.GetOriginal(), farmer);
        }

        public int GetFriendship()
        {
            return PariteeCore.Characters.FarmAnimal.GetFriendship(this.GetOriginal());
        }

        public void SetHome(StardewValley.Buildings.Building building)
        {
            PariteeCore.Characters.FarmAnimal.SetHome(this.GetOriginal(), building);
        }

        public bool MakesSound()
        {
            return PariteeCore.Characters.FarmAnimal.MakesSound(this.GetOriginal());
        }

        public string GetSound()
        {
            return PariteeCore.Characters.FarmAnimal.GetSound(this.GetOriginal());
        }

        public string GetDisplayType()
        {
            return PariteeCore.Characters.FarmAnimal.GetDisplayType(this.GetOriginal());
        }

        public string GetDisplayHouse()
        {
            return PariteeCore.Characters.FarmAnimal.GetDisplayHouse(this.GetOriginal());
        }

        public bool CanLiveIn(StardewValley.Buildings.Building building)
        {
            return PariteeCore.Characters.FarmAnimal.CanLiveIn(this.GetOriginal(), building);
        }

        public bool CanBeNamed()
        {
            return PariteeCore.Characters.FarmAnimal.CanBeNamed(this.GetOriginal());
        }

        public bool HasName()
        {
            return PariteeCore.Characters.FarmAnimal.HasName(this.GetOriginal());
        }

        public string GetName()
        {
            return PariteeCore.Characters.FarmAnimal.GetName(this.GetOriginal());
        }

        public bool IsCoopDweller()
        {
            return PariteeCore.Characters.FarmAnimal.IsCoopDweller(this.GetOriginal());
        }

        public bool HasController()
        {
            return PariteeCore.Characters.FarmAnimal.HasController(this.GetOriginal());
        }

        public Rectangle GetBoundingBox()
        {
            return PariteeCore.Characters.FarmAnimal.GetBoundingBox(this.GetOriginal());
        }

        public Vector2 GetTileLocation()
        {
            return PariteeCore.Characters.FarmAnimal.GetTileLocation(this.GetOriginal());
        }

        public int GetFacingDirection()
        {
            return PariteeCore.Characters.FarmAnimal.GetFacingDirection(this.GetOriginal());
        }
        
        public bool IsAProducer()
        {
            return PariteeCore.Characters.FarmAnimal.IsAProducer(this.GetOriginal());
        }

        public bool HasProduceThatMatchesAtLeastOne(int[] targets)
        {
            return PariteeCore.Characters.FarmAnimal.HasProduceThatMatchesAtLeastOne(this.GetOriginal(), targets);
        }

        public int GetDefaultProduce()
        {
            return PariteeCore.Characters.FarmAnimal.GetDefaultProduce(this.GetOriginal());
        }

        public int GetDeluxeProduce()
        {
            return PariteeCore.Characters.FarmAnimal.GetDeluxeProduce(this.GetOriginal());
        }

        public bool IsCurrentlyProducingDeluxe()
        {
            return PariteeCore.Characters.FarmAnimal.IsCurrentlyProducingDeluxe(this.GetOriginal());
        }

        public bool IsType(PariteeCore.Characters.Livestock type)
        {
            return PariteeCore.Characters.FarmAnimal.IsType(this.GetOriginal(), type);
        }

        public long GetOwnerId()
        {
            return PariteeCore.Characters.FarmAnimal.GetOwnerId(this.GetOriginal());
        }

        public StardewValley.Farmer GetOwner()
        {
            return PariteeCore.Utilities.Game.GetFarmer(this.GetOwnerId());
        }

        public byte GetDaysToLay(StardewValley.Farmer farmer)
        {
            return PariteeCore.Characters.FarmAnimal.GetDaysToLay(this.GetOriginal(), farmer);
        }

        public byte GetDaysSinceLastLay()
        {
            return PariteeCore.Characters.FarmAnimal.GetDaysSinceLastLay(this.GetOriginal());
        }

        public int GetMeatIndex()
        {
            return PariteeCore.Characters.FarmAnimal.GetMeatIndex(this.GetOriginal());
        }

        public StardewValley.Buildings.Building GetHome()
        {
            return PariteeCore.Characters.FarmAnimal.GetHome(this.GetOriginal());
        }

        public void SetDaysSinceLastLay(byte days)
        {
            PariteeCore.Characters.FarmAnimal.SetDaysSinceLastLay(this.GetOriginal(), days);
        }

        public PariteeCore.Objects.Object.Quality RollProduceQuality(StardewValley.Farmer farmer, int seed)
        {
            return PariteeCore.Characters.FarmAnimal.RollProduceQuality(this.GetOriginal(), farmer, seed);
        }

        public void SetProduceQuality(PariteeCore.Objects.Object.Quality quality)
        {
            PariteeCore.Characters.FarmAnimal.SetProduceQuality(this.GetOriginal(), quality);
        }

        public bool LaysProduce()
        {
            return PariteeCore.Characters.FarmAnimal.LaysProduce(this.GetOriginal());
        }

        public int GetProduceQuality()
        {
            return PariteeCore.Characters.FarmAnimal.GetProduceQuality(this.GetOriginal());
        }

        public void SetPauseTimer(int timer)
        {
            PariteeCore.Characters.FarmAnimal.SetPauseTimer(this.GetOriginal(), timer);
        }

        public int GetPauseTimer()
        {
            return PariteeCore.Characters.FarmAnimal.GetPauseTimer(this.GetOriginal());
        }

        public void SetHitGlowTimer(int timer)
        {
            PariteeCore.Characters.FarmAnimal.SetHitGlowTimer(this.GetOriginal(), timer);
        }

        public int GetHitGlowTimer()
        {
            return PariteeCore.Characters.FarmAnimal.GetHitGlowTimer(this.GetOriginal());
        }
    }
}
