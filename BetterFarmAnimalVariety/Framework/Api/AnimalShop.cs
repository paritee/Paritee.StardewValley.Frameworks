using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Api
{
    class AnimalShop
    {
        public static StardewValley.Object FormatAsAnimalAvailableForPurchase(Farm farm, string name, string displayName, int price, string[] buildings)
        {
            Api.AnimalShop.RequiredBuildingIsBuilt(farm, buildings, out string type);

            // Divide price by two because of the weird functionality in Object.salePrice()
            StardewValley.Object obj = new StardewValley.Object(Constants.AnimalShop.PurchaseAnimalStockParentSheetIndex, Constants.AnimalShop.PurchaseAnimalStockQuantity, false, price / 2)
            {
                Type = type,
                displayName = displayName
            };

            // MUST do this outside of the block because it gets overridden in 
            // the constructor
            obj.Name = name;

            return obj;
        }

        public static bool RequiredBuildingIsBuilt(Farm farm, string[] buildings, out string type)
        {
            if (buildings.Where(name => farm.isBuildingConstructed(name)).Any())
            {
                type = (string)null;

                return true;
            }

            // Grab the display name of the building
            string buildingName = Api.Content.GetDataValue<string, string>(
                Constants.Content.DataBlueprintsContentPath, buildings.First(),
                (int)Constants.Blueprint.DataValueIndex.DisplayName);

            // Grab the requires Coop string so we can replace "Coop" with the building's name
            type = Api.Content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5926").Replace(Constants.AnimalHouse.Coop, buildingName);

            return false;
        }

        public static List<ClickableTextureComponent> GetAnimalsToPurchaseComponents(PurchaseAnimalsMenu menu, List<StardewValley.Object> stock, Dictionary<string, Texture2D> icons, out int iconHeight)
        {
            iconHeight = 0;

            List<ClickableTextureComponent> animalsToPurchase = new List<ClickableTextureComponent>();

            for (int index = 0; index < stock.Count; ++index)
            {
                StardewValley.Object obj = stock[index];

                string name = obj.salePrice().ToString();
                string label = (string)null;
                string hoverText = obj.Name;

                Rectangle bounds = new Rectangle(menu.xPositionOnScreen + IClickableMenu.borderWidth + index % 3 * 64 * 2, menu.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth / 2 + index / 3 * 85, 128, 64);
                Texture2D texture = icons[obj.Name];
                Rectangle sourceRect = new Rectangle(0, 0, texture.Width, texture.Height);

                float scale = 4f;
                bool drawShadow = obj.Type == null;

                ClickableTextureComponent textureComponent = new ClickableTextureComponent(name, bounds, label, hoverText, texture, sourceRect, scale, drawShadow)
                {
                    item = obj,
                    myID = index,
                    rightNeighborID = index % 3 == 2 ? -1 : index + 1,
                    leftNeighborID = index % 3 == 0 ? -1 : index - 1,
                    downNeighborID = index + 3,
                    upNeighborID = index - 3
                };

                animalsToPurchase.Add(textureComponent);

                // We need the icon height for the menu resize
                iconHeight = texture.Height > iconHeight ? texture.Height : iconHeight;
            }

            return animalsToPurchase;
        }

        public static bool IsBlueChickenAvailableForPurchase(StardewValley.Farmer farmer)
        {
            return Api.FarmAnimal.RollBlueChickenChance(farmer);
        }
    }
}
