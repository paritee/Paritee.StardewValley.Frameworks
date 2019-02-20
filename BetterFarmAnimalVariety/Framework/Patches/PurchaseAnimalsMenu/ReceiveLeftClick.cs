using Harmony;
using Microsoft.Xna.Framework;
using StardewValley;
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

            return PariteeCore.Helpers.Reflection.GetFieldValue<bool>(__instance, "onFarm")
                ? ReceiveLeftClick.HandleOnFarm(ref __instance, x, y)
                : ReceiveLeftClick.HandleStockSelection(ref __instance, x, y);
        }

        private static bool IsActionable(StardewValley.Menus.PurchaseAnimalsMenu __instance)
        {
            return !PariteeCore.Api.BellsAndWhistles.IsFaded() && !PariteeCore.Helpers.Reflection.GetFieldValue<bool>(__instance, "freeze");
        }

        private static bool IsClosingMenu(StardewValley.Menus.PurchaseAnimalsMenu __instance, int x, int y)
        {
            return __instance.okButton != null && __instance.okButton.containsPoint(x, y) && __instance.readyToClose();
        }

        private static bool HandleStockSelection(ref StardewValley.Menus.PurchaseAnimalsMenu __instance, int x, int y)
        {
            // Copy all of the logic checkpoints and try to return to the base 
            // code as much as possible
            ClickableTextureComponent textureComponent = __instance.animalsToPurchase
                .Where(o => (o.item as StardewValley.Object).Type == null)
                .FirstOrDefault(o => o.containsPoint(x, y));

            if (textureComponent == null)
            {
                return true;
            }

            Farmer player = PariteeCore.Api.Game.GetPlayer();
            int priceOfAnimal = textureComponent.item.salePrice();

            if (!PariteeCore.Api.Farmer.CanAfford(player, priceOfAnimal))
            {
                return true;
            }

            // Randomly choose a type from the category selected
            string type = ReceiveLeftClick.GetRandomType(textureComponent.hoverText);

            FarmAnimal animalBeingPurchased = PariteeCore.Api.FarmAnimal.CreateFarmAnimal(type, PariteeCore.Api.Farmer.GetUniqueId(player));

            ReceiveLeftClick.SelectedStockBellsAndWhistles(ref __instance);

            PariteeCore.Helpers.Reflection.GetField(__instance, "onFarm").SetValue(__instance, true);
            PariteeCore.Helpers.Reflection.GetField(__instance, "animalBeingPurchased").SetValue(__instance, animalBeingPurchased);
            PariteeCore.Helpers.Reflection.GetField(__instance, "priceOfAnimal").SetValue(__instance, priceOfAnimal);

            return false;
        }

        private static void SelectedStockBellsAndWhistles(ref StardewValley.Menus.PurchaseAnimalsMenu __instance)
        {
            PariteeCore.Api.BellsAndWhistles.FadeToBlack(true, __instance.setUpForAnimalPlacement, 0.02f);
            PariteeCore.Api.BellsAndWhistles.PlaySound("smallSelect");
        }

        private static string GetRandomType(string category)
        {
            List<string> types = Helpers.Mod.ReadConfig<ModConfig>().GroupPurchaseableTypesByCategory()[category];

            types = PariteeCore.Api.FarmAnimal.SanitizeBlueChickens(types, PariteeCore.Api.Game.GetPlayer());

            return types[PariteeCore.Helpers.Random.Next(types.Count)];
        }

        private static bool HandleOnFarm(ref StardewValley.Menus.PurchaseAnimalsMenu __instance, int x, int y)
        {
            // Copy all of the logic checkpoints and try to return to the base 
            // code as much as possible

            if (PariteeCore.Helpers.Reflection.GetFieldValue<bool>(__instance, "namingAnimal"))
            {
                return true;
            }

            xTile.Dimensions.Rectangle viewport = PariteeCore.Api.Game.GetViewport();
            Building buildingAt = PariteeCore.Api.Game.GetFarm().getBuildingAt(new Vector2((x + viewport.X) / 64, (y + viewport.Y) / 64));

            if (buildingAt == null)
            {
                return true;
            }

            FarmAnimal animalBeingPurchased = PariteeCore.Helpers.Reflection.GetFieldValue<FarmAnimal>(__instance, "animalBeingPurchased");

            if (!PariteeCore.Api.FarmAnimal.CanLiveIn(animalBeingPurchased, buildingAt))
            {
                return true;
            }

            if (PariteeCore.Api.AnimalHouse.IsFull(buildingAt))
            {
                return true;
            }

            // "It" harvest type doesn't allow you to name the animal. This is 
            // mostly unused and is only seen on the Hog
            if (!PariteeCore.Api.FarmAnimal.CanBeNamed(animalBeingPurchased))
            {
                return true;
            }

            // Rarely going to ever make it down here, but add the support any 
            // way for the "It" harvest type

            int priceOfAnimal = PariteeCore.Helpers.Reflection.GetFieldValue<int>(__instance, "priceOfAnimal");

            if (!PariteeCore.Api.Farmer.CanAfford(PariteeCore.Api.Game.GetPlayer(), priceOfAnimal))
            {
                return true;
            }

            PariteeCore.Api.FarmAnimal.AddToBuilding(ref animalBeingPurchased, ref buildingAt);

            PariteeCore.Helpers.Reflection.GetField(__instance, "animalBeingPurchased").SetValue(__instance, animalBeingPurchased);
            PariteeCore.Helpers.Reflection.GetField(__instance, "newAnimalHome").SetValue(__instance, null as Building);
            PariteeCore.Helpers.Reflection.GetField(__instance, "namingAnimal").SetValue(__instance, false);

            PariteeCore.Api.Farmer.SpendMoney(PariteeCore.Api.Game.GetPlayer(), priceOfAnimal);

            ReceiveLeftClick.PurchasedAnimalBellsAndWhistles(animalBeingPurchased);

            return false;
        }

        private static void PurchasedAnimalBellsAndWhistles(FarmAnimal animalBeingPurchased)
        {
            if (PariteeCore.Api.FarmAnimal.MakesSound(animalBeingPurchased))
            {
                PariteeCore.Api.BellsAndWhistles.CueSound(animalBeingPurchased.sound.Value, "Pitch", 1200 + PariteeCore.Helpers.Random.Next(-200, 201));
            }

            string message = PariteeCore.Api.Content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11324", animalBeingPurchased.displayType);

            PariteeCore.Api.BellsAndWhistles.AddHudMessage(message, Color.LimeGreen, 3500f);
        }
    }
}
