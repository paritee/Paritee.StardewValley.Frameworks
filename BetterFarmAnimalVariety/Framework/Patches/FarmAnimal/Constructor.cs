using BetterFarmAnimalVariety.Framework.Models;
using Harmony;

namespace BetterFarmAnimalVariety.Framework.Patches.FarmAnimal
{
    [HarmonyPatch(typeof(StardewValley.FarmAnimal), MethodType.Constructor, new[] { typeof(string), typeof(long), typeof(long) })]
    class Constructor : Patch
    {
        public static void Postfix(ref StardewValley.FarmAnimal __instance, ref string type, ref long id, ref long ownerID)
        {
            (new FarmAnimalsSaveData(Constants.Mod.Key)).Read().OverwriteFarmAnimal(__instance, type);
        }
    }
}
