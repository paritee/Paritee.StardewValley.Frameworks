using System.Collections.Generic;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Cache
{
    class FarmAnimalType
    {
        public string Type;
        public string Data;
        public string BabySprite;
        public string AdultSprite;
        public string ShearedSprite;
        public Dictionary<string, string[]> Localization;

        public enum LocalizationIndex
        {
            DisplayType = 0,
            DisplayBuilding = 1,
        }

        public FarmAnimalType() { }

        public FarmAnimalType(string type)
        {
            this.Type = type;
        }

        public FarmAnimalType(string type, string data, string babySprite, string adultSprite, string shearedSprite, Dictionary<string, string[]> localization)
        {
            this.Type = type;
            this.Data = data;
            this.BabySprite = babySprite;
            this.AdultSprite = adultSprite;
            this.ShearedSprite = shearedSprite;
            this.Localization = localization;
        }

        public string LocalizeData(string locale)
        {
            if (this.Localization == null || !this.Localization.ContainsKey(locale))
            {
                return this.Data;
            }

            string[] values = PariteeCore.Api.Content.ParseDataValue(this.Data);

            values[(int)PariteeCore.Constants.FarmAnimal.DataValueIndex.DisplayType] = this.Localization[locale][(int)Cache.FarmAnimalType.LocalizationIndex.DisplayType];
            values[(int)PariteeCore.Constants.FarmAnimal.DataValueIndex.DisplayBuilding] = this.Localization[locale][(int)Cache.FarmAnimalType.LocalizationIndex.DisplayBuilding];

            return string.Join(PariteeCore.Constants.Content.DataValueDelimiter.ToString(), values);
        }
    }
}
