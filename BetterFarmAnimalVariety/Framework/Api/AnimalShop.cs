using StardewValley;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Api
{
    class AnimalShop
    {
        public static StardewValley.Object FormatAsAnimalAvailableForPurchase(Farm farm, string name, string displayName, int price, string[] buildings)
        {
            Api.AnimalShop.RequiredBuildingIsBuilt(farm, buildings, out string type);

            // Divide price by two because of the weird functionality in Object.salePrice()
            return new StardewValley.Object(Constants.AnimalShop.PurchaseAnimalStockParentSheetIndex, Constants.AnimalShop.PurchaseAnimalStockQuantity, false, price / 2)
            {
                Name = name,
                Type = type,
                displayName = displayName
            };
        }

        public static bool RequiredBuildingIsBuilt(Farm farm, string[] buildings, out string type)
        {
            bool hasBuilding = buildings.Where(name => farm.isBuildingConstructed(name)).Any();

            if (!hasBuilding)
            {
                type = (string)null;

                return false;
            }

            // Grab the display name of the building
            string buildingName = Api.Content.GetDataValue<string, string>(
                Constants.Content.DataBlueprintsContentPath, buildings.First(),
                (int)Constants.Blueprint.DataValueIndex.DisplayName);

            // Grab the requires Coop string so we can replace "Coop" with the building's name
            type = Api.Content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5926").Replace(Constants.AnimalHouse.Coop, buildingName);

            return true;
        }
    }
}
