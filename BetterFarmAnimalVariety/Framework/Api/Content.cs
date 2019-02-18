using StardewValley;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Api
{
    class Content
    {
        public static bool Exists<T>(string name)
        {
            try
            {
                Api.Content.Load<T>(name);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static T Load<T>(string path)
        {
            return Game1.content.Load<T>(path);
        }

        public static string LoadString(string path)
        {
            return Game1.content.LoadString(path);
        }

        public static KeyValuePair<T, U> GetDataEntry<T, U>(Dictionary<T, U> data, T id)
        {
            return data.First(kvp => kvp.Key.Equals(id));
        }

        public static U GetDataValue<T, U>(string path, T id, int index)
        {
            Dictionary<T, U> data = Api.Content.LoadData<T, U>(Constants.Content.DataBlueprintsContentPath);
            KeyValuePair<T, U> entry = Api.Content.GetDataEntry<T, U>(data, id);

            return (U)System.Convert.ChangeType(Api.Content.ParseDataValue(entry.Value.ToString())[index], typeof(U));
        }

        public static KeyValuePair<T, U> LoadDataEntry<T, U>(string path, T id)
        {
            Dictionary<T, U> data = Api.Content.Load<Dictionary<T, U>>(path);

            return Api.Content.GetDataEntry<T, U>(data, id);
        }

        public static Dictionary<T, U> LoadData<T, U>(string path)
        {
            return Api.Content.Load<Dictionary<T, U>>(path);
        }

        public static string BuildPath(string[] parts)
        {
            return Path.Combine(parts);
        }

        public static string[] ParseDataValue(string str)
        {
            return str.Split(Constants.Content.DataValueDelimiter);
        }
    }
}
