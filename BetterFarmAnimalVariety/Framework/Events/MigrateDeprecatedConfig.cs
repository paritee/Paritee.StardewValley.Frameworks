using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Events
{
    class MigrateDeprecatedConfig
    {
        public static void OnEntry(ModEntry mod, string targetFormat, out ModConfig config)
        {
            // Try to load the last supported format...
            //Config.V2.ModConfig deprecatedConfig = Helpers.Mod.ReadConfig<Config.V2.ModConfig>();


            string path = Path.Combine(PariteeCore.Constants.Mod.Path, Constants.Mod.ConfigFileName);
            string json = File.ReadAllText(path);
            Config.V2.ModConfig deprecatedConfig = JsonConvert.DeserializeObject<Config.V2.ModConfig>(json);


            Debug.WriteLine($"json {path}");
            Debug.WriteLine($"json {json}");
            Debug.WriteLine($"deprecatedConfig.IsEnabled {deprecatedConfig.IsEnabled}");
            Debug.WriteLine($"deprecatedConfig.Format == null {deprecatedConfig.Format == null}");
            Debug.WriteLine($"deprecatedConfig.VoidFarmAnimalsInShop == null {deprecatedConfig.VoidFarmAnimalsInShop == null}");
            Debug.WriteLine($"deprecatedConfig.FarmAnimals == null {deprecatedConfig.FarmAnimals == null}");

            //... and migrate them to the current format
            if (!MigrateDeprecatedConfig.ToCurrentFormat<Config.V2.ModConfig>(deprecatedConfig, targetFormat, out config))
            {
                // Escalate the exception if the deprecated config could not be migrated
                throw new FormatException($"Invalid config format. {mod.ModManifest.Version.ToString()} requires format:{mod.ModManifest.Version.MajorVersion.ToString()}.");
            }
        }

        public static bool ToCurrentFormat<T>(T deprecatedConfig, string targetFormat, out ModConfig config)
        {
            if (deprecatedConfig is Config.V2.ModConfig)
            {
                MigrateDeprecatedConfig.HandleV2((Config.V2.ModConfig)Convert.ChangeType(deprecatedConfig, typeof(Config.V2.ModConfig)), targetFormat, out config);

                return true;
            }

            config = new ModConfig();

            return false;
        }

        public static void HandleV2(Config.V2.ModConfig deprecatedConfig, string targetFormat, out ModConfig config)
        {
            Debug.WriteLine($"HandleV2: deprecatedConfig.FarmAnimals == null {deprecatedConfig.FarmAnimals == null}");

            config = new BetterFarmAnimalVariety.ModConfig
            {
                Format = targetFormat,
                IsEnabled = deprecatedConfig.IsEnabled
            };

            MigrateDeprecatedConfig.CreateContentPack(deprecatedConfig);
        }

        public static void CreateContentPack(Config.V2.ModConfig deprecatedConfig)
        {
            string voidChicken = PariteeCore.Constants.VanillaFarmAnimalType.VoidChicken.ToString();

            // Get ready to make a new content pack
            ContentPacks.FarmAnimals contentPack = new ContentPacks.FarmAnimals(new List<ContentPacks.FarmAnimalCategory>());

            List<string> iconsToBeMoved = new List<string>();

            // Go through all of the old categories so that we can determine if we need to make a new content pack to preserve the changes
            foreach (KeyValuePair<string, Config.V2.ConfigFarmAnimal> oldFarmAnimals in deprecatedConfig.FarmAnimals)
            {
                // Check if this category is a vanila category
                bool isVanilla = PariteeCore.Constants.VanillaFarmAnimalCategory.All()
                    .Exists(o => o.ToString() == oldFarmAnimals.Key);

                // Always create the category with the update for vanilla and create for non-vanilla
                ContentPacks.FarmAnimalCategory.Actions action = isVanilla ? ContentPacks.FarmAnimalCategory.Actions.Update : ContentPacks.FarmAnimalCategory.Actions.Create;

                Cache.FarmAnimalStock animalShop = null;
                bool forceOverrideRemoveFromShop = true;
                bool forceOverrideExclude = false;

                if (oldFarmAnimals.Value.CanBePurchased())
                {
                    Int32.TryParse(oldFarmAnimals.Value.AnimalShop.Price, out int price);

                    string[] exclude = deprecatedConfig.IsChickenCategory(oldFarmAnimals.Key)
                        && oldFarmAnimals.Value.Types.Contains(voidChicken)
                        && deprecatedConfig.AreVoidFarmAnimalsInShopAlways()
                        ? new string[0]
                        : new string[] { voidChicken };

                    animalShop = new Cache.FarmAnimalStock
                    {
                        Name = oldFarmAnimals.Value.AnimalShop.Name,
                        Description = oldFarmAnimals.Value.AnimalShop.Description,
                        Icon = oldFarmAnimals.Value.AnimalShop.Icon,
                        Price = Math.Abs(price),
                        Exclude = exclude
                    };

                    // Check to make sure the icon exists...
                    string pathToIcon = Path.Combine(PariteeCore.Constants.Mod.Path, animalShop.Icon);

                    if (File.Exists(pathToIcon))
                    {
                        iconsToBeMoved.Add(animalShop.Icon);
                    }

                    forceOverrideRemoveFromShop = false;
                    forceOverrideExclude = true;
                }

                // Create the start of the category
                ContentPacks.FarmAnimalCategory category = new ContentPacks.FarmAnimalCategory(action)
                {
                    Category = oldFarmAnimals.Key,
                    Types = oldFarmAnimals.Value.Types,
                    Buildings = oldFarmAnimals.Value.Buildings,
                    AnimalShop = animalShop,
                    ForceOverrideTypes = true,
                    ForceOverrideBuildings = true,
                    ForceRemoveFromShop = forceOverrideRemoveFromShop,
                    ForceOverrideExclude = forceOverrideExclude,
                };

                contentPack.Categories.Add(category);
            }

            // Create the content pack so that the user can move it
            if (contentPack.Categories.Any())
            {
                long timestamp = DateTime.Now.ToFileTime();

                // Create the directory
                string contentPackDirectory = Constants.Mod.ConfigMigrationContentPackFullPath + timestamp;

                DirectoryInfo dir = new System.IO.DirectoryInfo(contentPackDirectory);
                dir.Create(); // If the directory already exists, this method does nothing.

                // content.json
                string contentJsonFilePath = Path.Combine(contentPackDirectory, Constants.Mod.ContentPackContentFileName);
                string contentJson = JsonConvert.SerializeObject(contentPack, Formatting.Indented);

                // manifest.json
                JObject manifest = JObject.FromObject(new
                {
                    Name = Constants.Mod.ConfigMigrationContentPackName,
                    Author = Constants.Mod.ConfigMigrationContentPackAuthor,
                    Version = Constants.Mod.ConfigMigrationContentPackVersion,
                    Description = Constants.Mod.ConfigMigrationContentPackDescription,
                    UniqueID = Constants.Mod.ConfigMigrationContentPackUniqueID + timestamp,
                    // MinimumApiVersion = Constants.Mod.ConfigMigrationContentPackMinimumApiVersion,
                    ContentPackFor = new
                    {
                        UniqueID = Constants.Mod.Key
                    },
                });

                string manifestJsonFilePath = Path.Combine(contentPackDirectory, Constants.Mod.ContentPackManifestFileName);
                string manifestJson = JsonConvert.SerializeObject(manifest, Formatting.Indented);

                // Write the files
                File.WriteAllText(manifestJsonFilePath, manifestJson);
                File.WriteAllText(contentJsonFilePath, contentJson);

                foreach (string sourceFileName in iconsToBeMoved)
                {
                    string destFileName = Path.Combine(contentPackDirectory, sourceFileName);

                    // Create the subfolders if necessary
                    System.IO.FileInfo destinationDirectory = new System.IO.FileInfo(destFileName);
                    destinationDirectory.Directory.Create();

                    // Copy the icon to the new location
                    File.Copy(sourceFileName, destFileName);
                }
            }
        }
    }
}
