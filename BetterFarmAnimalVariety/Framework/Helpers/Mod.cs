using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StardewModdingAPI;
using System.IO;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Helpers
{
    class Mod
    {
        public static T ReadSaveData<T>(string saveDataKey) where T: new()
        {
            return PariteeCore.Api.Mod.ReadSaveData<T>(Constants.Mod.Key, saveDataKey);
        }

        public static void WriteSaveData<T>(string saveDataKey, T data)
        {
            PariteeCore.Api.Mod.WriteSaveData<T>(Constants.Mod.Key, saveDataKey, data);
        }

        public static T ReadConfig<T>() where T: new()
        {
            return PariteeCore.Api.Mod.ReadConfig<T>(PariteeCore.Constants.Mod.Path, Constants.Mod.ConfigFileName);
        }

        public static void WriteCache<T>(string cacheFilePath, T data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            string path = Path.Combine(Constants.Mod.CacheFullPath, cacheFilePath);

            System.IO.FileInfo file = new System.IO.FileInfo(path);
            file.Directory.Create(); // If the directory already exists, this method does nothing.

            File.WriteAllText(path, json);
        }

        public static T ReadCache<T>(string cacheFilePath) where T: new()
        {
            string path = Path.Combine(Constants.Mod.CacheFullPath, cacheFilePath);

            if (!File.Exists(path))
            {
                return new T();
            }

            string json = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<T>(json);
        }

        public static Texture2D LoadTexture(string filePath)
        {
            return PariteeCore.Api.Mod.LoadTexture(Path.Combine(PariteeCore.Constants.Mod.Path, filePath));
        }

        public static string GetShortAssetPath(string filePath)
        {
            return Path.Combine(Constants.Mod.AssetsDirectory, filePath);
        }

        public static bool TryGetApi<TInterface>(IModHelper helper, string key, out TInterface api) where TInterface : class
        {
            api = helper.ModRegistry.GetApi<TInterface>(key);

            return api != null;
        }
    }
}