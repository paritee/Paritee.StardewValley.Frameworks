using Newtonsoft.Json;
using System.IO;

namespace BetterFarmAnimalVariety.Framework.Data
{
    class SaveData
    {
        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        public static T Deserialize<T>(string path)
        {
            return SaveData.Exists(path) 
                ? JsonConvert.DeserializeObject<T>(path)
                : default(T);
        }

        protected void WriteChanges(object obj, string filePath)
        {
            // Serialize back to json
            string json = JsonConvert.SerializeObject(obj);

            // Will need to attempt to create the directory if it doesn't exist
            FileInfo file = new System.IO.FileInfo(filePath);

            // If the directory already exists, this method does nothing.
            file.Directory.Create();

            // Write back to the data file
            File.WriteAllText(file.FullName, json);
        }
    }
}
