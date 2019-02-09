using BetterFarmAnimalVariety.Framework.Data;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    class UtilityPatch
    {
        // TODO: StardewValley.Utility.getPurchaseAnimalStock

        // StardewValley.Utility.fixAllAnimals
        public static void FixAllAnimalsPostfix()
        {
            FarmAnimalsSaveData saveData = FarmAnimalsSaveData.Deserialize();

            saveData.CleanTypeHistory();
        }
    }
}
