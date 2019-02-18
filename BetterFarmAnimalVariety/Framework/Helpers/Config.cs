using Newtonsoft.Json;
using System.IO;

namespace BetterFarmAnimalVariety.Framework.Helpers
{
    class Config
    {
        public static T Load<T>()
        {
            string path = Path.Combine(Constants.Mod.ModPath, Constants.Config.FileName);
            string json = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
