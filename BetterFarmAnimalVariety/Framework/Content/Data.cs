namespace BetterFarmAnimalVariety.Framework.Content
{
    class Data : Asset
    {
        public static string[] Split(string str)
        {
            return str.Split(Helpers.Constants.DataValueDelimiter);
        }
    }
}
