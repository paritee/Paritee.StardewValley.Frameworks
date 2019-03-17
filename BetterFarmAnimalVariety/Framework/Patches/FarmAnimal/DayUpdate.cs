using Harmony;
using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Patches.FarmAnimal
{
    [HarmonyPatch(typeof(StardewValley.FarmAnimal), "dayUpdate")]
    class DayUpdate
    {
        private const double FullnessChanceOdds = 200.0;
        private const double HappinessChanceOdds = 70.0;

        private const int WhiteChickenEgg = 176;
        private const int BrownChickenEgg = 180;
        private const int Wool = 440;
        private const int DuckEgg = 442;

        public static bool Prefix(ref StardewValley.FarmAnimal __instance, ref StardewValley.GameLocation environtment)
        {
            // Handle the only "return" in the DayUpdate method for animals that 
            // are sent back to their homes
            bool movedIntoHome = __instance.home != null
                && !(__instance.home.indoors.Value as StardewValley.AnimalHouse).animals.ContainsKey(__instance.myID.Value)
                && environtment is Farm
                && __instance.home.animalDoorOpen.Value;

            // pushAccumulator will always be set to 0 exiting postfix
            __instance.pushAccumulator = movedIntoHome ? 1 : 0;

            if (movedIntoHome)
            {
                return true;
            }
            
            // Temporarily use the pauseTimer to store the daysSinceLastLay value 
            // for use in the postfix
            __instance.pauseTimer = __instance.daysSinceLastLay.Value;

            // Temporarily use the hitGlowTimer to store the fullness value 
            // for use in the postfix
            __instance.hitGlowTimer = (__instance.fullness.Value < 200 || Game1.timeOfDay < 1700)
                && environtment is StardewValley.AnimalHouse
                && environtment.objects.Pairs.Where(kvp => kvp.Value.Name == "Hay").Any()
                ? PariteeCore.Characters.FarmAnimal.MaxFullness
                : __instance.fullness.Value;

            // Set the days to 0 so we have full control over the current produce
            __instance.daysSinceLastLay.Value = PariteeCore.Characters.FarmAnimal.MinDaysSinceLastLay;

            return true;
        }

        public static void Postfix(ref StardewValley.FarmAnimal __instance, ref StardewValley.GameLocation environtment)
        {
            if (__instance.pushAccumulator == 1)
            {
                __instance.pushAccumulator = 0;

                // Don't do anything
                return;
            }

            Decorators.FarmAnimal moddedAnimal = new Decorators.FarmAnimal(__instance);

            // Get back the days since last lay value and increment for today
            moddedAnimal.SetDaysSinceLastLay((byte)(moddedAnimal.GetPauseTimer() + 1));
            moddedAnimal.SetPauseTimer(PariteeCore.Characters.FarmAnimal.MinPauseTimer);

            // Get the original fullness used to determine the current produce
            byte fullness = (byte)moddedAnimal.GetHitGlowTimer();
            moddedAnimal.SetHitGlowTimer(PariteeCore.Characters.FarmAnimal.MinHitGlowTimer);

            DayUpdate.HandleCurrentProduce(ref moddedAnimal, fullness);
        }

        private static void HandleCurrentProduce(ref Decorators.FarmAnimal moddedAnimal, byte originalFullness)
        {
            // Determine the daysToLay using the owner's profession bonus
            byte daysToLay = moddedAnimal.GetDaysToLay(moddedAnimal.GetOwner());

            // Roll a random chance check
            int seed = (int)moddedAnimal.GetUniqueId() / 2 + PariteeCore.Utilities.Game.GetDaysPlayed();
            bool produceChance = DayUpdate.RollRandomProduceChance(moddedAnimal, originalFullness, seed);

            // Non-producers and babies do not produce
            if (!moddedAnimal.IsAProducer() || moddedAnimal.IsBaby())
            {
                moddedAnimal.SetCurrentProduce(PariteeCore.Characters.FarmAnimal.NoProduce);

                return;
            }
            // No reason to roll new produce ...
            else if (moddedAnimal.GetDaysSinceLastLay() < daysToLay)
            {
                return;
            }
            // ... otherwise; there was a reason to roll new produce, but it failed
            else if (!produceChance)
            {
                moddedAnimal.SetCurrentProduce(PariteeCore.Characters.FarmAnimal.NoProduce);

                return;
            }

            // Update unused game stats
            DayUpdate.HandleGameStats(moddedAnimal);

            // Roll the current produce
            StardewValley.Farmer owner = PariteeCore.Utilities.Game.GetPlayer();

            Cache.FarmAnimals cache = Helpers.FarmAnimals.ReadCache();

            string typeStr = moddedAnimal.GetTypeString();
            Cache.FarmAnimalType type = cache.Categories.SelectMany(o => o.Types).Where(o => o.Type == typeStr).FirstOrDefault();

            double deluxeProduceLuck = type == null
                ? default(double)
                : type.DeluxeProduceLuck;

            int parentSheetIndex = moddedAnimal.RollProduce(seed, owner, deluxeProduceLuck);

            moddedAnimal.SetCurrentProduce(parentSheetIndex);

            // Could have rolled no produce so no need to continue
            if (!PariteeCore.Characters.FarmAnimal.IsProduceAnItem(parentSheetIndex))
            {
                return;
            }

            // Reset the days counter
            moddedAnimal.SetDaysSinceLastLay(PariteeCore.Characters.FarmAnimal.MinDaysSinceLastLay);

            DayUpdate.HandleProduceQuality(moddedAnimal, seed);
            DayUpdate.HandleProduceSpawn(moddedAnimal);
        }

        private static bool RollRandomProduceChance(Decorators.FarmAnimal moddedAnimal, byte fullness, int seed)
        {
            // Set up for random chance check
            Random random = new Random(seed);
            byte happiness = moddedAnimal.GetHappiness();

            bool fullnessChance = random.NextDouble() < fullness / DayUpdate.FullnessChanceOdds;
            bool happinessChance = random.NextDouble() < happiness / DayUpdate.HappinessChanceOdds;

            return fullnessChance && happinessChance;
        }

        private static void HandleGameStats(Decorators.FarmAnimal moddedAnimal)
        {
            try
            {
                // Track vanilla stats that are not currently being used
                switch (moddedAnimal.GetDefaultProduce())
                {
                    case DayUpdate.WhiteChickenEgg:
                    case DayUpdate.BrownChickenEgg:
                        ++Game1.stats.ChickenEggsLayed;
                        break;
                    case DayUpdate.Wool:
                        ++Game1.stats.RabbitWoolProduced;
                        break;
                    case DayUpdate.DuckEgg:
                        ++Game1.stats.DuckEggsLayed;
                        break;
                }
            }
            catch
            {
                // Do nothing. Future proof in case these stats get removed in 
                // an update.
            }
        }

        private static void HandleProduceQuality(Decorators.FarmAnimal moddedAnimal, int seed)
        {
            // Roll the produce quality
            PariteeCore.Objects.Object.Quality produceQuality = moddedAnimal.RollProduceQuality(moddedAnimal.GetOwner(), seed);

            moddedAnimal.SetProduceQuality(produceQuality);
        }

        private static void HandleProduceSpawn(Decorators.FarmAnimal moddedAnimal)
        {
            // Only need to continue if the animal lays produce and has a home 
            // (i.e. does not find or require tool)
            if (!moddedAnimal.LaysProduce() || !moddedAnimal.HasHome())
            {
                return;
            }

            StardewValley.AnimalHouse animalHouse = PariteeCore.Locations.AnimalHouse.GetIndoors(moddedAnimal.GetHome());
            Vector2 tileLocation = moddedAnimal.GetTileLocation();

            // Check if an object exists on the spawn tile already
            if (animalHouse.Objects.ContainsKey(tileLocation))
            {
                return;
            }

            int produceIndex = moddedAnimal.GetCurrentProduce();
            int quality = moddedAnimal.GetProduceQuality();

            // Create the item
            StardewValley.Object obj = new StardewValley.Object(Vector2.Zero, produceIndex, (string)null, false, true, false, true)
            {
                Quality = quality
            };

            // Spawn the item
            PariteeCore.Locations.Location.SpawnObject(animalHouse, tileLocation, obj);

            // Remove the animal's produce
            moddedAnimal.SetCurrentProduce(PariteeCore.Characters.FarmAnimal.NoProduce);
        }
    }
}
