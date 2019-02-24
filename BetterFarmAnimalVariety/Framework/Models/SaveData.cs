namespace BetterFarmAnimalVariety.Framework.Models
{
    public class SaveData
    {
        protected void Write<T>(string key, T data)
        {
            Helpers.Mod.WriteSaveData<T>(key, data);
        }
    }
}
