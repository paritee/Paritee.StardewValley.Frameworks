using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;
using BetterFarmAnimalVariety.Framework.Editors;
using PariteeCore = Paritee.StardewValley.Core;
using System.Diagnostics;
using System.IO;

namespace BetterFarmAnimalVariety.Framework.Events
{
    class LoadMod
    {
        public static void OnEntry(ModEntry mod)
        {
            // Always kill the mod if we could not set up the config
            LoadMod.SetUpConfig(mod);

            // Seed a new cache with the vanilla animals; content packs loaded 
            // later will modify these animals
            LoadMod.SeedCacheWithVanillaFarmAnimals();

            // Harmony
            LoadMod.SetUpHarmonyPatches();

            // Commands
            LoadMod.SetUpConsoleCommands(mod);

            // Asset Editors
            LoadMod.SetUpAssetEditors(mod);
        }

        private static void SetUpConfig(ModEntry mod)
        {
            Debug.WriteLine($"json {File.ReadAllText(Path.Combine(PariteeCore.Constants.Mod.Path, Constants.Mod.ConfigFileName))}");



            ModConfig config;

            string targetFormat = mod.ModManifest.Version.MajorVersion.ToString();

            try
            {
                Debug.WriteLine($"SetUpConfig");
                // Load the config
                config = Helpers.Mod.ReadConfig<ModConfig>();
                Debug.WriteLine($"ReadConfig as ModConfig");
                Debug.WriteLine($"config.Format == null {config.Format == null}");

                // Do this outside of the constructor so that we can use the ModManifest helper
                if (config.Format == null)
                {
                    config.Format = targetFormat;
                }
                else
                {
                    Debug.WriteLine($"config.Format {config.Format}");
                    config.AssertValidFormat(targetFormat);
                    Debug.WriteLine($"AssertedValidFormat");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"e.Message {e.Message}");
                Debug.WriteLine($"json {File.ReadAllText(Path.Combine(PariteeCore.Constants.Mod.Path, Constants.Mod.ConfigFileName))}");

                MigrateDeprecatedConfig.OnEntry(mod, targetFormat, out config);
            }

            // Write back the config
            config.Write(mod.Helper);

            if (!config.IsEnabled)
            {
                throw new ApplicationException($"Mod is disabled. To enable, set IsEnabled to true in config.json.");
            }
        }

        private static void SeedCacheWithVanillaFarmAnimals()
        {
            // Seed with all of the vanilla farm animals
            List<Cache.FarmAnimalCategory> categories = PariteeCore.Constants.VanillaFarmAnimalCategory.All()
                .Select(o => new Framework.Cache.FarmAnimalCategory(PariteeCore.Constants.Mod.Path, o))
                .ToList();

            // Reset the cache
            Cache.FarmAnimals cache = new Cache.FarmAnimals(categories);

            // Commit the seed
            Helpers.FarmAnimals.Write(cache);
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
