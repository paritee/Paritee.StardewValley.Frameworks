using System;
using System.Collections.Generic;
using Harmony;

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

            // Asset Loaders
            LoadMod.SetUpAssetLoaders(mod);
        }

        private static void SetUpConfig(ModEntry mod)
        {
            ModConfig config;

            string targetFormat = mod.ModManifest.Version.MajorVersion.ToString();

            try
            {
                // Load the config
                config = Helpers.Mod.ReadConfig<ModConfig>();

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
            catch (Exception e)
            {
                MigrateDeprecatedConfig.OnEntry(mod, targetFormat, out config);
            }

            // Write back the config
            config.Write(mod.Helper);

            if (!config.IsEnabled)
            {
                throw new ApplicationException($"Mod is disabled. To enable, set IsEnabled to true in config.json.");
            }
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
            mod.Helper.Content.AssetEditors.Add(new Editors.AnimalBirth(mod));
            mod.Helper.Content.AssetEditors.Add(new Editors.FarmAnimalData());
        }

        private static void SetUpAssetLoaders(ModEntry mod)
        {
            mod.Helper.Content.AssetLoaders.Add(new Loaders.FarmAnimalSprites());
        }
    }
}
