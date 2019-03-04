using Harmony;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using System.Diagnostics;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Patches.PurchaseAnimalsMenu
{
    [HarmonyPatch(typeof(StardewValley.Menus.PurchaseAnimalsMenu), "draw")]
    class Draw : Patch
    {
        public static bool Prefix(ref StardewValley.Menus.PurchaseAnimalsMenu __instance, ref SpriteBatch b)
        {
            Decorators.PurchaseAnimalsMenu moddedMenu = new Decorators.PurchaseAnimalsMenu(__instance);
            Decorators.FarmAnimal moddedAnimal = new Decorators.FarmAnimal(moddedMenu.GetAnimalBeingPurchased());

            if (!PariteeCore.Api.BellsAndWhistles.IsFaded() && moddedMenu.IsOnFarm())
            {
                int price = moddedAnimal.GetPrice();
                string str = PariteeCore.Api.Content.FormatMoneyString(price);
                int x = PariteeCore.Api.Game.GetViewport().Width / 2 - PariteeCore.Api.Content.GetWidthOfString(str) / 2;
                int y = (int)((SpriteText.characterHeight + SpriteText.verticalSpaceBetweenCharacters) * SpriteText.fontPixelZoom * 2);

                PariteeCore.Api.BellsAndWhistles.DrawScroll(b, str, x, y);
            }

            return true;
        }
    }
}
