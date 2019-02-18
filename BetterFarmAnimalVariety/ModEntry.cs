using BetterFarmAnimalVariety.Framework.Editors;
using BetterFarmAnimalVariety.Framework.Events;
using BetterFarmAnimalVariety.Framework.SaveData;
using Harmony;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
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
            // Config
            try
            {
                this.Config = this.LoadConfig();
            }
            catch (FormatException)
            {
                this.Monitor.Log($"Your config.json format is invalid and BFAV has shut down. You are running BFAV v{this.ModManifest.Version.ToString()} which requires a config.json format of {this.ModManifest.Version.MajorVersion.ToString()}.", LogLevel.Alert);
                return;
            }

            if (!this.Config.IsEnabled)
            {
                this.Monitor.Log($"BFAV is disabled. To enable, set IsEnabled to true in config.json.", LogLevel.Debug);
                return;
            }

            // Harmony
            this.SetupHarmonyPatches();

            // Commands
            this.SetupConsoleCommands();

            // Asset Editors
            this.Helper.Content.AssetEditors.Add(new AnimalBirthEditor(this));

            // Events
            this.Helper.Events.GameLoop.Saving += this.OnSaving;
            this.Helper.Events.GameLoop.Saved += this.OnSaved;
            this.Helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            // this.Helper.Events.Display.RenderingActiveMenu += this.OnRenderingActiveMenu;
        }

        private void SetupHarmonyPatches()
        {
            // Patch the game code directly
            HarmonyInstance harmony = HarmonyInstance.Create(Framework.Constants.Mod.ModKey);

            // TODO: might want to adjust these after some testing
            // - FarmInfoPage
            // - Forest

            harmony.PatchAll();
        }

        private void SetupConsoleCommands()
        {
            // TODO: AnimalShop.Exclude
            List<Framework.Commands.Command> commands = new List<Framework.Commands.Command>()
            {
                // Config
                new Framework.Commands.Config.ListCommand(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.Config.RandomizeFromCategoryCommand(this.Helper, this.Monitor, this.Config),

                // FarmAnimal
                new Framework.Commands.FarmAnimal.ListCommand(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.ResetCommand(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.AddCategoryCommand(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.RemoveCategoryCommand(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.AddTypesCommand(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.RemoveTypesCommand(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.SetBuildingsCommand(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.SetAnimalShopCommand(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.SetAnimalShopNameCommand(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.SetAnimalShopDescriptionCommand(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.SetAnimalShopPriceCommand(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.FarmAnimal.SetAnimalShopIconCommand(this.Helper, this.Monitor, this.Config),
            };

            foreach (Framework.Commands.Command command in commands)
            {
                this.Helper.ConsoleCommands.Add(command.Name, command.Description, command.Callback);
            }
        }

        public override object GetApi()
        {
            return new ModApi(this.Config, this.ModManifest.Version);
        }

        private ModConfig LoadConfig()
        {
            // Load the config
            ModConfig config = this.Helper.ReadConfig<ModConfig>();

            string targetFormat = this.ModManifest.Version.MajorVersion.ToString();

            // Do this outside of the constructor so that we can use the ModManifest helper
            if (config.Format == null)
            {
                config.Format = targetFormat;
            }
            else if (!config.IsValidFormat(targetFormat))
            {
                throw new FormatException();
            }

            // Only seed the config with vanilla if it's the first initializaiton 
            // of it or there are no farm animals in the list
            if (!config.FarmAnimals.Any())
            {
                config.SeedVanillaFarmAnimals();
            }

            // Write back the config
            this.Helper.WriteConfig<ModConfig>(config);

            return config;
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
                // Somehow they removed the default animals... 
                this.Monitor.Log(exception.Message, LogLevel.Error);
            
                // ... this is a show stopper
                throw exception;
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
                // Somehow they removed the default animals... 
                this.Monitor.Log(exception.Message, LogLevel.Error);

                // ... this is a show stopper
                throw exception;
            }
        }

        /// <summary>Raised after the player pressed/released a keyboard, mouse, or controller button. This includes mouse clicks.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                if (ConvertDirtyFarmAnimals.OnButtonPressed(e, out Dictionary<long, TypeLog> typesToBeMigrated))
                {
                    // Report if any animals were migrated and save the migrations
                    string message = typesToBeMigrated.Any()
                        ? $"ConvertDirtyFarmAnimals: Migrated {typesToBeMigrated.Count} dirty farm animals:\n-- {String.Join("\n-- ", typesToBeMigrated.Select(kvp => $"{kvp.Key}: {kvp.Value.CurrentType} saved as {kvp.Value.SavedType}"))}"
                        : $"ConvertDirtyFarmAnimals: No dirty farm animals found";

                    this.Monitor.Log(message, LogLevel.Trace);
                }
            }
            catch (Exception exception)
            {
                this.Monitor.Log(exception.Message, LogLevel.Error);
            }
        }

        private void OnRenderingActiveMenu(object sender, RenderingActiveMenuEventArgs e)
        {
            // Ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady || Game1.activeClickableMenu == null)
            {
                return;
            }

            if (!(Game1.activeClickableMenu is StardewValley.Menus.NamingMenu))
            {
                return;
            }

            StardewValley.Menus.NamingMenu namingMenu = Game1.activeClickableMenu as StardewValley.Menus.NamingMenu;

            if (namingMenu.GetType() == typeof(StardewValley.Menus.NamingMenu))
            {
                //Dictionary<string, List<string>> farmAnimals = this.Config.GroupTypesByCategory();
                //BreedFarmAnimalConfig breedFarmAnimalConfig = new BreedFarmAnimalConfig(farmAnimals, this.BlueFarmAnimals, this.Config.RandomizeNewbornFromCategory, this.Config.RandomizeHatchlingFromCategory, this.Config.IgnoreParentProduceCheck);
                //BreedFarmAnimal breedFarmAnimal = new BreedFarmAnimal(this.Player, breedFarmAnimalConfig);

                //NameFarmAnimalMenu nameFarmAnimalMenu = new NameFarmAnimalMenu(namingMenu, breedFarmAnimal);

                //nameFarmAnimalMenu.HandleChange();
            }
        }
    }
}
