using BetterFarmAnimalVariety.Framework.Editors;
using BetterFarmAnimalVariety.Framework.Events;
using Harmony;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        public ModConfig Config;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            // Kill the mod if we could not set up the config
            try
            {
                this.SetUpConfig();
            }
            catch (FormatException e)
            {
                this.Monitor.Log(e.Message, LogLevel.Alert);

                return;
            }
            catch (Exception e)
            {
                this.Monitor.Log(e.Message, LogLevel.Debug);

                return;
            }

            // Harmony
            this.SetUpHarmonyPatches();

            // Commands
            this.SetUpConsoleCommands();

            // Asset Editors
            this.SetUpAssetEditors();

            // Events
            this.SetUpEvents();
        }

        private void SetUpConfig()
        {
            ModConfig config;

            string targetFormat = this.ModManifest.Version.MajorVersion.ToString();

            try
            {
                // Load the config
                config = this.Helper.ReadConfig<ModConfig>();

                // Do this outside of the constructor so that we can use the ModManifest helper
                if (config.Format == null)
                {
                    config.Format = targetFormat;
                }
                else if (!config.IsValidFormat(targetFormat))
                {
                    throw new FormatException();
                }
            }
            catch (Exception)
            {
                // Try to load the last supported format...
                Framework.Config.V2.ModConfig deprecatedConfig = this.Helper.ReadConfig<Framework.Config.V2.ModConfig>();

                //... and migrate them to the current format
                if (!Framework.Helpers.Mod.MigrateDeprecatedConfigToCurrentFormat<Framework.Config.V2.ModConfig>(deprecatedConfig, targetFormat, out config))
                {
                    // Escalate the exception if the deprecated config could not be migrated
                    throw new FormatException($"Invalid config format. {this.ModManifest.Version.ToString()} requires format:{this.ModManifest.Version.MajorVersion.ToString()}.");
                }
            }

            // Only seed the config with vanilla if it's the first initializaiton 
            // of it or there are no farm animals in the list
            if (!config.FarmAnimals.Any())
            {
                config.SeedVanillaFarmAnimals();
            }

            // Write back the config
            this.Helper.WriteConfig<ModConfig>(config);

            if (!config.IsEnabled)
            {
                throw new Exception($"Mod is disabled. To enable, set IsEnabled to true in config.json.");
            }

            this.Config = config;
        }

        private void SetUpHarmonyPatches()
        {
            // Patch the game code directly
            HarmonyInstance harmony = HarmonyInstance.Create(Framework.Constants.Mod.Key);

            // Unpatched FarmAnimal constructor calls:
            // - Forest.Forest: Marnie's cows
            // - Farm.placeAnimal: only affects the "debug bluebook" command (CataloguePage is not used)
            // - Game1.parseDebugInput: only affects "debug animal" and "debug spawnCoopsAndBarns"
            // - FarmInfoPage: not used, but lists out animals by vanilla type

            harmony.PatchAll();
        }

        private void SetUpConsoleCommands()
        {
            // TODO: AnimalShop.Exclude
            List<Framework.Commands.Command> commands = new List<Framework.Commands.Command>()
            {
                // Config
                new Framework.Commands.Config.List(this.Helper, this.Monitor, this.Config),

                // FarmAnimal
                new Framework.Commands.FarmAnimal.List(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.Reset(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.AddCategory(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.RemoveCategory(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.AddTypes(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.RemoveTypes(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.SetBuildings(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.SetAnimalShop(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.SetAnimalShopName(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.SetAnimalShopDescription(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.SetAnimalShopPrice(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.SetAnimalShopIcon(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.SetAnimalShopExclude(this.Helper, this.Monitor, this.Config),
            };

            foreach (Framework.Commands.Command command in commands)
            {
                this.Helper.ConsoleCommands.Add(command.Name, command.Description, command.Callback);
            }
        }

        private void SetUpAssetEditors()
        {
            this.Helper.Content.AssetEditors.Add(new AnimalBirth(this));
        }

        private void SetUpEvents()
        {
            // NOTE:
            // Don't need to clean up saves prior to loading. Dirty saves will 
            // automatically be fixde on the next save. Only impact would be to 
            // players who upgraded from 1.x or 2.x who removed patches without 
            // selling /deleting the animals and without going through the cleaning 
            // script. Minor impact.

            this.Helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            this.Helper.Events.GameLoop.Saving += this.OnSaving;
            this.Helper.Events.GameLoop.Saved += this.OnSaved;
        }

        public override object GetApi()
        {
            return new ModApi(this.Config, this.ModManifest.Version);
        }

        /// <summary>Raised before/after the game reads data from a save file and initialises the world. This event isn't raised after saving; if you want to do something at the start of each day, see DayStarted instead.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            try
            {
                ConvertDirtyFarmAnimals.OnSaveLoaded(e);
            }
            catch (KeyNotFoundException exception)
            {
                this.HandleKeyNotFoundException(exception);
            }
        }

        /// <summary>Raised before the game writes data to save file (except the initial save creation). The save won't be written until all mods have finished handling this event.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaving(object sender, SavingEventArgs e)
        {
            try
            {
                ConvertDirtyFarmAnimals.OnSaving(e);
            }
            catch (KeyNotFoundException exception)
            {
                this.HandleKeyNotFoundException(exception);
            }
        }

        /// <summary>Raised before the game writes data to save file (except the initial save creation). The save won't be written until all mods have finished handling this event.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaved(object sender, SavedEventArgs e)
        {
            try
            {
                ConvertDirtyFarmAnimals.OnSaved(e);
            }
            catch (KeyNotFoundException exception)
            {
                this.HandleKeyNotFoundException(exception);
            }
        }

        private void HandleKeyNotFoundException(KeyNotFoundException exception)
        {
            // Somehow they removed the default animals... 
            this.Monitor.Log(exception.Message, LogLevel.Error);

            // ... this is a show stopper
            throw exception;
        }
    }
}
