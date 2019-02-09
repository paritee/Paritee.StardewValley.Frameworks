using BetterFarmAnimalVariety.Framework.Data;
using StardewValley;
using StardewValley.Buildings;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    class FarmAnimalPatch
    {
        // FarmInfoPage
        // Forest

        public static void FarmAnimalPostfix(ref FarmAnimal __instance, ref string type, ref long id, ref long ownerID, ref string __result)
        {
            FarmAnimalsSaveData saveData = FarmAnimalsSaveData.Deserialize();
            long myId = id;

            KeyValuePair<long, TypeHistory> saveDataEntry = saveData.TypeHistory
                .First(kvp => kvp.Key.Equals(myId));

            //// TODO
            // Don't sanitize a farm animals type by blue/void/brown cow chance/etc. or BFAV 
            // config existence. These checks should be done in the menus, etc.
            ////

            if (saveDataEntry.Key == -1L) // TODO: Validate this is how First() with nothing found behaves
            {
                // Add the animal
                saveData.AddTypeHistory(myId, type, __instance.type.Value);

                return;
            }

            // Grab the new type's data to override
            KeyValuePair<string, string> contentDataEntry = Content.FarmAnimalsData.Load().First(kvp => kvp.Key.Equals(saveDataEntry.Value));

            if (contentDataEntry.Key == null) // TODO: Validate this is how First() with nothing found behaves
            {
                saveData.RemoveTypeHistory(saveDataEntry.Key);

                // Nothing more to do
                return;
            }

            // Get the new type's data values
            Helpers.Utilities.OverwriteAnimalFromData(ref __instance, contentDataEntry);
        }

        public static void ReloadPostfix(ref FarmAnimal __instance, ref Building home, ref string __result)
        {
            __instance.Sprite = new AnimatedSprite(Helpers.Utilities.DetermineSpriteAssetName(__instance), 0, __instance.frontBackSourceRect.Width, __instance.frontBackSourceRect.Height);
        }

    }
}
