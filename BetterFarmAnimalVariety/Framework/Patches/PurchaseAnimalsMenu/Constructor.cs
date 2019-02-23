using Harmony;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Patches.PurchaseAnimalsMenu
{
    [HarmonyPatch(typeof(StardewValley.Menus.PurchaseAnimalsMenu), MethodType.Constructor, new[] { typeof(List<StardewValley.Object>) })]
    class Constructor : Patch
    {
        public static void Postfix(ref StardewValley.Menus.PurchaseAnimalsMenu __instance, ref List<StardewValley.Object> stock)
        {
            // Load the config
            ModConfig config = Helpers.Mod.ReadConfig<ModConfig>();
            List<StardewValley.Object> configStock = config.GetPurchaseAnimalStock(PariteeCore.Api.Game.GetFarm());

            // Grab the icons from the config by category
            Dictionary<string, Texture2D> icons = config.FarmAnimals
                .Where(o => o.CanBePurchased())
                .ToDictionary(o => o.Category, o => o.AnimalShop.GetIconTexture());

            // We need to completely redo the animalsToPurchase to account for the custom sprites
            PariteeCore.Api.PurchaseAnimalsMenu.SetUpAnimalsToPurchase(__instance, stock, icons, out int iconHeight);

            Constructor.AdjustMenuHeight(ref __instance, iconHeight);
        }

        private static void AdjustMenuHeight(ref StardewValley.Menus.PurchaseAnimalsMenu __instance, int iconHeight)
        {
            // Adjust the size of the menu if there are more or less rows than it normally handles
            if (iconHeight > 0)
            {
                PariteeCore.Api.PurchaseAnimalsMenu.AdjustHeightBasedOnIcons(__instance, iconHeight);
            }
        }
    }
}
