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

        public string GetTypeString()
        {
            return PariteeCore.Api.FarmAnimal.GetType(this.GetOriginal());
        }

        public long GetUniqueId()
        {
            return PariteeCore.Api.FarmAnimal.GetUniqueId(this.GetOriginal());
        }
        
        public bool IsVanilla()
        {
            return PariteeCore.Api.FarmAnimal.IsVanilla(this.GetTypeString());
        }

        public int RollProduce(StardewValley.Farmer farmer, int seed)
        {
            return PariteeCore.Api.FarmAnimal.RollProduce(this.GetOriginal(), farmer, seed);
        }

        public int GetCurrentProduce()
        {
            return PariteeCore.Api.FarmAnimal.GetCurrentProduce(this.GetOriginal());
        }

        public int GetPrice()
        {
            return PariteeCore.Api.FarmAnimal.GetPrice(this.GetOriginal());
        }

        public void SetCurrentProduce(int produceIndex)
        {
            PariteeCore.Api.FarmAnimal.SetCurrentProduce(this.GetOriginal(), produceIndex);
        }

        public void UpdateFromData(string type)
        {
            PariteeCore.Api.FarmAnimal.UpdateFromData(this.GetOriginal(), type);
        }

        public string GetDefaultType()
        {
            return PariteeCore.Api.FarmAnimal.GetDefaultType(this.GetOriginal());
        }

        public void AddToBuilding(StardewValley.Buildings.Building building)
        {
            PariteeCore.Api.FarmAnimal.AddToBuilding(this.GetOriginal(), building);
        }

        public string GetRandomTypeFromProduce(Dictionary<string, List<string>> restrictions)
        {
            return PariteeCore.Api.FarmAnimal.GetRandomTypeFromProduce(this.GetOriginal(), restrictions)
                ?? PariteeCore.Api.FarmAnimal.GetDefaultBarnDwellerType();
        }

        public void AssociateParent(StardewValley.FarmAnimal parent)
        {
            PariteeCore.Api.FarmAnimal.AssociateParent(this.GetOriginal(), parent);
        }

        public bool HasHome()
        {
            return PariteeCore.Api.FarmAnimal.HasHome(this.GetOriginal());
        }

        public bool IsEating()
        {
            return PariteeCore.Api.FarmAnimal.IsEating(this.GetOriginal());
        }

        public byte GetFullness()
        {
            return PariteeCore.Api.FarmAnimal.GetFullness(this.GetOriginal());
        }

        public byte GetHappiness()
        {
            return PariteeCore.Api.FarmAnimal.GetHappiness(this.GetOriginal());
        }

        public void SetFindGrassPathController(GameLocation location)
        {
            PariteeCore.Api.FarmAnimal.SetFindGrassPathController(this.GetOriginal(), location);
        }

        public void ReturnHome()
        {
            PariteeCore.Api.FarmAnimal.ReturnHome(this.GetOriginal());
        }

        public void SetFindHomeDoorPathController(GameLocation location)
        {
            PariteeCore.Api.FarmAnimal.SetFindHomeDoorPathController(this.GetOriginal(), location);
        }

        public bool IsBaby()
        {
            return PariteeCore.Api.FarmAnimal.IsBaby(this.GetOriginal());
        }

        public bool CanFindProduce()
        {
            return PariteeCore.Api.FarmAnimal.CanFindProduce(this.GetOriginal());
        }

        public void AnimateFindingProduce()
        {
            PariteeCore.Api.FarmAnimal.AnimateFindingProduce(this.GetOriginal());
        }

        public void FindProduce(StardewValley.Farmer farmer)
        {
            PariteeCore.Api.FarmAnimal.FindProduce(this.GetOriginal(), farmer);
        }

        public int GetFriendship()
        {
            return PariteeCore.Api.FarmAnimal.GetFriendship(this.GetOriginal());
        }

        public void SetHome(StardewValley.Buildings.Building building)
        {
            PariteeCore.Api.FarmAnimal.SetHome(this.GetOriginal(), building);
        }

        public bool MakesSound()
        {
            return PariteeCore.Api.FarmAnimal.MakesSound(this.GetOriginal());
        }

        public string GetSound()
        {
            return PariteeCore.Api.FarmAnimal.GetSound(this.GetOriginal());
        }

        public string GetDisplayType()
        {
            return PariteeCore.Api.FarmAnimal.GetDisplayType(this.GetOriginal());
        }

        public string GetDisplayHouse()
        {
            return PariteeCore.Api.FarmAnimal.GetDisplayHouse(this.GetOriginal());
        }

        public bool CanLiveIn(StardewValley.Buildings.Building building)
        {
            return PariteeCore.Api.FarmAnimal.CanLiveIn(this.GetOriginal(), building);
        }

        public bool CanBeNamed()
        {
            return PariteeCore.Api.FarmAnimal.CanBeNamed(this.GetOriginal());
        }

        public bool HasName()
        {
            return PariteeCore.Api.FarmAnimal.HasName(this.GetOriginal());
        }

        public string GetName()
        {
            return PariteeCore.Api.FarmAnimal.GetName(this.GetOriginal());
        }

        public bool IsCoopDweller()
        {
            return PariteeCore.Api.FarmAnimal.IsCoopDweller(this.GetOriginal());
        }

        public bool HasController()
        {
            return PariteeCore.Api.FarmAnimal.HasController(this.GetOriginal());
        }

        public Rectangle GetBoundingBox()
        {
            return PariteeCore.Api.FarmAnimal.GetBoundingBox(this.GetOriginal());
        }

        public Vector2 GetTileLocation()
        {
            return PariteeCore.Api.FarmAnimal.GetTileLocation(this.GetOriginal());
        }

        public int GetFacingDirection()
        {
            return PariteeCore.Api.FarmAnimal.GetFacingDirection(this.GetOriginal());
        }
        
        public bool IsAProducer()
        {
            return PariteeCore.Api.FarmAnimal.IsAProducer(this.GetOriginal());
        }

        public bool HasProduceThatMatchesAtLeastOne(int[] targets)
        {
            return PariteeCore.Api.FarmAnimal.HasProduceThatMatchesAtLeastOne(this.GetOriginal(), targets);
        }

        public int GetDefaultProduce()
        {
            return PariteeCore.Api.FarmAnimal.GetDefaultProduce(this.GetOriginal());
        }

        public int GetDeluxeProduce()
        {
            return PariteeCore.Api.FarmAnimal.GetDeluxeProduce(this.GetOriginal());
        }

        public bool IsType(PariteeCore.Constants.VanillaAnimalType type)
        {
            return PariteeCore.Api.FarmAnimal.IsType(this.GetOriginal(), type);
        }

        public long GetOwnerId()
        {
            return PariteeCore.Api.FarmAnimal.GetOwnerId(this.GetOriginal());
        }

        public StardewValley.Farmer GetOwner()
        {
            return PariteeCore.Api.Game.GetFarmer(this.GetOwnerId());
        }

        public byte GetDaysToLay(StardewValley.Farmer farmer)
        {
            return PariteeCore.Api.FarmAnimal.GetDaysToLay(this.GetOriginal(), farmer);
        }

        public byte GetDaysSinceLastLay()
        {
            return PariteeCore.Api.FarmAnimal.GetDaysSinceLastLay(this.GetOriginal());
        }

        public int GetMeatIndex()
        {
            return PariteeCore.Api.FarmAnimal.GetMeatIndex(this.GetOriginal());
        }

        public StardewValley.Buildings.Building GetHome()
        {
            return PariteeCore.Api.FarmAnimal.GetHome(this.GetOriginal());
        }

        public void SetDaysSinceLastLay(byte days)
        {
            PariteeCore.Api.FarmAnimal.SetDaysSinceLastLay(this.GetOriginal(), days);
        }

        public PariteeCore.Constants.Object.Quality RollProduceQuality(StardewValley.Farmer farmer, int seed)
        {
            return PariteeCore.Api.FarmAnimal.RollProduceQuality(this.GetOriginal(), farmer, seed);
        }

        public void SetProduceQuality(PariteeCore.Constants.Object.Quality quality)
        {
            PariteeCore.Api.FarmAnimal.SetProduceQuality(this.GetOriginal(), quality);
        }

        public bool LaysProduce()
        {
            return PariteeCore.Api.FarmAnimal.LaysProduce(this.GetOriginal());
        }

        public int GetProduceQuality()
        {
            return PariteeCore.Api.FarmAnimal.GetProduceQuality(this.GetOriginal());
        }

        public void SetPauseTimer(int timer)
        {
            PariteeCore.Api.FarmAnimal.SetPauseTimer(this.GetOriginal(), timer);
        }

        public int GetPauseTimer()
        {
            return PariteeCore.Api.FarmAnimal.GetPauseTimer(this.GetOriginal());
        }

        public void SetHitGlowTimer(int timer)
        {
            PariteeCore.Api.FarmAnimal.SetHitGlowTimer(this.GetOriginal(), timer);
        }

        public int GetHitGlowTimer()
        {
            return PariteeCore.Api.FarmAnimal.GetHitGlowTimer(this.GetOriginal());
        }
    }
}
