using Paritee.StardewValleyAPI.Buildings.AnimalShop;
using Paritee.StardewValleyAPI.Menus;
using Paritee.StardewValleyAPI.Players;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace BetterFarmAnimalVariety.Framework.Events
{
    class PurchaseFarmAnimal
    {
        private void OnButtonPressed(Player player, AnimalShop animalShop, ButtonPressedEventArgs e)
        {
            // Ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
            {
                return;
            }

            // We only care about left mouse clicks right now
            if (e.Button != SButton.MouseLeft)
            {
                return;
            }

            ActiveClickableMenu activeClickableMenu = new ActiveClickableMenu(Game1.activeClickableMenu);

            if (!activeClickableMenu.IsOpen())
            {
                return;
            }

            if (!(activeClickableMenu.GetMenu() is StardewValley.Menus.PurchaseAnimalsMenu))
            {
                return;
            }

            // Purchasing a new animal
            StardewValley.Menus.PurchaseAnimalsMenu purchaseAnimalsMenu = activeClickableMenu.GetMenu() as StardewValley.Menus.PurchaseAnimalsMenu;

            Paritee.StardewValleyAPI.Players.Actions.PurchaseFarmAnimal purchaseFarmAnimal = new Paritee.StardewValleyAPI.Players.Actions.PurchaseFarmAnimal(player, animalShop);
            PurchaseFarmAnimalMenu purchaseFarmAnimalMenu = new PurchaseFarmAnimalMenu(purchaseAnimalsMenu, purchaseFarmAnimal);

            purchaseFarmAnimalMenu.HandleTap(e);
        }
    }
}
