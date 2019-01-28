using BetterFarmAnimalVariety.Models;
using Paritee.StardewValleyAPI.Buidlings.AnimalShop.FarmAnimals;
using Paritee.StardewValleyAPI.FarmAnimals.Variations;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety
{
    public class ModConfig
    {
        public VoidConfig.InShop VoidFarmAnimalsInShop;
        public Dictionary<ConfigFarmAnimal.TypeGroup, ConfigFarmAnimal> FarmAnimals;

        private AppSettings AppSettings;

        public ModConfig()
        {
            Dictionary<string, string> settings = Properties.Settings.Default.Properties
              .Cast<System.Configuration.SettingsProperty>()
              .OrderBy(s => s.Name).ToDictionary(x => x.Name.ToString(), x => x.DefaultValue.ToString());

            this.AppSettings = new AppSettings(settings);
            this.VoidFarmAnimalsInShop = VoidConfig.InShop.Never;

            this.InitializeFarmAnimals();
        }

        private List<ConfigFarmAnimal.TypeGroup> GetFarmAnimalGroups()
        {
            return this.FarmAnimals.Keys.ToList<ConfigFarmAnimal.TypeGroup>();
        }

        public List<string> GetFarmAnimalTypes()
        {
            List<string> types = new List<string>();

            foreach (KeyValuePair<ConfigFarmAnimal.TypeGroup, ConfigFarmAnimal> Entry in this.FarmAnimals)
                types = types.Concat(Entry.Value.GetTypes()).ToList<string>();

            return types.ToList<string>();
        }

        public List<string> GetFarmAnimalTypes(ConfigFarmAnimal.TypeGroup farmAnimalGroup)
        {
            return this.GetFarmAnimalTypesByGroup(farmAnimalGroup);
        }

        public List<string> GetFarmAnimalTypesByGroup(ConfigFarmAnimal.TypeGroup group)
        {
            return this.FarmAnimals[group].GetTypes().ToList<string>();
        }

        private void InitializeFarmAnimals()
        {
            this.FarmAnimals = new Dictionary<ConfigFarmAnimal.TypeGroup, ConfigFarmAnimal>();

            this.UpdateFarmAnimalValuesFromAppSettings();
        }

        public void UpdateFarmAnimalValuesFromAppSettings()
        {
            List<AppSetting> appSettings = this.AppSettings.FindFarmAnimalAppSettings();

            foreach (AppSetting appSetting in appSettings)
            {
                ConfigFarmAnimal configFarmAnimal = new ConfigFarmAnimal(appSetting);
                  
                if (this.FarmAnimals.ContainsKey(configFarmAnimal.Group))
                {
                    // Preserve user preferences if the group already has data loaded from the user's config JSON
                    ConfigFarmAnimal custom = this.FarmAnimals[configFarmAnimal.Group];

                    configFarmAnimal.Name = custom.Name;
                    configFarmAnimal.Description = custom.Description;
                    configFarmAnimal.ShopIcon = custom.ShopIcon;
                    configFarmAnimal.Types = custom.Types;

                    // Add the configuration to the farm animals config
                    this.FarmAnimals[configFarmAnimal.Group] = configFarmAnimal;
                }
                else
                {
                    this.FarmAnimals.Add(configFarmAnimal.Group, configFarmAnimal);
                }
            }
        }

        public Dictionary<Stock.Name, string[]> MapFarmAnimalsToAvailableAnimalShopStock()
        {
            Dictionary<Stock.Name, string[]> availableStock = new Dictionary<Stock.Name, string[]>();

            foreach (KeyValuePair<ConfigFarmAnimal.TypeGroup, ConfigFarmAnimal> entry in this.FarmAnimals)
            {
                try
                {
                    Stock.Name name = entry.Value.GetStockName();
                    availableStock.Add(name, entry.Value.Types);
                }
                catch
                {
                    // Do nothing, "Dinosaurs" will trigger this
                }
            }

            return availableStock;
        }
    }
}