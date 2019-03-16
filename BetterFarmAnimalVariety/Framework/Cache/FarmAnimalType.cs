using System.Collections.Generic;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Cache
{
    class FarmAnimalType
    {
        public string Type;
        public string Data;
        public double DeluxeProduceLuck;
        public FarmAnimalSprites Sprites = new FarmAnimalSprites();
        public Dictionary<string, string[]> Localization = new Dictionary<string, string[]>();

        public enum LocalizationIndex
        {
            DisplayType = 0,
            DisplayBuilding = 1,
        }

        public FarmAnimalType() { }

        public FarmAnimalType(string type, double deluxeProduceLuck)
        {
            this.Type = type;
            this.DeluxeProduceLuck = deluxeProduceLuck;
        }

        public FarmAnimalType(string type, string data, double deluxeProduceLuck, string babySprite, string adultSprite, string readyForHarvestSprite, Dictionary<string, string[]> localization)
        {
            this.Type = type;
            this.Data = data;
            this.DeluxeProduceLuck = deluxeProduceLuck;
            this.Sprites = new FarmAnimalSprites(babySprite, adultSprite, readyForHarvestSprite);
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

        public bool HasLocalization()
        {
            return this.Localization != null && this.Localization.Any();
        }

        public bool HasSprites()
        {
            return this.Sprites != null;
        }

        public bool HasAdultSprite()
        {
            return this.HasSprites() && this.Sprites.Adult != null;
        }

        public bool HasBabySprite()
        {
            return this.HasSprites() && this.Sprites.Baby != null;
        }

        public bool HasReadyForHarvestSprite()
        {
            return this.HasSprites() && this.Sprites.ReadyForHarvest != null;
        }
    }
}
