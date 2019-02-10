using BetterFarmAnimalVariety.Editors;
using BetterFarmAnimalVariety.Framework.Data;
using BetterFarmAnimalVariety.Framework.Events;
using BetterFarmAnimalVariety.Models;
using Harmony;
using Microsoft.Xna.Framework.Graphics;
using Paritee.StardewValleyAPI.Buildings.AnimalShop;
using Paritee.StardewValleyAPI.Buildings.AnimalShop.FarmAnimals;
using Paritee.StardewValleyAPI.FarmAnimals.Variations;
using Paritee.StardewValleyAPI.Menus;
using Paritee.StardewValleyAPI.Players;
using Paritee.StardewValleyAPI.Players.Actions;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BetterFarmAnimalVariety
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        public ModConfig Config;

        public Player Player;
        public BlueVariation BlueFarmAnimals;
        public VoidVariation VoidFarmAnimals;
        public AnimalShop AnimalShop;

        private bool ChangedPurchaseAnimalsMenuClickableComponents = false;

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
            this.Helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            // this.Helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            // this.Helper.Events.Display.RenderingActiveMenu += this.OnRenderingActiveMenu;
            // this.Helper.Events.Display.RenderedActiveMenu += this.OnRenderedActiveMenu;
            // this.Helper.Events.Display.MenuChanged += this.OnMenuChanged;
        }

        private void SetupHarmonyPatches()
        {
            // Patch the game code directly
            HarmonyInstance harmony = HarmonyInstance.Create(Framework.Helpers.Constants.ModKey);

            // TODO: might want to adjust these after some testing
            // - FarmInfoPage
            // - Forest

            harmony.PatchAll();
        }

        private void SetupConsoleCommands()
        {
            List<Framework.Commands.Command> commands = new List<Framework.Commands.Command>()
            {
                // Config
                new Framework.Commands.Config.ListCommand(this.Helper, this.Monitor, this.Config),
                new Framework.Commands.Config.VoidShopCommand(this.Helper, this.Monitor, this.Config),
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

                this.Helper.WriteConfig<ModConfig>(config);
            }
            else if (!config.IsValidFormat(targetFormat))
            {
                throw new FormatException();
            }

            config.InitializeFarmAnimals();

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

        /// <summary>Raised after the player pressed/released a keyboard, mouse, or controller button. This includes mouse clicks.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {

            Debug.WriteLine($"OnButtonPressed");

            //try
            //{
                Debug.WriteLine($"try");

                if (ConvertDirtyFarmAnimals.OnButtonPressed(e, out Dictionary<long, TypeLog> typesToBeMigrated))
                {
                    Debug.WriteLine($"typesToBeMigrated");

                    // Report if any animals were migrated and save the migrations
                    string message = typesToBeMigrated.Count > 0
                        ? $"ConvertDirtyFarmAnimals: Migrated {typesToBeMigrated.Count} dirty farm animals:\n-- {String.Join("\n-- ", typesToBeMigrated.Select(kvp => $"{kvp.Key}: {kvp.Value.CurrentType} saved as {kvp.Value.SavedType}"))}"
                        : $"ConvertDirtyFarmAnimals: No dirty farm animals found";

                    this.Monitor.Log(message, LogLevel.Trace);
                }
                Debug.WriteLine($"after");
            //}
            //catch (Exception exception)
            //{
            //    Debug.WriteLine($"exception");
            //    this.Monitor.Log(exception.Message, LogLevel.Error);
            //}

            // TODO: enable everything in bfav again
            //Framework.Events.PurchaseFarmAnimal.OnButtonPressed(this.Player, this.AnimalShop, e);
        }

        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            this.Player = new Player(Game1.player, this.Helper);

            // Set up everything else
            BlueConfig blueConfig = new BlueConfig(this.Player.HasSeenEvent(BlueVariation.EVENT_ID));
            this.BlueFarmAnimals = new BlueVariation(blueConfig);

            VoidConfig voidConfig = new VoidConfig(this.Config.VoidFarmAnimalsInShop, this.Player.HasCompletedQuest(VoidVariation.QUEST_ID));
            this.VoidFarmAnimals = new VoidVariation(voidConfig);

            List<FarmAnimalForPurchase> farmAnimalsForPurchase = this.Config.GetFarmAnimalsForPurchase(Game1.getFarm());
            StockConfig stockConfig = new StockConfig(farmAnimalsForPurchase, this.BlueFarmAnimals, this.VoidFarmAnimals);
            Stock stock = new Stock(stockConfig);

            this.AnimalShop = new AnimalShop(stock);
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
                Dictionary<string, List<string>> farmAnimals = this.Config.GetFarmAnimalTypes();
                BreedFarmAnimalConfig breedFarmAnimalConfig = new BreedFarmAnimalConfig(farmAnimals, this.BlueFarmAnimals, this.Config.RandomizeNewbornFromCategory, this.Config.RandomizeHatchlingFromCategory, this.Config.IgnoreParentProduceCheck);
                BreedFarmAnimal breedFarmAnimal = new BreedFarmAnimal(this.Player, breedFarmAnimalConfig);

                NameFarmAnimalMenu nameFarmAnimalMenu = new NameFarmAnimalMenu(namingMenu, breedFarmAnimal);

                nameFarmAnimalMenu.HandleChange();
            }
        }

        private void OnRenderedActiveMenu(object sender, RenderedActiveMenuEventArgs e)
        {
            // Ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady || Game1.activeClickableMenu == null)
            {
                return;
            }

            // Stop triggering the heavy redraws
            if (this.ChangedPurchaseAnimalsMenuClickableComponents)
            {
                return;
            }

            if (Game1.activeClickableMenu.GetType() == typeof(StardewValley.Menus.PurchaseAnimalsMenu))
            {
                if (!(Game1.activeClickableMenu is StardewValley.Menus.PurchaseAnimalsMenu))
                {
                    return;
                }

                StardewValley.Menus.PurchaseAnimalsMenu purchaseAnimalsMenu = Game1.activeClickableMenu as StardewValley.Menus.PurchaseAnimalsMenu;

                // We need to completely redo the animalsToPurchase to account for the custom sprites
                Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
                int iconHeight = 0;

                foreach (KeyValuePair<string, ConfigFarmAnimal> entry in this.Config.FarmAnimals)
                {
                    if (entry.Value.CanBePurchased())
                    {
                        Texture2D texture = this.Helper.Content.Load<Texture2D>(entry.Value.AnimalShop.Icon, ContentSource.ModFolder);

                        iconHeight = texture.Height;

                        textures.Add(entry.Value.Category, texture);
                    }
                }

                purchaseAnimalsMenu.animalsToPurchase = this.AnimalShop.FarmAnimalStock.DetermineClickableComponents(purchaseAnimalsMenu, textures);

                int rows = (int)Math.Ceiling((float)purchaseAnimalsMenu.animalsToPurchase.Count / 3); // Always at least one row

                // Adjust the size of the menud if there are more or less rows than it normally handles
                if (iconHeight > 0)
                {
                    purchaseAnimalsMenu.height = (int)(iconHeight * 2f) + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth / 2 + rows * 85;
                }

                this.ChangedPurchaseAnimalsMenuClickableComponents = true;

                return;
            }
        }

        private void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            this.ChangedPurchaseAnimalsMenuClickableComponents = false;
        }
    }
}
