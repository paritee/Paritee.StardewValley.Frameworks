using BetterFarmAnimalVariety.Framework.Data;
using Harmony;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Patches
{
    [HarmonyPatch(typeof(FarmAnimal))]
    [HarmonyPatch(new Type[] { })]
    class FarmAnimalPatch : Patch
    {
        public static void Postfix(ref FarmAnimal __instance, ref string type, ref long id, ref long ownerID, ref string __result)
        {
            FarmAnimalsSaveData saveData = FarmAnimalsSaveData.Deserialize();
            long myId = id;

            KeyValuePair<long, TypeHistory> saveDataEntry = saveData.TypeHistory
                .First(kvp => kvp.Key.Equals(myId));

            //// TODO
            // Don't sanitize a farm animals type by blue/void/brown cow chance/etc. or BFAV 
            // config existence. These checks should be done in the menus, etc.
            ////

            if (saveDataEntry.Key == default(long))
            {
                // Add the animal
                saveData.AddTypeHistory(myId, type, __instance.type.Value);

                return;
            }

            // Grab the new type's data to override if it exists
            KeyValuePair<string, string> contentDataEntry = Api.Content.Load<Dictionary<string, string>>(Helpers.Constants.DataFarmAnimalsContentDirectory)
                .First(kvp => kvp.Key.Equals(saveDataEntry.Value));

            // If the data doesn't exist, 
            if (contentDataEntry.Key == null)
            {
                saveData.RemoveTypeHistory(saveDataEntry.Key);

                // Nothing more to do
                return;
            }

            // Get the new type's data values
            Api.FarmAnimal.UpdateFromData(ref __instance, contentDataEntry);
        }
    }
}
