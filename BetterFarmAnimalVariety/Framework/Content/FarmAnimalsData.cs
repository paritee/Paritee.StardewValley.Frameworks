using System.Collections.Generic;

namespace BetterFarmAnimalVariety.Framework.Content
{
    class FarmAnimalsData : Data
    {
        public enum ValueIndex
        {
            DaysToLay = 0,
            AgeWhenMature = 1,
            DefaultProduce = 2,
            DeluxeProduce = 3,
            Sound = 4,
            FrontBackBoundingBoxX = 5,
            FrontBackBoundingBoxY = 6,
            FrontBackBoundingBoxWidth = 7,
            FrontBackBoundingBoxHeight = 8,
            SidewaysBoundingBoxX = 9,
            SidewaysBoundingBoxY = 10,
            SidewaysBoundingBoxWidth = 11,
            SidewaysBoundingBoxHeight = 12,
            HarvestType = 13,
            ShowDifferentTextureWhenReadyForHarvest = 14,
            BuildingTypeILiveIn = 15,
            SpriteWidth = 16,
            SpritHeight = 17,
            SidewaysSourceRectWidth = 18,
            SidewaysSourceRectHeight = 19,
            FullnessDrain = 20,
            HappinessDrain = 21,
            ToolUsedForHarvest = 22,
            MeatIndex = 23,
            Price = 24
        }

        public static int GetIndex(ValueIndex index)
        {
            return (int)index;
        }

        public static Dictionary<string, string> Load()
        {
            string filename = Helpers.Utilities.BuildContentPath(new string[] { Helpers.Constants.DataContentDirectory, Helpers.Constants.DataFarmAnimalsContentDirectory });
            return Asset.Load<Dictionary<string, string>>(filename);
        }
    }
}
