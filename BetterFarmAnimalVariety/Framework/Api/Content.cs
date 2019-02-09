using StardewValley;
using System;

namespace BetterFarmAnimalVariety.Framework.Api
{
    class Content
    {
        public static bool Exists<T>(string name)
        {
            return Api.Content.Load<T>(name) != null;
        }

        public static T Load<T>(string name)
        {
            return Game1.content.Load<T>(name);
        }

        public static string BuildPath(string[] parts)
        {
            return String.Join(Helpers.Constants.ContentPathDelimiter, parts);
        }

        public static string[] ParseDataValue(string str)
        {
            return str.Split(Helpers.Constants.DataValueDelimiter);
        }
    }
}
