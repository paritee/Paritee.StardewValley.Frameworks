using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Helpers
{
    class Config
    {
        public static ModConfig Load()
        {
            string path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, Helpers.Constants.ConfigFileName);
            string json = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<ModConfig>(json);
        }

        public static Dictionary<string, List<string>> ExtractTypesByCategory(ModConfig config)
        {
            return config.FarmAnimals.ToDictionary(kvp => kvp.Key, kvp => new List<string>(kvp.Value.Types));
        }
    }
}
