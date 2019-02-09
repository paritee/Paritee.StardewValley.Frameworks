using BetterFarmAnimalVariety.Framework.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Api
{
    class FarmAnimal
    {
        public static bool IsBaby(StardewValley.FarmAnimal animal)
        {
            return animal.isBaby();
        }

        public static bool IsSheared(StardewValley.FarmAnimal animal)
        {
            return animal.showDifferentTextureWhenReadyForHarvest.Value && animal.currentProduce.Value <= 0;
        }

        public static string BuildSpriteAssetName(StardewValley.FarmAnimal animal)
        {
            string prefix = "";

            if (Api.FarmAnimal.IsBaby(animal))
            {
                prefix = Helpers.Constants.BabyPrefix;
            }
            else if (Api.FarmAnimal.IsSheared(animal))
            {
                prefix = Helpers.Constants.ShearedPrefix;
            }

            string assetName = prefix + animal.type.Value;

            // Check if the asset exists (ex. vanilla fails on BabyDuck)
            if (!Api.Content.Exists<Texture2D>(assetName))
            {
                // Covers the BabyDuck scenario by using BabyWhite Chicken
                assetName = Api.FarmAnimal.GetDefaultType(animal);
            }

            return Api.Content.BuildPath(new string[] { Helpers.Constants.AnimalsContentDirectory, assetName });
        }

        public static AnimatedSprite CreateSprite(StardewValley.FarmAnimal animal)
        {
            return new AnimatedSprite(Api.FarmAnimal.BuildSpriteAssetName(animal), Helpers.Constants.StartingFrame, animal.frontBackSourceRect.Width, animal.frontBackSourceRect.Height);
        }

        public static StardewValley.FarmAnimal CreateFarmAnimal(string type, long ownerId, string name = null, Building home = null, long myId = default(long))
        {
            myId = myId.Equals(default(long)) ? Helpers.Multiplayer.GetNewId() : myId;

            return new StardewValley.FarmAnimal(type, myId, ownerId)
            {
                Name = name,
                displayName = name,
                home = home
            };
        }

        public static bool IsVanilla(string type)
        {
            return Helpers.VanillaFarmAnimal.Exists(type);
        }

        public static bool IsCoopDweller(StardewValley.FarmAnimal animal)
        {
            string buildingType = animal.home != null
                ? animal.home.buildingType.Value
                : animal.buildingTypeILiveIn.Value;

            return buildingType.Equals(Helpers.Constants.Coop);
        }

        public static string GetDefaultType(string buildingType)
        {
            return Api.FarmAnimal.GetDefaultType(buildingType.Equals(Helpers.Constants.Coop));
        }

        public static string GetDefaultType(StardewValley.FarmAnimal animal)
        {
            return Api.FarmAnimal.GetDefaultType(Api.FarmAnimal.IsCoopDweller(animal));
        }

        public static string GetDefaultType(bool isCoop)
        {
            return isCoop
                ? Api.FarmAnimal.GetDefaultCoopDwellerType()
                : Api.FarmAnimal.GetDefaultBarnDwellerType();
        }

        public static string GetDefaultCoopDwellerType()
        {
            return VanillaFarmAnimal.WhiteChicken.ToString();
        }

        public static string GetDefaultBarnDwellerType()
        {
            return VanillaFarmAnimal.WhiteCow.ToString();
        }

        public static void UpdateFromData(ref StardewValley.FarmAnimal animal, KeyValuePair<string, string> contentDataEntry)
        {
            string[] values = Api.Content.ParseDataValue(contentDataEntry.Value);

            // Reset the instance's values based on the new type
            animal.type.Value = contentDataEntry.Value;
            animal.daysToLay.Value = Convert.ToByte(values[(int)Helpers.Data.FarmAnimalsIndex.DaysToLay]);
            animal.ageWhenMature.Value = Convert.ToByte(values[(int)Helpers.Data.FarmAnimalsIndex.AgeWhenMature]);
            animal.defaultProduceIndex.Value = Convert.ToInt32(values[(int)Helpers.Data.FarmAnimalsIndex.DefaultProduce]);
            animal.deluxeProduceIndex.Value = Convert.ToInt32(values[(int)Helpers.Data.FarmAnimalsIndex.DeluxeProduce]);
            animal.sound.Value = values[(int)Helpers.Data.FarmAnimalsIndex.Sound].Equals(Helpers.Constants.None) ? null : values[(int)Helpers.Data.FarmAnimalsIndex.Sound];

            int x, y, width, height;

            x = Convert.ToInt32(values[(int)Helpers.Data.FarmAnimalsIndex.FrontBackBoundingBoxX]);
            y = Convert.ToInt32(values[(int)Helpers.Data.FarmAnimalsIndex.FrontBackBoundingBoxY]);
            width = Convert.ToInt32(values[(int)Helpers.Data.FarmAnimalsIndex.FrontBackBoundingBoxWidth]);
            height = Convert.ToInt32(values[(int)Helpers.Data.FarmAnimalsIndex.FrontBackBoundingBoxHeight]);

            animal.frontBackBoundingBox.Value = new Rectangle(x, y, width, height);

            x = Convert.ToInt32(values[(int)Helpers.Data.FarmAnimalsIndex.SidewaysBoundingBoxX]);
            y = Convert.ToInt32(values[(int)Helpers.Data.FarmAnimalsIndex.SidewaysBoundingBoxY]);
            width = Convert.ToInt32(values[(int)Helpers.Data.FarmAnimalsIndex.SidewaysBoundingBoxWidth]);
            height = Convert.ToInt32(values[(int)Helpers.Data.FarmAnimalsIndex.SidewaysBoundingBoxHeight]);

            animal.sidewaysBoundingBox.Value = new Rectangle(x, y, width, height);

            animal.harvestType.Value = Convert.ToByte(values[(int)Helpers.Data.FarmAnimalsIndex.HarvestType]);
            animal.showDifferentTextureWhenReadyForHarvest.Value = Convert.ToBoolean(values[(int)Helpers.Data.FarmAnimalsIndex.ShowDifferentTextureWhenReadyForHarvest]);
            animal.buildingTypeILiveIn.Value = values[(int)Helpers.Data.FarmAnimalsIndex.BuildingTypeILiveIn];

            width = Convert.ToInt32(values[(int)Helpers.Data.FarmAnimalsIndex.SpriteWidth]);
            height = Convert.ToInt32(values[(int)Helpers.Data.FarmAnimalsIndex.SpritHeight]);

            animal.Sprite = new AnimatedSprite(Api.FarmAnimal.BuildSpriteAssetName(animal), Helpers.Constants.StartingFrame, width, height);
            animal.frontBackSourceRect.Value = new Rectangle(0, 0, width, height);

            width = Convert.ToInt32(values[(int)Helpers.Data.FarmAnimalsIndex.SidewaysSourceRectWidth]);
            height = Convert.ToInt32(values[(int)Helpers.Data.FarmAnimalsIndex.SidewaysSourceRectHeight]);

            animal.sidewaysSourceRect.Value = new Rectangle(0, 0, width, height);

            animal.fullnessDrain.Value = Convert.ToByte(values[(int)Helpers.Data.FarmAnimalsIndex.FullnessDrain]);
            animal.happinessDrain.Value = Convert.ToByte(values[(int)Helpers.Data.FarmAnimalsIndex.HappinessDrain]);
            animal.toolUsedForHarvest.Value = values[(int)Helpers.Data.FarmAnimalsIndex.ToolUsedForHarvest].Equals(Helpers.Constants.None) ? null : values[(int)Helpers.Data.FarmAnimalsIndex.ToolUsedForHarvest];
            animal.meatIndex.Value = Convert.ToInt32(values[(int)Helpers.Data.FarmAnimalsIndex.MeatIndex]);
            animal.price.Value = Convert.ToInt32(values[(int)Helpers.Data.FarmAnimalsIndex.Price]);
        }

        public static List<string> GetTypesFromProduce(int produceId)
        {
            List<string> potentialTypes = new List<string>();

            // TODO: check BFAVs config instead
            // Someone could have the data set up, but not add it to BFAV so that it's hidden from the game
            Dictionary<string, string> contentData = Api.Content.Load<Dictionary<string, string>>(Helpers.Constants.DataFarmAnimalsContentDirectory);

            foreach (KeyValuePair<string, string> entry in contentData)
            {
                string[] values = Api.Content.ParseDataValue(entry.Value);

                int defaultProduceId = Int32.Parse(values[(int)Helpers.Data.FarmAnimalsIndex.DefaultProduce]);
                int deluxeProduceId = Int32.Parse(values[(int)Helpers.Data.FarmAnimalsIndex.DeluxeProduce]);

                if (produceId.Equals(defaultProduceId) || produceId.Equals(deluxeProduceId))
                {
                    potentialTypes.Add(entry.Key);
                }
            }

            // TODO: bfav category search

            return potentialTypes;
        }

        public static string GetRandomTypeFromProduce(int produceIndex)
        {
            List<string> potentialTypes = Api.FarmAnimal.GetTypesFromProduce(produceIndex);

            // Check to make sure types came back
            return potentialTypes.Count >= 1
                ? potentialTypes.ElementAt(Game1.random.Next(potentialTypes.Count - 1))
                : null;
        }

        public static string GetRandomTypeFromParent(StardewValley.FarmAnimal parent)
        {
            // TODO: randomize on bfav categories
            return parent.type.Value;
        }

        public static void AddToBuilding(ref StardewValley.FarmAnimal animal, ref Building building)
        {
            animal.homeLocation.Value = new Vector2((float)building.tileX.Value, (float)building.tileY.Value);
            animal.setRandomPosition(animal.home.indoors.Value);

            StardewValley.AnimalHouse animalHouse = building.indoors.Value as StardewValley.AnimalHouse;

            animalHouse.animals.Add(animal.myID.Value, animal);
            animalHouse.animalsThatLiveHere.Add(animal.myID.Value);
        }

        public static void AssociateParent(ref StardewValley.FarmAnimal animal, long parentId)
        {
            animal.parentId.Value = parentId;
        }
    }
}
