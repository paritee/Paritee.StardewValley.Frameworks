using BetterFarmAnimalVariety.Framework.Data;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    class Patch
    {
        public static void CleanSaveData()
        {
            FarmAnimalsSaveData saveData = FarmAnimalsSaveData.Deserialize();

            saveData.CleanTypeHistory();
        }
    }
}
