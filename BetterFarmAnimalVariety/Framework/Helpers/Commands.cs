using System;
using System.Collections.Generic;
using System.IO;

namespace BetterFarmAnimalVariety.Framework.Helpers
{
    class Commands
    {
        public static Framework.Config.FarmAnimalStock GetAnimalShopConfig(string category, string animalShop)
        {
            Framework.Config.FarmAnimalStock configFarmAnimalAnimalShop = new Framework.Config.FarmAnimalStock();

            if (animalShop.Equals(false.ToString().ToLower()))
            {
                return configFarmAnimalAnimalShop;
            }

            configFarmAnimalAnimalShop.Category = category;
            configFarmAnimalAnimalShop.Name = category;
            configFarmAnimalAnimalShop.Description = Constants.Config.AnimalShopDescriptionPlaceholder;
            configFarmAnimalAnimalShop.Price = Constants.Config.AnimalShopPricePlaceholder;
            configFarmAnimalAnimalShop.Icon = configFarmAnimalAnimalShop.GetDefaultIconPath();

            string fullPathToIcon = Path.Combine(Constants.Mod.ModPath, configFarmAnimalAnimalShop.Icon);

            if (!File.Exists(fullPathToIcon))
            {
                throw new FileNotFoundException($"{fullPathToIcon} does not exist");
            }

            return configFarmAnimalAnimalShop;
        }

        public static string DescribeFarmAnimalCategory(KeyValuePair<string, Framework.Config.FarmAnimal> entry)
        {
            string output = "";

            output += $"{entry.Key}\n";
            output += $"- Types: {String.Join(",", entry.Value.Types)}\n";
            output += $"- Buildings: {String.Join(",", entry.Value.Buildings)}\n";
            output += $"- AnimalShop:\n";
            output += $"-- Name: {entry.Value.AnimalShop.Name}\n";
            output += $"-- Description: {entry.Value.AnimalShop.Description}\n";
            output += $"-- Price: {entry.Value.AnimalShop.Price}\n";
            output += $"-- Icon: {entry.Value.AnimalShop.Icon}\n";

            return output;
        }
    }
}
