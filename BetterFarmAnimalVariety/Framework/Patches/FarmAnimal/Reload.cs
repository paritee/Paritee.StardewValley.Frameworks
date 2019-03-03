using BetterFarmAnimalVariety.Framework.SaveData;
using Harmony;
using StardewValley.Buildings;

namespace BetterFarmAnimalVariety.Framework.Patches.FarmAnimal
{
    [HarmonyPatch(typeof(StardewValley.FarmAnimal), "reload")]
    class Reload : Patch
    {
        public static bool Prefix(ref StardewValley.FarmAnimal __instance, ref Building home)
        {
            Decorators.FarmAnimal moddedAnimal = new Decorators.FarmAnimal(__instance);

            if (!moddedAnimal.HasName())
            {
                return true;
            }

            moddedAnimal.SetHome(home);

            // Can't get the FarmAnimal empty constructor in a patch so need to 
            // use the reload function to handle it
            FarmAnimals saveData = Helpers.Mod.ReadSaveData<FarmAnimals>(Constants.Mod.FarmAnimalsSaveDataKey);

            saveData.OverwriteFarmAnimal(ref moddedAnimal, null);

            return false;
        }
    }
}
