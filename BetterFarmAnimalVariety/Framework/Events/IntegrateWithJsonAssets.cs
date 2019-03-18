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
            // Invalidate the farm animals due to the JsonAssets integration. Since 
            // the JA IDs are per save, we want to make sure the the replacement is 
            // invalidated for each save load.
            helper.Content.InvalidateCache(PariteeCore.Utilities.Content.DataFarmAnimalsContentPath);

            // Load them again to replace the produce values
            helper.Content.Load<Dictionary<string, string>>(PariteeCore.Utilities.Content.DataFarmAnimalsContentPath, ContentSource.GameContent);
        }

        public static bool TryParseObjectName(string objectName, out int index)
        {
            return PariteeCore.Objects.Object.TryParse(objectName, out index);
        }
    }
}
