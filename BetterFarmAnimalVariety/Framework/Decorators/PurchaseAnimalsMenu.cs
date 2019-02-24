using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using System.Collections.Generic;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Decorators
{
    class PurchaseAnimalsMenu : Decorator
    {
        public PurchaseAnimalsMenu(StardewValley.Menus.PurchaseAnimalsMenu original) : base(original) { }

        public StardewValley.Menus.PurchaseAnimalsMenu GetOriginal()
        {
            return base.GetOriginal<StardewValley.Menus.PurchaseAnimalsMenu>();
        }

        public void SetUpAnimalsToPurchase(List<StardewValley.Object> stock, Dictionary<string, Texture2D> icons, out int iconHeight)
        {
            PariteeCore.Api.PurchaseAnimalsMenu.SetUpAnimalsToPurchase(this.GetOriginal(), stock, icons, out iconHeight);
        }

        public List<ClickableTextureComponent> GetAnimalsToPurchase()
        {
            return PariteeCore.Api.PurchaseAnimalsMenu.GetAnimalsToPurchase(this.GetOriginal());
        }

        public void SetHeight(int height)
        {
            PariteeCore.Api.PurchaseAnimalsMenu.SetHeight(this.GetOriginal(), height);
        }

        public int GetRows()
        {
            return PariteeCore.Api.PurchaseAnimalsMenu.GetRows(this.GetOriginal());
        }

        public void AdjustHeightBasedOnIcons(int iconHeight)
        {
            PariteeCore.Api.PurchaseAnimalsMenu.AdjustHeightBasedOnIcons(this.GetOriginal(), iconHeight);
        }

        public bool IsNamingAnimal()
        {
            return PariteeCore.Api.PurchaseAnimalsMenu.IsNamingAnimal(this.GetOriginal());
        }

        public StardewValley.FarmAnimal GetAnimalBeingPurchased()
        {
            return PariteeCore.Api.PurchaseAnimalsMenu.GetAnimalBeingPurchased(this.GetOriginal());
        }

        public int GetPriceOfAnimal()
        {
            return PariteeCore.Api.PurchaseAnimalsMenu.GetPriceOfAnimal(this.GetOriginal());
        }

        public void SetAnimalBeingPurchased(StardewValley.FarmAnimal animal)
        {
            PariteeCore.Api.PurchaseAnimalsMenu.SetAnimalBeingPurchased(this.GetOriginal(), animal);
        }

        public void SetNewAnimalHome(StardewValley.Buildings.Building building)
        {
            PariteeCore.Api.PurchaseAnimalsMenu.SetNewAnimalHome(this.GetOriginal(), building);
        }

        public void SetNamingAnimal(bool namingAnimal)
        {
            PariteeCore.Api.PurchaseAnimalsMenu.SetNamingAnimal(this.GetOriginal(), namingAnimal);
        }

        public bool IsOnFarm()
        {
            return PariteeCore.Api.PurchaseAnimalsMenu.IsOnFarm(this.GetOriginal());
        }

        public void SetOnFarm(bool onFarm)
        {
            PariteeCore.Api.PurchaseAnimalsMenu.SetOnFarm(this.GetOriginal(), onFarm);
        }

        public void SetPriceOfAnimal(int price)
        {
            PariteeCore.Api.PurchaseAnimalsMenu.SetPriceOfAnimal(this.GetOriginal(), price);
        }

        public bool IsFrozen()
        {
            return PariteeCore.Api.PurchaseAnimalsMenu.IsFrozen(this.GetOriginal());
        }

        public bool HasTappedOkButton(int x, int y)
        {
            return PariteeCore.Api.PurchaseAnimalsMenu.HasTappedOkButton(this.GetOriginal(), x, y);
        }

        public bool IsReadyToClose()
        {
            return PariteeCore.Api.PurchaseAnimalsMenu.IsReadyToClose(this.GetOriginal());
        }
    }
}
