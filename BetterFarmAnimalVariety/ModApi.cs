using BetterFarmAnimalVariety.Framework.Models;
using StardewModdingAPI;
using System;
using System.Collections.Generic;

namespace BetterFarmAnimalVariety
{
    public class ModApi
    {
        private readonly ModConfig Config;
        private readonly ISemanticVersion ModVersion;

        public ModApi(ModConfig config, ISemanticVersion modVersion)
        {
            this.Config = config;
            this.ModVersion = modVersion;
        }

        /// <param name="version">string</param>
        /// <returns>Returns bool</returns>
        public bool IsEnabled(string version)
        {
            if (!this.IsVersionSupported(version))
            {
                throw new NotSupportedException();
            }

            return this.Config.IsEnabled;
        }

        /// <param name="version">string</param>
        /// <returns>Returns Dictionary<string, string[]></returns>
        public List<FarmAnimalCategory> GetFarmAnimalCategories(string version)
        {
            if (!this.IsVersionSupported(version))
            {
                throw new NotSupportedException();
            }

            List<FarmAnimalCategory> categories = new List<FarmAnimalCategory>();

            int order = 0;

            foreach (Framework.Config.FarmAnimal animal in this.Config.FarmAnimals)
            {
                FarmAnimalCategory category = animal.CanBePurchased()
                    ? new FarmAnimalCategory(animal.Category, order++, animal.AnimalShop.Name, animal.AnimalShop.Description, animal.AnimalShop.Price, animal.Types, animal.Buildings, animal.AnimalShop.Exclude)
                    : new FarmAnimalCategory(animal.Category, order++, animal.Types, animal.Buildings);

                categories.Add(category);
            }

            return categories;
        }

        private bool IsVersionSupported(string version)
        {
            // Must match the major version
            return version == this.ModVersion.MajorVersion.ToString();
        }
    }
}
