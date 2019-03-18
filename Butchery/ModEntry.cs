using Microsoft.Xna.Framework;
using Netcode;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Network;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Butchery
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        private Framework.Config.Settings Config;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            // Static helpers
            Framework.Constants.Static.Helper = Helper;
            Framework.Constants.Static.i18n = Helper.Translation;

            // Config
            this.Config = helper.ReadConfig<Framework.Config.Settings>();

            if (!this.Config.IsEnabled)
            {
                Monitor.Log($"{helper.ModRegistry.ModID} is disabled. Enable in the config.json.", LogLevel.Debug);

                return;
            }

            // Asset editors
            helper.Content.AssetEditors.Add(new Framework.Editors.Meat());

            // Events
            this.Helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            this.Helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
        }

        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the game is launched, right before the first update tick.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            Framework.Data.Animals.Seed();
        }

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // Save must be loaded ...
            if (!Context.IsWorldReady)
            {
                return;
            }

            // ... and the player can move ...
            if (!Game1.player.CanMove)
            {
                return;
            }

            // ... and player tried to use a tool ...
            if (!e.Button.IsUseToolButton())
            {
                return;
            }

            // ... and the axe is equipped ...
            if (!(Game1.player.CurrentTool is Axe axe))
            {
                return;
            }

            // ... and there wasn't an object on this tile
            if (Game1.currentLocation.isObjectAtTile((int)e.Cursor.Tile.X, (int)e.Cursor.Tile.Y))
            {
                return;
            }

            // Get all animals in the location ... 
            if (!this.TryGetAnimalsInLocation(out NetLongDictionary<FarmAnimal, NetRef<FarmAnimal>> animals))
            {
                return;
            }

            // ... and get one animal that was in range
            if (!this.TryGetAnimalInStrikeRange(animals, e.Cursor.Tile, out FarmAnimal animal))
            {
                return;
            }

            // Perform the tool action
            Game1.pressUseToolButton();

            // Affect the animal's health
            animal.health.Value -= this.Config.StrikePower;

            // Oof
            if (animal.health.Value <= 0)
            {
                this.KillAnimal(animal, animals);
                this.AffectOtherAnimals(animal, animals);
            }
            else
            {
                this.MakeAnimalSad(animal);
            }

            // Don't continue
            this.Helper.Input.Suppress(e.Button);
        }

        private bool TryGetAnimalsInLocation(out NetLongDictionary<FarmAnimal, NetRef<FarmAnimal>> animals)
        {
            animals = null;

            if (Game1.currentLocation is AnimalHouse animalHouse)
            {
                animals = animalHouse.animals;
            }
            else if (Game1.currentLocation is Farm farm)
            {
                animals = farm.animals;
            }
            else
            {
                // ... if the location has animals
                return false;
            }

            return true;
        }

        private bool TryGetAnimalInStrikeRange(NetLongDictionary<FarmAnimal, NetRef<FarmAnimal>> animals, Vector2 tile, out FarmAnimal animal)
        {
            animal = null;

            Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle((int)tile.X * 64, (int)tile.Y * 64, 64, 64);

            foreach (KeyValuePair<long, FarmAnimal> pair in animals.Pairs)
            {
                if (pair.Value.GetBoundingBox().Intersects(rectangle))
                {
                    animal = pair.Value;

                    return true;
                }
            }

            return false;
        }

        private void KillAnimal(FarmAnimal animal, NetLongDictionary<FarmAnimal, NetRef<FarmAnimal>> animals)
        {
            // Remove the animal
            (animal.home.indoors.Value as AnimalHouse).animalsThatLiveHere.Remove(animal.myID.Value);
            animal.health.Value = -1;

            // Display the death
            this.ShowAnimalDeath(animal);

            // Gain farming experience
            Game1.player.gainExperience(0, this.Config.FarmingExperience);

            // Spawn the meat
            this.SpawnMeat(animal);
        }

        private void ShowAnimalDeath(FarmAnimal animal)
        {
            // Display clouds
            int clouds = animal.frontBackSourceRect.Width / 2;
            Multiplayer multiplayer = this.Helper.Reflection.GetField<Multiplayer>(typeof(Game1), "multiplayer").GetValue();

            for (int index = 0; index < clouds; ++index)
            {
                int num2 = Game1.random.Next(25, 200);
                multiplayer.broadcastSprites(Game1.currentLocation, new TemporaryAnimatedSprite(5, animal.Position + new Vector2((float)Game1.random.Next(-32, animal.frontBackSourceRect.Width * 3), (float)Game1.random.Next(-32, animal.frontBackSourceRect.Height * 3)), new Color((int)byte.MaxValue - num2, 0, 0), 8, false, Game1.random.NextDouble() < 0.5 ? 50f : (float)Game1.random.Next(30, 200), 0, 64, -1f, 64, Game1.random.NextDouble() < 0.5 ? 0 : Game1.random.Next(0, 600))
                {
                    scale = Game1.random.Next(2, 5) * 0.25f,
                    alpha = Game1.random.Next(2, 5) * 0.25f,
                    motion = new Vector2(0.0f, (float)-Game1.random.NextDouble())
                });
            }

            // Oof
            animal.makeSound();
            Game1.playSound("dirtyHit");
            Game1.playSound("killAnimal");
        }

        private void AffectOtherAnimals(FarmAnimal animal, NetLongDictionary<FarmAnimal, NetRef<FarmAnimal>> animals)
        {
            // Check if other animals saw ...
            foreach (KeyValuePair<long, FarmAnimal> pair in animals.Pairs)
            {
                if (pair.Key == animal.myID.Value)
                {
                    continue;
                }

                if (Vector2.Distance(pair.Value.getTileLocation(), animal.getTileLocation()) > this.Config.MoodTilesDistance)
                {
                    continue;
                }

                // ... and make them sad
                this.MakeAnimalSad(pair.Value);
            }
        }

        private void MakeAnimalSad(FarmAnimal animal)
        {
            animal.happiness.Value = (byte)(animal.happiness.Value * this.Config.MoodPenalty);
            animal.moodMessage.Value = FarmAnimal.unhappy;
            animal.doEmote(Character.sadEmote);
        }

        private void SpawnMeat(FarmAnimal animal)
        {
            // Quantity of meat
            int quantity = this.CalculateQuantity(animal);

            if (quantity <= 0)
            {
                return;
            }

            int price = this.CalculatePrice(animal);

            // Spawn the meat
            for (int index = 0; index < quantity; ++index)
            {
                Game1.createItemDebris(new StardewValley.Object(animal.meatIndex.Value, 1, false, price, animal.produceQuality.Value), Utility.PointToVector2(animal.GetBoundingBox().Center), Game1.random.Next(1, 4));
            }
        }

        private int CalculateQuantity(FarmAnimal animal)
        {
            Framework.Data.AnimalType animalType = this.Helper.Data.ReadJsonFile<Framework.Data.Animals>(Framework.Constants.Mod.AnimalsData)
                .Types.First(o => o.Name == animal.type.Value);

            float minQuantity = animalType.MinMeat;
            float maxQuantity = animalType.MaxMeat;

            // Babies don't have much meat
            if (animal.isBaby())
            {
                minQuantity *= this.Config.BabyPenalty;
                maxQuantity *= this.Config.BabyPenalty;
            }

            // If starving, reduce quantitiy
            if (animal.fullness.Value < 200)
            {
                minQuantity *= this.Config.StarvingPenalty;
                maxQuantity *= this.Config.StarvingPenalty;
            }

            minQuantity = Math.Min(minQuantity, maxQuantity);

            return (int)maxQuantity > 0
                ? Game1.random.Next((int)minQuantity, (int)maxQuantity)
                : 0;
        }

        private int CalculatePrice(FarmAnimal animal)
        {
            int price = -1;
            Dictionary<int, string> data = Game1.content.Load<Dictionary<int, string>>(Path.Combine("Data", "ObjectInformation"));

            if (data.ContainsKey(animal.meatIndex.Value))
            {
                string[] values = data[animal.meatIndex.Value].Split('/');
                price = int.Parse(values[1]);

                float multiplier = 1f;

                foreach (Farmer onlineFarmer in Game1.getOnlineFarmers())
                {
                    if (onlineFarmer.professions.Where(p => this.Config.Professions.Contains(p)).Any())
                    {
                        multiplier = this.Config.ProfessionMultiplier;
                    }
                }

                price = (int)(price * multiplier);
            }

            return price;
        }
    }
}
