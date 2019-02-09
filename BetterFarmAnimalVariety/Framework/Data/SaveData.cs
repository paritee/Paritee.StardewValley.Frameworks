using Newtonsoft.Json;
using System.IO;

namespace BetterFarmAnimalVariety.Framework.Data
{
    class SaveData
    {
        public static T Deserialize<T>(string path)
        {
            return JsonConvert.DeserializeObject<T>(path);
        }

        protected void WriteChanges(object obj, string path)
        {
            // Serialize back to json
            string json = JsonConvert.SerializeObject(obj);

            // Write back to the data file
            File.WriteAllText(path, json);
        }
    }
}
