using BetterFarmAnimalVariety.Framework.Events;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using System;
using System.Collections.Generic;

namespace BetterFarmAnimalVariety
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            try
            {
                LoadMod.OnEntry(this);
            }
            catch (Exception e)
            {
                // Any errors that happen here should be considered show-stoppers
                this.Monitor.Log(e.Message, LogLevel.Error);

                return;
            }

            // Events
            this.Helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            this.Helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            this.Helper.Events.GameLoop.Saving += this.OnSaving;
            this.Helper.Events.GameLoop.Saved += this.OnSaved;
        }

        public override object GetApi()
        {
            ModConfig config = Framework.Helpers.Mod.ReadConfig<ModConfig>();

            return new Framework.Api.BetterFarmAnimalVariety(config);
        }
        
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            // Seed a new cache with the vanilla animals...
            RefreshCache.SeedCacheWithVanillaFarmAnimals();

            //... then content packs are loaded to apply changes to the cache...
            LoadContentPacks.SetUpContentPacks(this.Helper.ContentPacks.GetOwned(), this.Monitor);

            // ... and validate all of the cached animals...
            RefreshCache.ValidateCachedFarmAnimals(this.Helper, this.Monitor);

            // ... finally register the types with MoreAnimals
            try
            {
                IntegrateWithMoreAnimals.RegisterAnimals(this.Helper, this.Monitor);
            }
            catch (Framework.Exceptions.ApiNotFoundException exception)
            {
                this.Monitor.Log($"Cannot register animals with More Animals: {exception.Message}", LogLevel.Trace);
            }
        }

        /// <summary>Raised before/after the game reads data from a save file and initialises the world. This event isn't raised after saving; if you want to do something at the start of each day, see DayStarted instead.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            try
            {
                // Invalidate the farm animals data cache for JsonAssets
                IntegrateWithJsonAssets.RefreshFarmAnimalData(this.Helper);
            }
            catch (Framework.Exceptions.ApiNotFoundException exception)
            {
                this.Monitor.Log($"Cannot refresh farm animal data: {exception.Message}", LogLevel.Trace);
            }
            catch (Framework.Exceptions.SaveNotLoadedException exception)
            {
                this.Monitor.Log($"Cannot refresh farm animal data: {exception.Message}", LogLevel.Trace);
            }

            try
            {
                // NOTE:
                // Don't need to clean up saves prior to loading. Dirty saves will 
                // automatically be fixed on the next save. Only impact would be to 
                // players who upgraded from 1.x or 2.x who removed patches without 
                // selling/deleting the animals and without going through the cleaning 
                // script. Minor impact.

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
