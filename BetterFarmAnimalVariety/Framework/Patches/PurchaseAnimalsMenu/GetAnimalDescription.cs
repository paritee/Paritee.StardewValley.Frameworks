using Harmony;
using System.Collections.Generic;
using System.Diagnostics;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Patches.PurchaseAnimalsMenu
{
    [HarmonyPatch(typeof(StardewValley.Menus.PurchaseAnimalsMenu), "getAnimalDescription")]
    class GetAnimalDescription : Patch
    {
        public static bool Prefix(ref string name, ref string __result)
        {
            // Get the description from the config
            string category = name;

            __result = Helpers.FarmAnimals.GetCategory(category).AnimalShop.Description;

            // Add totals to see how many types the player can afford
            Decorators.Farmer moddedPlayer = new Decorators.Farmer(PariteeCore.Api.Game.GetPlayer());

            List<string> types = Helpers.FarmAnimals.GroupPurchaseableTypesByCategory()[category];

            // Remove blue chickens if needed
            types = moddedPlayer.SanitizeBlueChickens(types);

            int total = types.Count;

            // Remove any types that the player cannot afford
            types = moddedPlayer.SanitizeAffordableTypes(types);

            int available = types.Count;

            // Only display totals if there is a discrepancy and at least one 
            // type available for purchase
            if (available > 0 && available < total)
            {
                // Ex: (1/3 <gold symbol>)
                string[] substitutions = new string[] { available.ToString(), total.ToString(), "$" };
                __result += " (" + PariteeCore.Api.Content.LoadString("Strings\\Locations:AdventureGuild_KillList_LineFormat", substitutions) + ")";
            }

            return false;
        }
    }
}
