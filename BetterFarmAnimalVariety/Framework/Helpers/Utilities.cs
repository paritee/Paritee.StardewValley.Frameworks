using BetterFarmAnimalVariety.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BetterFarmAnimalVariety.Framework.Helpers
{
    // TODO: replicate in API
    class Utilities
    {
        public static T GetReflectedValue<T>(object obj, string field, BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic)
        {
            object value = obj is Type type
                ? type.GetField(field, bindingAttr).GetValue(null)
                : obj.GetType().GetField(field, bindingAttr).GetValue(obj);

            return (T)value;
        }

        public static Multiplayer Multiplayer()
        {
            return Helpers.Utilities.GetReflectedValue<Multiplayer>(typeof(Game1), "multiplayer", BindingFlags.Static | BindingFlags.NonPublic);
        }

        public static StardewValley.Object GetIncubator(AnimalHouse animalHouse)
        {
            foreach (StardewValley.Object @object in animalHouse.objects.Values)
            {
                if (@object.bigCraftable.Value && @object.Name.Contains(Helpers.Constants.Incubator) && (@object.heldObject.Value != null && @object.MinutesUntilReady <= 0) && !animalHouse.isFull())
                {
                    return @object;
                }
            }

            return (StardewValley.Object)null;
        }

        public static List<string> GetTypesFromProduce(int produceId)
        {
            List<string> potentialTypes = new List<string>();

            // TODO: check BFAVs config instead
            // Someone could have the data set up, but not add it to BFAV so that it's hidden from the game
            Dictionary<string, string> contentData = Content.FarmAnimalsData.Load();

            foreach (KeyValuePair<string, string> entry in contentData)
            {
                string[] values = entry.Value.Split('/');

                int defaultProduceId = Int32.Parse(values[(int)Content.FarmAnimalsData.ValueIndex.DefaultProduce]);
                int deluxeProduceId = Int32.Parse(values[(int)Content.FarmAnimalsData.ValueIndex.DeluxeProduce]);

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
            List<string> potentialTypes = Helpers.Utilities.GetTypesFromProduce(produceIndex);

            // Check to make sure types came back
            return potentialTypes.Count >= 1
                ? potentialTypes.ElementAt(Game1.random.Next(potentialTypes.Count - 1))
                : Helpers.Utilities.GetDefaultCoopDwellerType();
        }

        public static string GetRandomTypeFromParent(FarmAnimal parent)
        {
            // TODO: randomize on bfav categories
            return parent.type.Value;
        }

        public static void AddFarmAnimalToBuilding(ref FarmAnimal animal, ref Building building)
        {
            animal.homeLocation.Value = new Vector2((float)building.tileX.Value, (float)building.tileY.Value);
            animal.setRandomPosition(animal.home.indoors.Value);

            AnimalHouse animalHouse = building.indoors.Value as AnimalHouse;

            animalHouse.animals.Add(animal.myID.Value, animal);
            animalHouse.animalsThatLiveHere.Add(animal.myID.Value);
        }

        public static void ResetIncubator(AnimalHouse animalHouse)
        {
            animalHouse.incubatingEgg.X = 0;
            animalHouse.incubatingEgg.Y = -1;
        }

        public static void ResetIncubator(StardewValley.Object incubator, AnimalHouse animalHouse)
        {
            incubator.heldObject.Value = (StardewValley.Object)null;
            incubator.ParentSheetIndex = Helpers.Constants.DefaultIncubatorItem;

            Helpers.Utilities.ResetIncubator(animalHouse);
        }

        public static FarmAnimal CreateFarmAnimal(string type, long ownerId, string name = null, Building home = null, long myId = -1L)
        {
            Multiplayer multiplayer = Helpers.Utilities.Multiplayer();

            myId = myId.Equals(-1L) ? multiplayer.getNewID() : myId;

            return new FarmAnimal(type, myId, ownerId)
            {
                Name = name,
                displayName = name,
                home = home
            };
        }

        public static bool IsVanillaFarmAnimalType(string type)
        {
            return Helpers.VanillaFarmAnimal.Exists(type);
        }

        public static bool HasVanillaFarmAnimalType(FarmAnimal animal)
        {
            return Helpers.Utilities.IsVanillaFarmAnimalType(animal.type.Value);
        }

        public static string GetDefaultCoopDwellerType()
        {
            return VanillaFarmAnimal.WhiteChicken.ToString();
        }

        public static string GetDefaultBarnDwellerType()
        {
            return VanillaFarmAnimal.WhiteCow.ToString();
        }

        public static void OverwriteAnimalFromData(ref FarmAnimal animal, KeyValuePair<string, string> contentDataEntry)
        {
            string[] values = Content.Data.Split(contentDataEntry.Value);

            // Reset the instance's values based on the new type
            animal.type.Value = contentDataEntry.Value;
            animal.daysToLay.Value = Convert.ToByte(values[(int)Content.FarmAnimalsData.ValueIndex.DaysToLay]);
            animal.ageWhenMature.Value = Convert.ToByte(values[(int)Content.FarmAnimalsData.ValueIndex.AgeWhenMature]);
            animal.defaultProduceIndex.Value = Convert.ToInt32(values[(int)Content.FarmAnimalsData.ValueIndex.DefaultProduce]);
            animal.deluxeProduceIndex.Value = Convert.ToInt32(values[(int)Content.FarmAnimalsData.ValueIndex.DeluxeProduce]);
            animal.sound.Value = values[(int)Content.FarmAnimalsData.ValueIndex.Sound].Equals(Helpers.Constants.None) ? null : values[(int)Content.FarmAnimalsData.ValueIndex.Sound];

            int x, y, width, height;

            x = Convert.ToInt32(values[(int)Content.FarmAnimalsData.ValueIndex.FrontBackBoundingBoxX]);
            y = Convert.ToInt32(values[(int)Content.FarmAnimalsData.ValueIndex.FrontBackBoundingBoxY]);
            width = Convert.ToInt32(values[(int)Content.FarmAnimalsData.ValueIndex.FrontBackBoundingBoxWidth]);
            height = Convert.ToInt32(values[(int)Content.FarmAnimalsData.ValueIndex.FrontBackBoundingBoxHeight]);

            animal.frontBackBoundingBox.Value = new Rectangle(x, y, width, height);

            x = Convert.ToInt32(values[(int)Content.FarmAnimalsData.ValueIndex.SidewaysBoundingBoxX]);
            y = Convert.ToInt32(values[(int)Content.FarmAnimalsData.ValueIndex.SidewaysBoundingBoxY]);
            width = Convert.ToInt32(values[(int)Content.FarmAnimalsData.ValueIndex.SidewaysBoundingBoxWidth]);
            height = Convert.ToInt32(values[(int)Content.FarmAnimalsData.ValueIndex.SidewaysBoundingBoxHeight]);

            animal.sidewaysBoundingBox.Value = new Rectangle(x, y, width, height);

            animal.harvestType.Value = Convert.ToByte(values[(int)Content.FarmAnimalsData.ValueIndex.HarvestType]);
            animal.showDifferentTextureWhenReadyForHarvest.Value = Convert.ToBoolean(values[(int)Content.FarmAnimalsData.ValueIndex.ShowDifferentTextureWhenReadyForHarvest]);
            animal.buildingTypeILiveIn.Value = values[(int)Content.FarmAnimalsData.ValueIndex.BuildingTypeILiveIn];

            width = Convert.ToInt32(values[(int)Content.FarmAnimalsData.ValueIndex.SpriteWidth]);
            height = Convert.ToInt32(values[(int)Content.FarmAnimalsData.ValueIndex.SpritHeight]);

            animal.Sprite = new AnimatedSprite(Helpers.Utilities.DetermineSpriteAssetName(animal), Helpers.Constants.StartingFrame, width, height);
            animal.frontBackSourceRect.Value = new Rectangle(0, 0, width, height);

            width = Convert.ToInt32(values[(int)Content.FarmAnimalsData.ValueIndex.SidewaysSourceRectWidth]);
            height = Convert.ToInt32(values[(int)Content.FarmAnimalsData.ValueIndex.SidewaysSourceRectHeight]);

            animal.sidewaysSourceRect.Value = new Rectangle(0, 0, width, height);

            animal.fullnessDrain.Value = Convert.ToByte(values[(int)Content.FarmAnimalsData.ValueIndex.FullnessDrain]);
            animal.happinessDrain.Value = Convert.ToByte(values[(int)Content.FarmAnimalsData.ValueIndex.HappinessDrain]);
            animal.toolUsedForHarvest.Value = values[(int)Content.FarmAnimalsData.ValueIndex.ToolUsedForHarvest].Equals(Helpers.Constants.None) ? null : values[(int)Content.FarmAnimalsData.ValueIndex.ToolUsedForHarvest];
            animal.meatIndex.Value = Convert.ToInt32(values[(int)Content.FarmAnimalsData.ValueIndex.MeatIndex]);
            animal.price.Value = Convert.ToInt32(values[(int)Content.FarmAnimalsData.ValueIndex.Price]);
        }

        public static string BuildContentPath(string[] parts)
        {
            return String.Join(Helpers.Constants.ContentPathDelimiter, parts);
        }

        public static string DetermineSpriteAssetName(FarmAnimal animal)
        {
            string prefix = "";

            if (animal.isBaby())
            {
                prefix = Helpers.Constants.BabyPrefix;
            }
            else if (animal.showDifferentTextureWhenReadyForHarvest.Value && animal.currentProduce.Value <= 0)
            {
                prefix = Helpers.Constants.ShearedPrefix;
            }

            string assetName = prefix + animal.type.Value;

            // Check if there's a baby "duck" asset since vanilla doesn't have one
            if (Helpers.Utilities.AssetExists<Texture2D>(assetName))
            {
                return Helpers.Utilities.BuildContentPath(new string[] { Helpers.Constants.AnimalsContentDirectory, assetName });
            }

            string buildingType = animal.home != null 
                ? animal.home.buildingType.Value 
                : animal.buildingTypeILiveIn.Value;

            assetName = buildingType.Equals(Helpers.Constants.Barn)
                ? Helpers.Utilities.GetDefaultBarnDwellerType()
                : Helpers.Utilities.GetDefaultCoopDwellerType();

            return Helpers.Utilities.BuildContentPath(new string[] { Helpers.Constants.AnimalsContentDirectory, assetName });
        }

        public static bool AssetExists<T>(string name)
        {
            return Content.Asset.Load<T>(name) != null;
        }
    }
}