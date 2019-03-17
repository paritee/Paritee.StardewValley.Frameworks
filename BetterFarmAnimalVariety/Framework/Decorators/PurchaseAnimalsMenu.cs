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
            PariteeCore.Menus.PurchaseAnimals.SetUpAnimalsToPurchase(this.GetOriginal(), stock, icons, out iconHeight);
        }

        public List<ClickableTextureComponent> GetAnimalsToPurchase()
        {
            return PariteeCore.Menus.PurchaseAnimals.GetAnimalsToPurchase(this.GetOriginal());
        }

        public void SetHeight(int height)
        {
            PariteeCore.Menus.PurchaseAnimals.SetHeight(this.GetOriginal(), height);
        }

        public int GetRows()
        {
            return PariteeCore.Menus.PurchaseAnimals.GetRows(this.GetOriginal());
        }

        public void AdjustHeightBasedOnIcons(int iconHeight)
        {
            PariteeCore.Menus.PurchaseAnimals.AdjustHeightBasedOnIcons(this.GetOriginal(), iconHeight);
        }

        public bool IsNamingAnimal()
        {
            return PariteeCore.Menus.PurchaseAnimals.IsNamingAnimal(this.GetOriginal());
        }

        public StardewValley.FarmAnimal GetAnimalBeingPurchased()
        {
            return PariteeCore.Menus.PurchaseAnimals.GetAnimalBeingPurchased(this.GetOriginal());
        }

        public int GetPriceOfAnimal()
        {
            return PariteeCore.Menus.PurchaseAnimals.GetPriceOfAnimal(this.GetOriginal());
        }

        public void SetAnimalBeingPurchased(StardewValley.FarmAnimal animal)
        {
            PariteeCore.Menus.PurchaseAnimals.SetAnimalBeingPurchased(this.GetOriginal(), animal);
        }

        public void SetNewAnimalHome(StardewValley.Buildings.Building building)
        {
            PariteeCore.Menus.PurchaseAnimals.SetNewAnimalHome(this.GetOriginal(), building);
        }

        public void SetNamingAnimal(bool namingAnimal)
        {
            PariteeCore.Menus.PurchaseAnimals.SetNamingAnimal(this.GetOriginal(), namingAnimal);
        }

        public bool IsOnFarm()
        {
            return PariteeCore.Menus.PurchaseAnimals.IsOnFarm(this.GetOriginal());
        }

        public void SetOnFarm(bool onFarm)
        {
            PariteeCore.Menus.PurchaseAnimals.SetOnFarm(this.GetOriginal(), onFarm);
        }

        public void SetPriceOfAnimal(int price)
        {
            PariteeCore.Menus.PurchaseAnimals.SetPriceOfAnimal(this.GetOriginal(), price);
        }

        public bool IsFrozen()
        {
            return PariteeCore.Menus.PurchaseAnimals.IsFrozen(this.GetOriginal());
        }

        public bool HasTappedOkButton(int x, int y)
        {
            return PariteeCore.Menus.PurchaseAnimals.HasTappedOkButton(this.GetOriginal(), x, y);
        }

        public bool IsReadyToClose()
        {
            return PariteeCore.Menus.PurchaseAnimals.IsReadyToClose(this.GetOriginal());
        }
    }
}
