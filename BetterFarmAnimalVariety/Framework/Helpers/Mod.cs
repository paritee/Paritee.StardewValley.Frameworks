namespace BetterFarmAnimalVariety.Framework.Helpers
{
    class Mod
    {
        public static string SmapiSaveDataKey(string key)
        {
            return Api.Mod.SmapiSaveDataKey(Constants.Mod.Key, key);
        }

        public static T ReadConfig<T>()
        {
            return Api.Mod.ReadConfig<T>(Constants.Mod.Path, Constants.Mod.ConfigFileName);
        }
    }
}
