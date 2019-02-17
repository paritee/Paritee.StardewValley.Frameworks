namespace BetterFarmAnimalVariety.Framework.Constants
{
    class Config
    {
        public const string FileName = "config.json";
        public const int AnimalShopPricePlaceholder = 1000;
        public static string AnimalShopDescriptionPlaceholder { get { return Api.Content.LoadString("Strings\\StringsFromCSFiles:BluePrint.cs.1"); } }
    }
}
