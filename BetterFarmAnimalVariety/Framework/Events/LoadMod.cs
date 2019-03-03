using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;
using BetterFarmAnimalVariety.Framework.Editors;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Events
{
    class LoadMod
    {
        public static void OnEntry(ModEntry mod)
        {
            // Always kill the mod if we could not set up the config
            LoadMod.SetUpConfig(mod);

            // Harmony
            LoadMod.SetUpHarmonyPatches();

            // Commands
            LoadMod.SetUpConsoleCommands(mod);

            // Asset Editors
            LoadMod.SetUpAssetEditors(mod);
        }

        private static void SetUpConfig(ModEntry mod)
        {
            ModConfig config;

            string targetFormat = mod.ModManifest.Version.MajorVersion.ToString();

            try
            {
                // Load the config
                config = mod.Helper.ReadConfig<ModConfig>();

                // Do this outside of the constructor so that we can use the ModManifest helper
                if (config.Format == null)
                {
                    config.Format = targetFormat;
                }
                else
                {
                    config.AssertValidFormat(targetFormat);
                }
            }
            catch (Exception)
            {
                // Try to load the last supported format...
                Config.V2.ModConfig deprecatedConfig = mod.Helper.ReadConfig<Config.V2.ModConfig>();

                //... and migrate them to the current format
                if (!Helpers.Mod.MigrateDeprecatedConfigToCurrentFormat<Config.V2.ModConfig>(deprecatedConfig, targetFormat, out config))
                {
                    // Escalate the exception if the deprecated config could not be migrated
                    throw new FormatException($"Invalid config format. {mod.ModManifest.Version.ToString()} requires format:{mod.ModManifest.Version.MajorVersion.ToString()}.");
                }
            }

            if (!config.IsEnabled)
            {
                throw new ApplicationException($"Mod is disabled. To enable, set IsEnabled to true in config.json.");
            }

            // Only seed the config with vanilla if it's the first initializaiton 
            // of it or there are no farm animals in the list
            if (!config.HasCategories())
            {
                config.SeedVanillaFarmAnimals();
            }

            // Write back the config
            config.Write(mod.Helper);

            // Cache the farm animals and overwrite any existing file
            List<Cache.FarmAnimalCategory> categories = config.Categories
                .Select(o => new Cache.FarmAnimalCategory(PariteeCore.Constants.Mod.Path, o))
                .ToList();

            Helpers.FarmAnimals.Write(new Cache.FarmAnimals(categories));
        }

        private static void SetUpHarmonyPatches()
        {
            // Patch the game code directly
            HarmonyInstance harmony = HarmonyInstance.Create(Constants.Mod.Key);

            // Unpatched FarmAnimal constructor calls:
            // - Forest.Forest: Marnie's cows
            // - Farm.placeAnimal: only affects the "debug bluebook" command (CataloguePage is not used)
            // - Game1.parseDebugInput: only affects "debug animal" and "debug spawnCoopsAndBarns"
            // - FarmInfoPage: not used, but lists out animals by vanilla type

            harmony.PatchAll();
        }

        private static void SetUpConsoleCommands(ModEntry mod)
        {
            List<Commands.Command> commands = new List<Commands.Command>()
            {
                new Commands.List(mod.Helper, mod.Monitor),
            };

            foreach (Commands.Command command in commands)
            {
                mod.Helper.ConsoleCommands.Add(command.Name, command.Description, command.Callback);
            }
        }

        private static void SetUpAssetEditors(ModEntry mod)
        {
            mod.Helper.Content.AssetEditors.Add(new AnimalBirth(mod));
        }
    }
}
