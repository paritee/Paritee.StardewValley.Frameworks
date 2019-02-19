namespace BetterFarmAnimalVariety.Framework.SaveData
{
    class SaveData
    {
        protected static string GetKey(string key)
        {
            return $"smapi/mod-data/{Constants.Mod.Key}/{key}";
        }

        protected T Read<T>(string key)
        {
            return Api.Game.ReadSaveData<T>(key);
        }

        protected void Write(object obj, string key)
        {
            Api.Game.WriteSaveData(key, obj);
        }
    }
}
