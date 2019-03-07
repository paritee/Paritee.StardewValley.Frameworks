using StardewModdingAPI;
using System.Collections.Generic;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Events
{
    class IntegrateWithJsonAssets
    {
        private static void AssertApiIsReady(IModHelper helper, out Api.IJsonAssets api)
        {
            // Check if JsonAssets is actully installed ...
            Helpers.Assert.ApiExists(helper, Constants.Integration.JsonAssets, out api);

            // ... and a save has loaded (JA requirement)
            Helpers.Assert.SaveLoaded();
        }

        public static void RefreshFarmAnimalData(IModHelper helper)
        {
            IntegrateWithJsonAssets.AssertApiIsReady(helper, out Api.IJsonAssets api);

            // Invalidate the farm animals due to the JsonAssets integration. Since 
            // the JA IDs are per save, we want to make sure the the replacement is 
            // invalidated for each save load.
            helper.Content.InvalidateCache(PariteeCore.Constants.Content.DataFarmAnimalsContentPath);

            // Load them again to replace the produce values
            helper.Content.Load<Dictionary<string, string>>(PariteeCore.Constants.Content.DataFarmAnimalsContentPath, ContentSource.GameContent);
        }

        public static bool TryParseFarmAnimalProduceName(IModHelper helper, string objectName, out int index)
        {
            index = PariteeCore.Constants.FarmAnimal.NoProduce;

            IntegrateWithJsonAssets.AssertApiIsReady(helper, out Api.IJsonAssets api);

            index = api.GetObjectId(objectName);

            return index >= 0;
        }
    }
}
