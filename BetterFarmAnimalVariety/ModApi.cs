using BetterFarmAnimalVariety.Framework.Models;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

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
            this.AssertVersionIsSupported(version);

            return this.Config.IsEnabled;
        }

        /// <param name="version">string</param>
        /// <returns>Returns List<Paritee.Core.Models.FarmAnimalCategory></returns>
        public List<PariteeCore.Models.FarmAnimalCategory> GetFarmAnimalCategories(string version)
        {
            this.AssertVersionIsSupported(version);

            List<PariteeCore.Models.FarmAnimalCategory> categories = new List<PariteeCore.Models.FarmAnimalCategory>();

            int order = 0;

            foreach (Framework.Config.FarmAnimal animal in this.Config.FarmAnimals)
            {
                PariteeCore.Models.FarmAnimalCategory category = animal.CanBePurchased()
                    ? new PariteeCore.Models.FarmAnimalCategory(animal.Category, order++, animal.AnimalShop.Name, animal.AnimalShop.Description, animal.AnimalShop.Price, animal.Types, animal.Buildings, animal.AnimalShop.Exclude)
                    : new PariteeCore.Models.FarmAnimalCategory(animal.Category, order++, animal.Types, animal.Buildings);

                categories.Add(category);
            }

            return categories;
        }

        /// <summary>Get the farm animal's types from the save data.</summary>
        /// <param name="version">string</param>
        /// <returns>Returns Dictionary<long, KeyValuePair<string, string>></returns>
        public Dictionary<long, KeyValuePair<string, string>> GetFarmAnimalTypeHistory(string version)
        {
            this.AssertVersionIsSupported(version);

            Framework.Helpers.Assert.GameLoaded();

            FarmAnimalsSaveData saveData = Framework.Helpers.Mod.ReadSaveData<FarmAnimalsSaveData>(Framework.Constants.Mod.FarmAnimalsSaveDataKey);

            return saveData.TypeHistory.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ConvertToKeyValuePair());
        }

        /// <summary>Assert the version asked for is supported.</summary>
        /// <param name="version">string</param>
        /// <exception cref="NotSupportedException"></exception>
        private void AssertVersionIsSupported(string version)
        {
            Framework.Helpers.Assert.VersionIsSupported(version, this.ModVersion.MajorVersion.ToString());
        }
    }
}
