using Harmony;
using StardewValley;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    [HarmonyPatch(typeof(FarmAnimal), MethodType.Constructor, new[] { typeof(string), typeof(long), typeof(long) })]
    class Constructor : Patch
    {
        public static void Postfix(ref FarmAnimal __instance, ref string type, ref long id, ref long ownerID) 
        {
            Helpers.GameSave.OverwriteFarmAnimal(ref __instance, type);
        }
    }
}
