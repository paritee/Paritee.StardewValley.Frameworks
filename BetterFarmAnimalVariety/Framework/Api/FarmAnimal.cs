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
        public static void Reload(ref StardewValley.FarmAnimal animal, Building home)
        {
            animal.reload(home);
        }

        public static void ReloadAll()
        {
            for (int index = 0; index < Game1.locations.Count; ++index)
            {
                if (!(Game1.locations[index] is Farm farm))
                {
                    continue;
                }

                for (int j = 0; j < farm.buildings.Count; ++j)
                {
                    if (!(farm.buildings[j].indoors.Value is StardewValley.AnimalHouse animalHouse))
                    {
                        continue;
                    }

                    for (int k = 0; k < animalHouse.animalsThatLiveHere.Count(); ++k)
                    {
                        long id = animalHouse.animalsThatLiveHere.ElementAt(k);
                        StardewValley.FarmAnimal animal = animalHouse.animals[id];

                        Api.FarmAnimal.Reload(ref animal, animal.home);
                    }
                }

                break;
            }
        }

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
                prefix = Constants.FarmAnimal.BabyPrefix;
            }
            else if (Api.FarmAnimal.IsSheared(animal))
            {
                prefix = Constants.FarmAnimal.ShearedPrefix;
            }

            string assetName = prefix + animal.type.Value;

            // Check if the asset exists (ex. vanilla fails on BabyDuck)
            if (!Api.Content.Exists<Texture2D>(Api.Content.BuildPath(new string[] { Constants.Content.AnimalsContentDirectory, assetName })))
            {
                // Covers the BabyDuck scenario by using BabyWhite Chicken
                assetName = Api.FarmAnimal.GetDefaultType(animal);
            }

            return Api.Content.BuildPath(new string[] { Constants.Content.AnimalsContentDirectory, assetName });
        }

        public static AnimatedSprite CreateSprite(StardewValley.FarmAnimal animal)
        {
            return new AnimatedSprite(Api.FarmAnimal.BuildSpriteAssetName(animal), Constants.Content.StartingFrame, animal.frontBackSourceRect.Width, animal.frontBackSourceRect.Height);
        }

        public static StardewValley.FarmAnimal CreateFarmAnimal(string type, long ownerId, string name = null, Building home = null, long myId = default(long))
        {
            myId = myId.Equals(default(long)) ? Api.Game.GetNewId() : myId;

            StardewValley.FarmAnimal animal = new StardewValley.FarmAnimal(type, myId, ownerId)
            {
                Name = name,
                displayName = name,
                home = home
            };

            return animal;
        }

        public static bool IsVanilla(string type)
        {
            return Constants.VanillaFarmAnimalType.Exists(type);
        }

        public static bool IsCoopDweller(StardewValley.FarmAnimal animal)
        {
            string buildingType = animal.home != null
                ? animal.home.buildingType.Value
                : animal.buildingTypeILiveIn.Value;

            return buildingType.Contains(Constants.AnimalHouse.Coop);
        }

        public static string GetDefaultType(string buildingType)
        {
            return Api.FarmAnimal.GetDefaultType(buildingType.Equals(Constants.AnimalHouse.Coop));
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
            return Constants.VanillaFarmAnimalType.WhiteChicken.ToString();
        }

        public static string GetDefaultBarnDwellerType()
        {
            return Constants.VanillaFarmAnimalType.WhiteCow.ToString();
        }

        public static void UpdateFromData(ref StardewValley.FarmAnimal animal, KeyValuePair<string, string> contentDataEntry)
        {
            string[] values = Api.Content.ParseDataValue(contentDataEntry.Value);

            // Reset the instance's values based on the new type
            animal.type.Value = contentDataEntry.Key;
            animal.daysToLay.Value = Convert.ToByte(values[(int)Constants.FarmAnimal.DataValueIndex.DaysToLay]);
            animal.ageWhenMature.Value = Convert.ToByte(values[(int)Constants.FarmAnimal.DataValueIndex.AgeWhenMature]);
            animal.defaultProduceIndex.Value = Convert.ToInt32(values[(int)Constants.FarmAnimal.DataValueIndex.DefaultProduce]);
            animal.deluxeProduceIndex.Value = Convert.ToInt32(values[(int)Constants.FarmAnimal.DataValueIndex.DeluxeProduce]);
            animal.sound.Value = Api.FarmAnimal.IsDataValueNull(values[(int)Constants.FarmAnimal.DataValueIndex.Sound]) ? null : values[(int)Constants.FarmAnimal.DataValueIndex.Sound];

            int x, y, width, height;

            x = Convert.ToInt32(values[(int)Constants.FarmAnimal.DataValueIndex.FrontBackBoundingBoxX]);
            y = Convert.ToInt32(values[(int)Constants.FarmAnimal.DataValueIndex.FrontBackBoundingBoxY]);
            width = Convert.ToInt32(values[(int)Constants.FarmAnimal.DataValueIndex.FrontBackBoundingBoxWidth]);
            height = Convert.ToInt32(values[(int)Constants.FarmAnimal.DataValueIndex.FrontBackBoundingBoxHeight]);

            animal.frontBackBoundingBox.Value = new Rectangle(x, y, width, height);

            x = Convert.ToInt32(values[(int)Constants.FarmAnimal.DataValueIndex.SidewaysBoundingBoxX]);
            y = Convert.ToInt32(values[(int)Constants.FarmAnimal.DataValueIndex.SidewaysBoundingBoxY]);
            width = Convert.ToInt32(values[(int)Constants.FarmAnimal.DataValueIndex.SidewaysBoundingBoxWidth]);
            height = Convert.ToInt32(values[(int)Constants.FarmAnimal.DataValueIndex.SidewaysBoundingBoxHeight]);

            animal.sidewaysBoundingBox.Value = new Rectangle(x, y, width, height);

            animal.harvestType.Value = Convert.ToByte(values[(int)Constants.FarmAnimal.DataValueIndex.HarvestType]);
            animal.showDifferentTextureWhenReadyForHarvest.Value = Convert.ToBoolean(values[(int)Constants.FarmAnimal.DataValueIndex.ShowDifferentTextureWhenReadyForHarvest]);
            animal.buildingTypeILiveIn.Value = values[(int)Constants.FarmAnimal.DataValueIndex.BuildingTypeILiveIn];

            width = Convert.ToInt32(values[(int)Constants.FarmAnimal.DataValueIndex.SpriteWidth]);
            height = Convert.ToInt32(values[(int)Constants.FarmAnimal.DataValueIndex.SpritHeight]);

            animal.Sprite = new AnimatedSprite(Api.FarmAnimal.BuildSpriteAssetName(animal), Constants.Content.StartingFrame, width, height);
            animal.frontBackSourceRect.Value = new Rectangle(0, 0, width, height);

            width = Convert.ToInt32(values[(int)Constants.FarmAnimal.DataValueIndex.SidewaysSourceRectWidth]);
            height = Convert.ToInt32(values[(int)Constants.FarmAnimal.DataValueIndex.SidewaysSourceRectHeight]);

            animal.sidewaysSourceRect.Value = new Rectangle(0, 0, width, height);

            animal.fullnessDrain.Value = Convert.ToByte(values[(int)Constants.FarmAnimal.DataValueIndex.FullnessDrain]);
            animal.happinessDrain.Value = Convert.ToByte(values[(int)Constants.FarmAnimal.DataValueIndex.HappinessDrain]);
            animal.toolUsedForHarvest.Value = Api.FarmAnimal.IsDataValueNull(values[(int)Constants.FarmAnimal.DataValueIndex.ToolUsedForHarvest]) ? "" : values[(int)Constants.FarmAnimal.DataValueIndex.ToolUsedForHarvest];
            animal.meatIndex.Value = Convert.ToInt32(values[(int)Constants.FarmAnimal.DataValueIndex.MeatIndex]);
            animal.price.Value = Convert.ToInt32(values[(int)Constants.FarmAnimal.DataValueIndex.Price]);
        }

        private static bool IsDataValueNull(string value)
        {
            return value.Equals(null) || value.Equals("null") || value.Equals(default(string)) || value.Equals("") || value.Equals(Constants.Content.None);
        }

        public static List<string> GetTypesFromProduce(int[] produceIndexes, Dictionary<string, List<string>> restrictions)
        {
            List<string> potentialCategories = new List<string>();
            List<string> potentialTypes = new List<string>();

            // Someone could have the data set up, but not add it to BFAV so that
            // it's hidden from the game so we must use BFAV's restrictions
            Dictionary<string, string> contentData = Api.Content.LoadData<string, string>(Constants.Content.DataFarmAnimalsContentPath);
            
            foreach (KeyValuePair<string, List<string>> entry in restrictions)
            {
                foreach (string type in entry.Value)
                {
                    string[] values = Api.Content.ParseDataValue(contentData[type]);

                    int defaultProduceId = Int32.Parse(values[(int)Constants.FarmAnimal.DataValueIndex.DefaultProduce]);
                    int deluxeProduceId = Int32.Parse(values[(int)Constants.FarmAnimal.DataValueIndex.DeluxeProduce]);

                    if (Api.FarmAnimal.ProducesAtLeastOne(defaultProduceId, deluxeProduceId, produceIndexes))
                    {
                        potentialTypes.Add(type);
                        potentialCategories.Add(entry.Key);
                    }
                }
            }

            return potentialTypes;
        }

        public static bool ProducesAll(StardewValley.FarmAnimal animal, int[] targets)
        {
            // Intersection length should match target length
            return Api.FarmAnimal.ProducesAll(animal.defaultProduceIndex.Value, animal.deluxeProduceIndex.Value, targets);
        }

        public static bool ProducesAll(int defaultProduceId, int deluxeProduceId, int[] targets)
        {
            int[] produceIndexes = new int[] { defaultProduceId, deluxeProduceId };

            // Intersection length should not change
            return produceIndexes.Intersect(targets)
                .Count().Equals(produceIndexes.Length);
        }

        public static bool ProducesAtLeastOne(StardewValley.FarmAnimal animal, int[] targets)
        {
            return Api.FarmAnimal.ProducesAtLeastOne(animal.defaultProduceIndex.Value, animal.deluxeProduceIndex.Value, targets);
        }

        public static bool ProducesAtLeastOne(int defaultProduceId, int deluxeProduceId, int[] targets)
        {
            // Must actualy be a product
            return targets.Where(o => !o.Equals(Constants.FarmAnimal.NoProduce))
                .Intersect(new int[] { defaultProduceId, deluxeProduceId })
                .Any();
        }

        public static bool ProducesNothing(StardewValley.FarmAnimal animal)
        {
            // This is to support "male" animals being hatched/born from their 
            // "produce". Since they have a harvest type that requires a tool, 
            // but none specified, they cannot produce those items.
            return Api.FarmAnimal.RequiresToolForHarvest(animal) 
                && Api.FarmAnimal.GetToolUsedForHarvest(animal).Equals(default(string));
        }

        public static bool ProducesNothing(int harvestType, string harvestTool)
        {
            // This is to support "male" animals being hatched/born from their 
            // "produce". Since they have a harvest type that requires a tool, 
            // but none specified, they cannot produce those items.
            return Api.FarmAnimal.RequiresToolForHarvest(harvestType)
                && (Api.FarmAnimal.IsDataValueNull(harvestTool));
        }

        // TODO:
        // - This is not used or tested
        // - Potentially add sex consideration in a future update
        // - Should be moved to a FarmAnimal.isMale() patch
        // - Check if partial produce matches make sense; no current use cases
        public static bool IsMale(StardewValley.FarmAnimal animal)
        {
            // Any animal that follows the produces nothing BFAV rules should be 
            // assumed as male since this pattern is not commonly used
            if (Api.FarmAnimal.ProducesNothing(animal))
            {
                return true;
            }

            // Produce of the animal we're going to match against
            int[] targetProduce = new int[] {
                animal.defaultProduceIndex.Value,
                animal.deluxeProduceIndex.Value
            };

            // Check if any other animals exist that do produce nothing, but have the same produce indexes
            Dictionary<string, string> data = Api.Content.LoadData<string, string>(Constants.Content.DataFarmAnimalsContentPath);

            foreach (KeyValuePair<string, string> entry in data)
            {
                string[] values = Api.Content.ParseDataValue(entry.Value);

                int defaultProduce = Convert.ToInt32(values[(int)Constants.FarmAnimal.DataValueIndex.DefaultProduce]);
                int deluxeProduce = Convert.ToInt32(values[(int)Constants.FarmAnimal.DataValueIndex.DeluxeProduce]);

                // Only check against animals that completely match the produce
                if (!Api.FarmAnimal.ProducesAll(defaultProduce, deluxeProduce, targetProduce))
                {
                    continue;
                }

                int harvestType = Convert.ToInt32(values[(int)Constants.FarmAnimal.DataValueIndex.HarvestType]);
                string harvestTool = values[(int)Constants.FarmAnimal.DataValueIndex.ToolUsedForHarvest];

                // Assume that since the source animal produces something and there's 
                // another animal that does not produce anything, it's a female
                if (Api.FarmAnimal.ProducesNothing(harvestType, harvestTool))
                {
                    return false;
                }
            }

            // If no other animal exists that produces nothing, then use their 
            // ID to assign them a sex. This is the same rule used for rabbits 
            // and pigs.
            return animal.myID.Value % 2L == 0L;
        }

        public static string GetRandomTypeFromProduce(int[] produceIndexes, Dictionary<string, List<string>> restrictions)
        {
            List<string> potentialTypes = Api.FarmAnimal.GetTypesFromProduce(produceIndexes, restrictions);

            int index = Helpers.Random.Next(potentialTypes.Count);

            // Check to make sure types came back
            return potentialTypes.Any()
                ? potentialTypes[index]
                : null;
        }

        public static string GetRandomTypeFromProduce(int produceIndex, Dictionary<string, List<string>> restrictions)
        {
            return Api.FarmAnimal.GetRandomTypeFromProduce(new int[] { produceIndex }, restrictions);
        }

        public static string GetRandomTypeFromParent(StardewValley.FarmAnimal parent, Dictionary<string, List<string>> restrictions)
        {
            // Use the parent's produce to find other potentials
            return GetRandomTypeFromProduce(new int[] { parent.defaultProduceIndex.Value, parent.defaultProduceIndex.Value }, restrictions);
        }

        public static bool CanLiveIn(StardewValley.FarmAnimal animal, Building building)
        {
            return building.buildingType.Value.Contains(animal.buildingTypeILiveIn.Value);
        }

        public static void SetHome(ref StardewValley.FarmAnimal animal, Building home)
        {
            animal.home = home;
            animal.homeLocation.Value = home == null ? default(Vector2) : new Vector2(home.tileX.Value, home.tileY.Value);
        }

        public static bool SetRandomPositionInHome(ref StardewValley.FarmAnimal animal)
        {
            if (animal.home == null)
            {
                return false;
            }

            animal.setRandomPosition(animal.home.indoors.Value);

            return true;
        }

        public static void AddToBuilding(ref StardewValley.FarmAnimal animal, ref Building building)
        {
            Api.FarmAnimal.SetHome(ref animal, building);
            Api.FarmAnimal.SetRandomPositionInHome(ref animal);
            Api.AnimalHouse.AddAnimal(ref building, animal);
        }

        public static void AssociateParent(ref StardewValley.FarmAnimal animal, long parentId)
        {
            animal.parentId.Value = parentId;
        }

        public static bool BlueChickenIsUnlocked(StardewValley.Farmer farmer)
        {
            return Api.Farmer.HasSeenEvent(farmer, Constants.Event.BlueChicken);
        }

        public static bool RollBlueChickenChance(StardewValley.Farmer farmer)
        {
            if (!Api.FarmAnimal.BlueChickenIsUnlocked(farmer))
            {
                return false;
            }

            return Helpers.Random.NextDouble() >= Constants.FarmAnimal.BlueChickenChance;
        }

        public static List<string> SanitizeBlueChickens(List<string> types, StardewValley.Farmer farmer)
        {
            // Sanitize for blue chickens
            string blueChicken = Constants.VanillaFarmAnimalType.BlueChicken.ToString();

            // Check for blue chicken chance
            if (types.Contains(blueChicken) && !Api.AnimalShop.IsBlueChickenAvailableForPurchase(farmer))
            {
                types.Remove(blueChicken);
            }

            return types;
        }

        public static bool HasHarvestType(StardewValley.FarmAnimal animal, int harvestType)
        {
            return animal.harvestType.Value.Equals(harvestType);
        }

        public static bool HasHarvestType(int harvestType, int target)
        {
            return harvestType.Equals(target);
        }

        public static bool CanBeNamed(StardewValley.FarmAnimal animal)
        {
            // "It" harvest type doesn't allow you to name the animal. This is 
            // mostly unused and is only seen on the Hog
            return Api.FarmAnimal.HasHarvestType(animal, Constants.FarmAnimal.ItHarvestType);
        }

        public static bool RequiresToolForHarvest(StardewValley.FarmAnimal animal)
        {
            // "It" harvest type doesn't allow you to name the animal. This is 
            // mostly unused and is only seen on the Hog
            return Api.FarmAnimal.HasHarvestType(animal, Constants.FarmAnimal.RequiresToolHarvestType);
        }

        public static bool RequiresToolForHarvest(int harvestType)
        {
            // "It" harvest type doesn't allow you to name the animal. This is 
            // mostly unused and is only seen on the Hog
            return Api.FarmAnimal.HasHarvestType(harvestType, Constants.FarmAnimal.RequiresToolHarvestType);
        }

        public static bool MakesSound(StardewValley.FarmAnimal animal)
        {
            return animal.sound.Value != null;
        }

        public static string GetToolUsedForHarvest(StardewValley.FarmAnimal animal)
        {
            return animal.toolUsedForHarvest.Value.Length > 0 ? animal.toolUsedForHarvest.Value : default(string);
        }
    }
}
