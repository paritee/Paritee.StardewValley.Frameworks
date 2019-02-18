using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace BetterFarmAnimalVariety.Framework.Helpers
{
    class Mod
    {
        public static string GetPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public static T LoadConfig<T>()
        {
            string path = Path.Combine(Constants.Mod.Path, Constants.Config.FileName);
            string json = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
