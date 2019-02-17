using BetterFarmAnimalVariety.Framework.SaveData;
using Harmony;
using StardewValley;
using StardewValley.Menus;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    //[HarmonyPatch(typeof(AnimalQueryMenu))]
    //[HarmonyPatch("receiveLeftClick")]
    class AnimalQueryMenuPatch
    {
        public static void Postfix(ref AnimalQueryMenu __instance, ref int x, ref int y, ref bool playSound)
        {
            // Remove from save data if the animal gets sold
            if (!AnimalQueryMenuPatch.IsSelling(__instance, x, y))
            {
                return;
            }

            FarmAnimalsSaveData saveData = FarmAnimalsSaveData.Deserialize();
            FarmAnimal animal = Helpers.Reflection.GetFieldValue<FarmAnimal>(__instance, "animal");

            saveData.RemoveTypeHistory(animal.myID.Value);
        }

        private static bool IsSelling(AnimalQueryMenu __instance, int x, int y)
        {
            if (!Helpers.Reflection.GetFieldValue<bool>(__instance, "confirmingSell"))
            {
                return false;
            }

            if (!__instance.yesButton.containsPoint(x, y))
            {
                return false;
            }

            return true;
        }
    }
}
