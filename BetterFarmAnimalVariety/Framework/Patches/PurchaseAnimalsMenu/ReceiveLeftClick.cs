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
            if (!ReceiveLeftClick.IsActionable(__instance))
            {
                return true;
            }

            if (ReceiveLeftClick.IsClosingMenu(__instance, x, y))
            {
                return true;
            }

            StardewValley.Farmer player = PariteeCore.Api.Game.GetPlayer();

            return PariteeCore.Api.PurchaseAnimalsMenu.IsOnFarm(__instance)
                ? ReceiveLeftClick.HandleOnFarm(ref __instance, x, y, player)
                : ReceiveLeftClick.HandleStockSelection(ref __instance, x, y, player);
        }

        private static bool IsActionable(StardewValley.Menus.PurchaseAnimalsMenu __instance)
        {
            return !PariteeCore.Api.BellsAndWhistles.IsFaded() && !PariteeCore.Api.PurchaseAnimalsMenu.IsFrozen(__instance);
        }

        private static bool IsClosingMenu(StardewValley.Menus.PurchaseAnimalsMenu __instance, int x, int y)
        {
            return PariteeCore.Api.PurchaseAnimalsMenu.HasTappedOkButton(__instance, x, y) && PariteeCore.Api.PurchaseAnimalsMenu.IsReadyToClose(__instance);
        }

        private static bool HandleStockSelection(ref StardewValley.Menus.PurchaseAnimalsMenu __instance, int x, int y, StardewValley.Farmer player)
        {
            // Copy all of the logic checkpoints and try to return to the base 
            // code as much as possible
            ClickableTextureComponent textureComponent = PariteeCore.Api.PurchaseAnimalsMenu.GetAnimalsToPurchase(__instance)
                .Where(o => (o.item as StardewValley.Object).Type == null)
                .FirstOrDefault(o => o.containsPoint(x, y));

            if (textureComponent == null)
            {
                return true;
            }

            int priceOfAnimal = textureComponent.item.salePrice();

            if (!PariteeCore.Api.Farmer.CanAfford(player, priceOfAnimal))
            {
                return true;
            }

            // Randomly choose a type from the category selected
            string type = ReceiveLeftClick.GetRandomType(player, textureComponent.hoverText);

            StardewValley.FarmAnimal animalBeingPurchased = PariteeCore.Api.Farmer.CreateFarmAnimal(player, type);

            ReceiveLeftClick.SelectedStockBellsAndWhistles(ref __instance);

            PariteeCore.Api.PurchaseAnimalsMenu.SetOnFarm(__instance, true);
            PariteeCore.Api.PurchaseAnimalsMenu.SetAnimalBeingPurchased(__instance, animalBeingPurchased);
            PariteeCore.Api.PurchaseAnimalsMenu.SetPriceOfAnimal(__instance, priceOfAnimal);

            return false;
        }

        private static void SelectedStockBellsAndWhistles(ref StardewValley.Menus.PurchaseAnimalsMenu __instance)
        {
            PariteeCore.Api.BellsAndWhistles.FadeToBlack(true, __instance.setUpForAnimalPlacement, 0.02f);
            PariteeCore.Api.BellsAndWhistles.PlaySound("smallSelect");
        }

        private static string GetRandomType(StardewValley.Farmer player, string category)
        {
            List<string> types = Helpers.Mod.ReadConfig<ModConfig>().GroupPurchaseableTypesByCategory()[category];

            types = PariteeCore.Api.FarmAnimal.SanitizeBlueChickens(types, player);

            return types[PariteeCore.Helpers.Random.Next(types.Count)];
        }

        private static bool HandleOnFarm(ref StardewValley.Menus.PurchaseAnimalsMenu __instance, int x, int y, StardewValley.Farmer player)
        {
            // Copy all of the logic checkpoints and try to return to the base 
            // code as much as possible

            if (PariteeCore.Api.PurchaseAnimalsMenu.IsNamingAnimal(__instance))
            {
                return true;
            }

            xTile.Dimensions.Rectangle viewport = PariteeCore.Api.Game.GetViewport();
            Building buildingAt = PariteeCore.Api.Game.GetFarm().getBuildingAt(new Vector2((x + viewport.X) / 64, (y + viewport.Y) / 64));

            if (buildingAt == null)
            {
                return true;
            }

            StardewValley.FarmAnimal animal = PariteeCore.Api.PurchaseAnimalsMenu.GetAnimalBeingPurchased(__instance);

            if (!PariteeCore.Api.FarmAnimal.CanLiveIn(animal, buildingAt))
            {
                return true;
            }

            if (PariteeCore.Api.AnimalHouse.IsFull(buildingAt))
            {
                return true;
            }

            // "It" harvest type doesn't allow you to name the animal. This is 
            // mostly unused and is only seen on the Hog
            if (!PariteeCore.Api.FarmAnimal.CanBeNamed(animal))
            {
                return true;
            }

            // Rarely going to ever make it down here, but add the support any 
            // way for the "It" harvest type

            int priceOfAnimal = PariteeCore.Api.PurchaseAnimalsMenu.GetPriceOfAnimal(__instance);

            if (!PariteeCore.Api.Farmer.CanAfford(player, priceOfAnimal))
            {
                return true;
            }

            PariteeCore.Api.FarmAnimal.AddToBuilding(animal, buildingAt);

            PariteeCore.Api.PurchaseAnimalsMenu.SetAnimalBeingPurchased(__instance, animal);
            PariteeCore.Api.PurchaseAnimalsMenu.SetNewAnimalHome(__instance, null as StardewValley.Buildings.Building);
            PariteeCore.Api.PurchaseAnimalsMenu.SetNamingAnimal(__instance, false);

            PariteeCore.Api.Farmer.SpendMoney(player, priceOfAnimal);

            ReceiveLeftClick.PurchasedAnimalBellsAndWhistles(animal);

            return false;
        }

        private static void PurchasedAnimalBellsAndWhistles(StardewValley.FarmAnimal animal)
        {
            if (PariteeCore.Api.FarmAnimal.MakesSound(animal))
            {
                PariteeCore.Api.BellsAndWhistles.CueSound(PariteeCore.Api.FarmAnimal.GetSound(animal), "Pitch", 1200 + PariteeCore.Helpers.Random.Next(-200, 201));
            }

            string message = PariteeCore.Api.Content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11324", PariteeCore.Api.FarmAnimal.GetDisplayType(animal));

            PariteeCore.Api.BellsAndWhistles.AddHudMessage(message, Color.LimeGreen, 3500f);
        }
    }
}
