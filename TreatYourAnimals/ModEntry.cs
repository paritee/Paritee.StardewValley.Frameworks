using System.Collections.Generic;
using System.Linq;
using Netcode;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Network;
using TreatYourAnimals.Framework;
using xTile.Dimensions;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace TreatYourAnimals
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        private ModConfig _config;
        private CharacterTreatedData _characterTreatedData;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            // Load config
            _config = Helper.ReadConfig<ModConfig>();

            // Events
            Helper.Events.Input.ButtonPressed += OnButtonPressed;
            Helper.Events.GameLoop.DayStarted += OnDayStarted;
            // TODO: Change cursor to gift on hover with a valid object to give to animal - use same rectangle
        }

        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            // Refresh the tracked character treats each day
            SetDailyTreatsData(new CharacterTreatedData());
        }

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // Ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
            {
                return;
            }

            // We only care about action buttons
            if (!e.Button.IsActionButton())
            {
                return;
            }

            // We only care if the player is holding an object ...
            if (Game1.player.ActiveObject == null)
            {
                return;
            }

            // ... and that object is sort-of edible
            if (Game1.player.ActiveObject.Edibility <= CharacterTreat.InedibleThreshold)
            {
                return;
            }

            //Vector2 index = new Vector2((float)((Game1.getOldMouseX() + Game1.viewport.X) / 64), (float)((Game1.getOldMouseY() + Game1.viewport.Y) / 64));
            var tileLocation = new Location((int)e.Cursor.GrabTile.X, (int)e.Cursor.GrabTile.Y);
            var rectangle = new Rectangle(tileLocation.X * 64, tileLocation.Y * 64, 64, 64);

            var intersected = false;

            switch (Game1.player.currentLocation)
            {
                case AnimalHouse _:
                {
                    if (Game1.player.currentLocation is AnimalHouse animalHouse)
                        intersected = AttemptToGiveTreatToFarmAnimals(animalHouse.animals, rectangle);
                    break;
                }
                case Farm _:
                {
                    if (Game1.player.currentLocation is Farm farm) intersected = AttemptToGiveTreatToFarmAnimals(farm.animals, rectangle);
                    break;
                }
            }

            if (!intersected)
            {
                intersected = AttemptToGiveTreatToHorsesAndPets(rectangle);
            }

            // Always suppress the button if we intersected as an attempt to treat
            // Blocks weird behaviour of mounting if you meant to treat a horse that was already treated
            if (intersected)
            {
                Helper.Input.Suppress(e.Button);
            }
        }

        private bool AttemptToGiveTreatToFarmAnimals(NetLongDictionary<FarmAnimal, NetRef<FarmAnimal>> animals, Rectangle rectangle)
        {
            foreach (var pair in animals.Pairs.Where(pair => pair.Value.GetBoundingBox().Intersects(rectangle)))
            {
                AttemptToGiveTreatToFarmAnimal(pair.Value);
                    
                // Intersects always return true
                return true;
            }

            return false;
        }

        private void AttemptToGiveTreatToFarmAnimal(FarmAnimal farmAnimal)
        {
            var type = farmAnimal.GetType().ToString();
            var id = farmAnimal.myID.ToString();

            var treatHandler = new FarmAnimalTreat(farmAnimal, _config);

            // Refuse a poisonous treat
            if (CharacterTreat.IsPoisonous(Game1.player.ActiveObject))
            {
                treatHandler.RefuseTreat(true);

                return;
            }

            // Can only give a treat once per day
            if (GivenTreatToday(type, id))
            {
                treatHandler.RefuseTreat(false);

                return;
            }

            treatHandler.GiveTreat();

            TrackGivenTreat(type, id);
        }

        private bool AttemptToGiveTreatToHorsesAndPets(Rectangle rectangle)
        {
            foreach (var character in Game1.player.currentLocation.characters.Where(character => character is Horse || character is Pet).Where(character => character.GetBoundingBox().Intersects(rectangle)))
            {
                if (character is Horse horse)
                {
                    // Check if horse has a name
                    if (horse.Name.Length <= 0)
                    {
                        // We don't want to stop the naming prompt even with an intercept
                        return false;
                    }

                    AttemptToGiveTreatToHorse(horse);
                }
                else
                {
                    AttemptToGiveTreatToPet(character as Pet);
                }

                // Intersects always return true
                return true;
            }

            return false;
        }

        private void AttemptToGiveTreatToHorse(Horse horse)
        {
            var type = horse.GetType().ToString();
            var id = horse.id.ToString();

            var treatHandler = new HorseTreat(horse, _config);

            // Refuse a poisonous treat
            if (CharacterTreat.IsPoisonous(Game1.player.ActiveObject))
            {
                treatHandler.RefuseTreat(true);

                return;
            }

            // Can only give a treat once per day
            if (GivenTreatToday(type, id))
            {
                treatHandler.RefuseTreat(false);

                return;
            }

            treatHandler.GiveTreat();

            TrackGivenTreat(type, id);
        }

        private void AttemptToGiveTreatToPet(Pet pet)
        {
            var type = pet.GetType().ToString();
            var id = pet.id.ToString();

            var treatHandler = new PetTreat(pet, _config);

            // Refuse a poisonous treat
            if (CharacterTreat.IsPoisonous(Game1.player.ActiveObject))
            {
                treatHandler.RefuseTreat(true);

                return;
            }

            // Can only give a treat once per day
            if (GivenTreatToday(type, id))
            {
                treatHandler.RefuseTreat();

                return;
            }

            treatHandler.GiveTreat();

            TrackGivenTreat(type, id);
        }

        private bool GivenTreatToday(string type, string id)
        {
            var model = GetDailyTreatsData();

            // Check if the entry already was treated
            return model.Characters.Contains(CharacterTreatedData.FormatEntry(type, id));
        }

        private void TrackGivenTreat(string type, string id)
        {
            var model = GetDailyTreatsData();
            var entry = CharacterTreatedData.FormatEntry(type, id);

            model.Characters.Add(entry);

            SetDailyTreatsData(model);
        }

        private CharacterTreatedData GetDailyTreatsData()
        {
            return _characterTreatedData;
        }

        private void SetDailyTreatsData(CharacterTreatedData model)
        {
            _characterTreatedData = model;
        }
    }
}
