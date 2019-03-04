using Harmony;
using Microsoft.Xna.Framework;
using StardewValley.Buildings;
using StardewValley.Menus;
using System.Collections.Generic;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Patches.PurchaseAnimalsMenu
{
    [HarmonyPatch(typeof(StardewValley.Menus.PurchaseAnimalsMenu), "receiveLeftClick")]
    class ReceiveLeftClick
    {
        public static bool Prefix(ref StardewValley.Menus.PurchaseAnimalsMenu __instance, ref int x, ref int y, ref bool playSound)
        {
            Decorators.PurchaseAnimalsMenu moddedMenu = new Decorators.PurchaseAnimalsMenu(__instance);

            if (!ReceiveLeftClick.IsActionable(moddedMenu))
            {
                return true;
            }

            if (ReceiveLeftClick.IsClosingMenu(moddedMenu, x, y))
            {
                return true;
            }

            Decorators.Farmer moddedPlayer = new Decorators.Farmer(PariteeCore.Api.Game.GetPlayer());

            return moddedMenu.IsOnFarm()
                ? ReceiveLeftClick.HandleOnFarm(ref moddedMenu, x, y, moddedPlayer)
                : ReceiveLeftClick.HandleStockSelection(ref moddedMenu, x, y, moddedPlayer);
        }

        private static bool IsActionable(Decorators.PurchaseAnimalsMenu moddedMenu)
        {
            return !PariteeCore.Api.BellsAndWhistles.IsFaded() && !moddedMenu.IsFrozen();
        }

        private static bool IsClosingMenu(Decorators.PurchaseAnimalsMenu moddedMenu, int x, int y)
        {
            return moddedMenu.HasTappedOkButton(x, y) && moddedMenu.IsReadyToClose();
        }

        private static bool HandleStockSelection(ref Decorators.PurchaseAnimalsMenu moddedMenu, int x, int y, Decorators.Farmer moddedPlayer)
        {
            // Copy all of the logic checkpoints and try to return to the base 
            // code as much as possible
            ClickableTextureComponent textureComponent = moddedMenu.GetAnimalsToPurchase()
                .Where(o => (o.item as StardewValley.Object).Type == null)
                .FirstOrDefault(o => o.containsPoint(x, y));

            if (textureComponent == null)
            {
                return true;
            }

            int priceOfAnimal = textureComponent.item.salePrice();

            if (!moddedPlayer.CanAfford(priceOfAnimal))
            {
                return true;
            }

            // Randomly choose a type from the category selected if the type is 
            // within the player's budget
            string type = ReceiveLeftClick.GetRandomType(moddedPlayer, textureComponent.hoverText);

            StardewValley.FarmAnimal animalBeingPurchased = moddedPlayer.CreateFarmAnimal(type);

            ReceiveLeftClick.SelectedStockBellsAndWhistles(ref moddedMenu);

            moddedMenu.SetOnFarm(true);
            moddedMenu.SetAnimalBeingPurchased(animalBeingPurchased);

            // Use the animal's price as the purchase price instead of the lowest 
            // price found attached to the menu
            Decorators.FarmAnimal moddedAnimal = new Decorators.FarmAnimal(animalBeingPurchased);

            moddedMenu.SetPriceOfAnimal(moddedAnimal.GetPrice());

            return false;
        }

        private static void SelectedStockBellsAndWhistles(ref Decorators.PurchaseAnimalsMenu moddedMenu)
        {
            PariteeCore.Api.BellsAndWhistles.FadeToBlack(true, moddedMenu.GetOriginal().setUpForAnimalPlacement, 0.02f);
            PariteeCore.Api.BellsAndWhistles.PlaySound("smallSelect");
        }

        private static string GetRandomType(Decorators.Farmer moddedPlayer, string category)
        {
            List<string> types = Helpers.FarmAnimals.GroupPurchaseableTypesByCategory()[category];

            // Remove blue chickens if needed
            types = moddedPlayer.SanitizeBlueChickens(types);

            // Remove any types that the player cannot afford
            types = moddedPlayer.SanitizeAffordableTypes(types);

            return types[PariteeCore.Helpers.Random.Next(types.Count)];
        }

        private static bool HandleOnFarm(ref Decorators.PurchaseAnimalsMenu moddedMenu, int x, int y, Decorators.Farmer moddedPlayer)
        {
            // Copy all of the logic checkpoints and try to return to the base 
            // code as much as possible

            if (moddedMenu.IsNamingAnimal())
            {
                return true;
            }

            xTile.Dimensions.Rectangle viewport = PariteeCore.Api.Game.GetViewport();
            Building buildingAt = PariteeCore.Api.Game.GetFarm().getBuildingAt(new Vector2((x + viewport.X) / 64, (y + viewport.Y) / 64));

            if (buildingAt == null)
            {
                return true;
            }

            StardewValley.FarmAnimal animal = moddedMenu.GetAnimalBeingPurchased();
            Decorators.FarmAnimal moddedAnimal = new Decorators.FarmAnimal(animal);

            if (!moddedAnimal.CanLiveIn(buildingAt))
            {
                return true;
            }

            Decorators.Building moddedBuilding = new Decorators.Building(buildingAt);

            if (moddedBuilding.IsFull())
            {
                return true;
            }

            // "It" harvest type doesn't allow you to name the animal. This is 
            // mostly unused and is only seen on the Hog
            if (!moddedAnimal.CanBeNamed())
            {
                return true;
            }

            // Rarely going to ever make it down here, but add the support any 
            // way for the "It" harvest type

            int priceOfAnimal = moddedMenu.GetPriceOfAnimal();

            if (!moddedPlayer.CanAfford(priceOfAnimal))
            {
                return true;
            }

            moddedAnimal.AddToBuilding(buildingAt);

            moddedMenu.SetAnimalBeingPurchased(animal);
            moddedMenu.SetNewAnimalHome(null as StardewValley.Buildings.Building);
            moddedMenu.SetNamingAnimal(false);

            moddedPlayer.SpendMoney(priceOfAnimal);

            ReceiveLeftClick.PurchasedAnimalBellsAndWhistles(moddedAnimal);

            return false;
        }

        private static void PurchasedAnimalBellsAndWhistles(Decorators.FarmAnimal moddedAnimal)
        {
            if (moddedAnimal.MakesSound())
            {
                PariteeCore.Api.BellsAndWhistles.CueSound(moddedAnimal.GetSound(), "Pitch", 1200 + PariteeCore.Helpers.Random.Next(-200, 201));
            }

            string message = PariteeCore.Api.Content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11324", moddedAnimal.GetDisplayType());

            PariteeCore.Api.BellsAndWhistles.AddHudMessage(message, Color.LimeGreen, 3500f);
        }
    }
}
