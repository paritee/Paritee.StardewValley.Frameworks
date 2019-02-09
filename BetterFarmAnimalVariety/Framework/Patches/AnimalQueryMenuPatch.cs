using BetterFarmAnimalVariety.Framework.Data;
using StardewValley;
using StardewValley.Menus;
using System.Reflection;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    class AnimalQueryMenuPatch
    {
        // StardewValley.Menus.AnimalQueryMenu.receiveLeftClick
        public static void ReceiveLeftClickPostfix(ref AnimalQueryMenu __instance, ref int x, ref int y, ref bool playSound)
        {
            // Remove from save data if the animal gets sold

            if (!Helpers.Utilities.GetReflectedValue<bool>(__instance, "confirmingSell"))
            {
                return;
            }

            if (!__instance.yesButton.containsPoint(x, y))
            {
                return;
            }

            FarmAnimalsSaveData saveData = FarmAnimalsSaveData.Deserialize();
            FarmAnimal animal = Helpers.Utilities.GetReflectedValue<FarmAnimal>(__instance, "animal");

            saveData.RemoveTypeHistory(animal.myID.Value);
        }
    }
}
