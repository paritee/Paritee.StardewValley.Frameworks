using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Helpers
{
    class Mod
    {
        public static string SmapiSaveDataKey(string key)
        {
            return PariteeCore.Api.Mod.SmapiSaveDataKey(Constants.Mod.Key, key);
        }

        public static T ReadConfig<T>()
        {
            return PariteeCore.Api.Mod.ReadConfig<T>(PariteeCore.Constants.Mod.Path, Constants.Mod.ConfigFileName);
        }
    }
}