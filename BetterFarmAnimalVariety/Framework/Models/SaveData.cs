namespace BetterFarmAnimalVariety.Framework.Models
{
    public class SaveData
    {
        protected string Key;

        protected SaveData(string uniqueModId, string key)
        {
            this.Key = this.FormatFullKey(uniqueModId, key);
        }

        private string FormatFullKey(string uniqueModId, string key)
        {
            return Helpers.GameSave.SmapiSaveDataKey(uniqueModId, key);
        }

        protected T Read<T>()
        {
            return Api.Game.ReadSaveData<T>(this.Key);
        }

        protected void Write(object obj)
        {
            Api.Game.WriteSaveData(this.Key, obj);
        }
    }
}
