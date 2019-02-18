using Harmony;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Patches.PurchaseAnimalsMenu
{
    [HarmonyPatch(typeof(StardewValley.Menus.PurchaseAnimalsMenu), MethodType.Constructor, new[] { typeof(List<StardewValley.Object>) })]
    class Constructor : Patch
    {
        public static void Postfix(ref StardewValley.Menus.PurchaseAnimalsMenu __instance, ref List<StardewValley.Object> stock)
        {
            Constructor.RefreshAnimalsToPurchase(ref __instance, out int iconHeight);
            Constructor.AdjustMenuHeight(ref __instance, iconHeight);
        }

        private static void RefreshAnimalsToPurchase(ref StardewValley.Menus.PurchaseAnimalsMenu __instance, out int iconHeight)
        {
            // Load the config
            ModConfig config = Helpers.Mod.LoadConfig<ModConfig>();
           
            // Grab the icons from the config by category
            Dictionary<string, Texture2D> icons = config.FarmAnimals
                .Where(o => o.CanBePurchased())
                .ToDictionary(o => o.Category, o => o.AnimalShop.GetIconTexture());

            // We need to completely redo the animalsToPurchase to account for the custom sprites
            __instance.animalsToPurchase = Api.AnimalShop.GetAnimalsToPurchaseComponents(__instance, config.GetPurchaseAnimalStock(Game1.getFarm()), icons, out iconHeight);
        }

        private static void AdjustMenuHeight(ref StardewValley.Menus.PurchaseAnimalsMenu __instance, int iconHeight)
        {
            int rows = (int)Math.Ceiling((float)__instance.animalsToPurchase.Count / 3); // Always at least one row

            // Adjust the size of the menu if there are more or less rows than it normally handles
            if (iconHeight > 0)
            {
                __instance.height = (int)(iconHeight * 2f) + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth / 2 + rows * 85;
            }
        }
    }
}
