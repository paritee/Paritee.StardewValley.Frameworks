namespace BetterFarmAnimalVariety.Framework.Constants
{
    class Content
    {
        public const string AnimalsContentDirectory = "Animals";
        public const char DataValueDelimiter = '/';
        public const string None = "none";
        public const int StartingFrame = 0;

        public static string DataFarmAnimalsContentPath { get { return Api.Content.BuildPath(new string[] { "Data", "FarmAnimals" }); } }
        public static string DataBlueprintsContentPath { get { return Api.Content.BuildPath(new string[] { "Data", "Blueprints" }); } }
    }
}
