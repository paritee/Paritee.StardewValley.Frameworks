using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Paritee.StardewValleyAPI.Buidlings.AnimalShop.FarmAnimals;
using Paritee.StardewValleyAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BetterFarmAnimalVariety.Models
{
    public class ConfigFarmAnimal
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum TypeGroup
        {
            [EnumMember(Value = "Cows")]
            Cow,
            [EnumMember(Value = "Chickens")]
            Chicken,
            [EnumMember(Value = "Sheep")]
            Sheep,
            [EnumMember(Value = "Goats")]
            Goat,
            [EnumMember(Value = "Pigs")]
            Pig,
            [EnumMember(Value = "Ducks")]
            Duck,
            [EnumMember(Value = "Rabbits")]
            Rabbit,
            [EnumMember(Value = "Dinosaurs")]
            Dinosaur
        }

        public const string DEFAULT = "default";
        public const string ANIMAL_SHOP_TO_AREA_X = "X";
        public const string ANIMAL_SHOP_TO_AREA_Y = "Y";
        public const string ANIMAL_SHOP_TO_AREA_WIDTH = "Width";
        public const string ANIMAL_SHOP_TO_AREA_HEIGHT = "Height";

        public ConfigFarmAnimal.TypeGroup Group;
        public string AnimalShopNameID;
        public string AnimalShopDescriptionID;
        public Dictionary<string, int> AnimalShopIconToArea;

        [JsonIgnore]
        public string ConfigName;

        [JsonIgnore]
        public string ConfigDescription;

        [JsonIgnore]
        public string ConfigAnimalShopIcon;

        [JsonProperty(Order = 1)]
        public string Name
        {
            get
            {
                return this.ConfigName ?? ConfigFarmAnimal.DEFAULT;
            }
            set
            {
                this.ConfigName = value;
            }
        }

        [JsonProperty(Order = 2)]
        public string Description
        {
            get
            {
                return this.ConfigDescription ?? ConfigFarmAnimal.DEFAULT;
            }
            set
            {
                this.ConfigDescription = value;
            }
        }

        [JsonProperty(Order = 3)]
        public string ShopIcon
        {
            get
            {
                return this.ConfigAnimalShopIcon ?? ConfigFarmAnimal.DEFAULT;
            }
            set
            {
                this.ConfigAnimalShopIcon = value;
            }
        }

        [JsonProperty(Order = 4)]
        public string[] Types;

        [JsonConstructor]
        public ConfigFarmAnimal()
        {
            // Do nothing; this is for loading an existing config
        }

        public ConfigFarmAnimal(AppSetting appSetting)
        {
            string[] Values = appSetting.SplitValue();
            
            this.Group = this.ConvertStringToTypeGroup(appSetting.SplitKey()[AppSetting.FARMANIMALS_GROUP_INDEX]);
            this.AnimalShopNameID = Values[AppSetting.FARMANIMALS_ANIMAL_SHOP_NAME_ID_INDEX];
            this.AnimalShopDescriptionID = Values[AppSetting.FARMANIMALS_ANIMAL_SHOP_DESCRIPTION_ID_INDEX];

            this.AnimalShopIconToArea = new Dictionary<string, int>() {
                { ConfigFarmAnimal.ANIMAL_SHOP_TO_AREA_X, Int32.Parse(Values[AppSetting.FARMANIMALS_ANIMAL_SHOP_TO_AREA_X_INDEX]) },
                { ConfigFarmAnimal.ANIMAL_SHOP_TO_AREA_Y, Int32.Parse(Values[AppSetting.FARMANIMALS_ANIMAL_SHOP_TO_AREA_Y_INDEX]) },
                { ConfigFarmAnimal.ANIMAL_SHOP_TO_AREA_WIDTH, Int32.Parse(Values[AppSetting.FARMANIMALS_ANIMAL_SHOP_TO_AREA_WIDTH_INDEX]) },
                { ConfigFarmAnimal.ANIMAL_SHOP_TO_AREA_HEIGHT, Int32.Parse(Values[AppSetting.FARMANIMALS_ANIMAL_SHOP_TO_AREA_HEIGHT_INDEX]) },
            };

            this.Types = appSetting.Split(Values[AppSetting.FARMANIMALS_TYPES_INDEX], AppSetting.VALUE_ARRAY_DELIMITER);
        }

        public bool ShouldSerializeGroup()
        {
            return false;
        }

        public bool ShouldSerializeAnimalShopNameID()
        {
            return false;
        }

        public bool ShouldSerializeAnimalShopDescriptionID()
        {
            return false;
        }

        public bool ShouldSerializeAnimalShopIconToArea()
        {
            return false;
        }

        public string DetermineAnimalShopNameKey()
        {
            return "Utility.cs." + this.AnimalShopNameID;
        }

        public string DetermineAnimalShopDescriptionKey()
        {
            return "PurchaseAnimalsMenu.cs." + this.AnimalShopDescriptionID;
        }

        public Rectangle GetAnimalShopIconToArea()
        {
            return new Rectangle(
                this.AnimalShopIconToArea[ConfigFarmAnimal.ANIMAL_SHOP_TO_AREA_X],
                this.AnimalShopIconToArea[ConfigFarmAnimal.ANIMAL_SHOP_TO_AREA_Y],
                this.AnimalShopIconToArea[ConfigFarmAnimal.ANIMAL_SHOP_TO_AREA_WIDTH],
                this.AnimalShopIconToArea[ConfigFarmAnimal.ANIMAL_SHOP_TO_AREA_HEIGHT]
            );
        }

        public string[] GetTypes()
        {
            return this.Types;
        }

        public bool IsDefault(string value)
        {
            return value == ConfigFarmAnimal.DEFAULT;
        }

        public void ResetAnimalShopFields(ConfigFarmAnimal Reset)
        {
            this.AnimalShopNameID = Reset.AnimalShopNameID;
            this.AnimalShopDescriptionID = Reset.AnimalShopDescriptionID;
            this.AnimalShopIconToArea = Reset.AnimalShopIconToArea;
        }

        public Stock.Name GetStockName()
        {
            return this.ConvertTypeGroupToStockName(this.Group);
        }

        private ConfigFarmAnimal.TypeGroup ConvertStringToTypeGroup(string str)
        {
            Array values = Enum.GetValues(typeof(ConfigFarmAnimal.TypeGroup));

            foreach (ConfigFarmAnimal.TypeGroup typeGroup in values)
            {
                string description = Enums.GetValue(typeGroup);

                if (str.Equals(description))
                    return typeGroup;
            }

            throw new Exception();
        }

        private Stock.Name ConvertTypeGroupToStockName(ConfigFarmAnimal.TypeGroup key)
        {
            switch (key)
            {
                case ConfigFarmAnimal.TypeGroup.Cow:
                    return Stock.Name.DairyCow;
                case ConfigFarmAnimal.TypeGroup.Chicken:
                    return Stock.Name.Chicken;
                case ConfigFarmAnimal.TypeGroup.Sheep:
                    return Stock.Name.Sheep;
                case ConfigFarmAnimal.TypeGroup.Goat:
                    return Stock.Name.Goat;
                case ConfigFarmAnimal.TypeGroup.Pig:
                    return Stock.Name.Pig;
                case ConfigFarmAnimal.TypeGroup.Duck:
                    return Stock.Name.Duck;
                case ConfigFarmAnimal.TypeGroup.Rabbit:
                    return Stock.Name.Rabbit;
                default:
                    throw new StockDoesNotExistException();
            }
        }
    }
}
