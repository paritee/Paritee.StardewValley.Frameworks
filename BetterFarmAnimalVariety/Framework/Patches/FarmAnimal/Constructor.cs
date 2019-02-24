using BetterFarmAnimalVariety.Framework.Models;
using Harmony;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Patches.FarmAnimal
{
    [HarmonyPatch(typeof(StardewValley.FarmAnimal), MethodType.Constructor, new[] { typeof(string), typeof(long), typeof(long) })]
    class Constructor : Patch
    {
        public static void Postfix(ref StardewValley.FarmAnimal __instance, ref string type, ref long id, ref long ownerID)
        {
            FarmAnimalsSaveData saveData = Helpers.Mod.ReadSaveData<FarmAnimalsSaveData>(Constants.Mod.FarmAnimalsSaveDataKey);
            Decorators.FarmAnimal moddedAnimal = new Decorators.FarmAnimal(__instance);

            saveData.OverwriteFarmAnimal(moddedAnimal, type);
        }
    }
}
