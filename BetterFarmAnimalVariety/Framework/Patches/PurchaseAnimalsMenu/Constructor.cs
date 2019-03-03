using Harmony;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Patches.PurchaseAnimalsMenu
{
    [HarmonyPatch(typeof(StardewValley.Menus.PurchaseAnimalsMenu), MethodType.Constructor, new[] { typeof(List<StardewValley.Object>) })]
    class Constructor : Patch
    {
        public static void Postfix(ref StardewValley.Menus.PurchaseAnimalsMenu __instance, ref List<StardewValley.Object> stock)
        {
            Decorators.PurchaseAnimalsMenu moddedMenu = new Decorators.PurchaseAnimalsMenu(__instance);

            // Grab the icons from the config by category
            Dictionary<string, Texture2D> icons = Helpers.FarmAnimals.GetCategories()
                .Where(o => o.CanBePurchased())
                .ToDictionary(o => o.Category, o => o.GetAnimalShopIconTexture());

            // We need to completely redo the animalsToPurchase to account for the custom sprites
            moddedMenu.SetUpAnimalsToPurchase(stock, icons, out int iconHeight);

            Constructor.AdjustMenuHeight(ref moddedMenu, iconHeight);
        }

        private static void AdjustMenuHeight(ref Decorators.PurchaseAnimalsMenu moddedMenu, int iconHeight)
        {
            // Adjust the size of the menu if there are more or less rows than it normally handles
            if (iconHeight > 0)
            {
                moddedMenu.AdjustHeightBasedOnIcons(iconHeight);
            }
        }
    }
}
