using Harmony;
using Microsoft.Xna.Framework;
using StardewValley;
using System;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Patches.FarmAnimal
{
    //[HarmonyPatch(typeof(StardewValley.FarmAnimal), "behaviors")]
    class Behaviors
    {
        public static bool Prefix(ref StardewValley.FarmAnimal __instance, ref GameTime time, ref GameLocation location, ref bool __result)
        {
            Decorators.FarmAnimal moddedAnimal = new Decorators.FarmAnimal(__instance);

            if (!moddedAnimal.HasHome())
            {
                return true;
            }

            if (moddedAnimal.IsEating())
            {
                return true;
            }

            if (Game1.IsClient || __instance.controller != null)
            {
                return true;
            }

            Decorators.Location moddedLocation = new Decorators.Location(location);

            try
            {
                Behaviors.HandleFindGrassToEat(ref moddedAnimal, ref moddedLocation);

                if (!Behaviors.HandleNightTimeRoutine(ref moddedAnimal, ref moddedLocation))
                {
                    Behaviors.HandleFindProduce(ref moddedAnimal, ref moddedLocation);
                }

                __result = true;
            }
            catch
            {
                __result = false;
            }

            return false;
        }

        private static void HandleFindGrassToEat(ref Decorators.FarmAnimal moddedAnimal, ref Decorators.Location moddedLocation)
        {
            if (moddedLocation.IsOutdoors() && moddedAnimal.GetFullness() < 195 && (PariteeCore.Helpers.Random.NextDouble() < 0.002 && PariteeCore.Api.FarmAnimal.UnderMaxPathFindingPerTick()))
            {
                PariteeCore.Api.FarmAnimal.IncreasePathFindingThisTick();
                moddedAnimal.SetFindGrassPathController(moddedLocation.GetOriginal());
            }
        }

        private static bool HandleNightTimeRoutine(ref Decorators.FarmAnimal moddedAnimal, ref Decorators.Location moddedLocation)
        {
            if (PariteeCore.Api.Game.GetTimeOfDay() < 1700)
            {
                return false;
            }

            if (!moddedLocation.IsOutdoors())
            {
                return false;
            }

            if (moddedAnimal.HasController())
            {
                return false;
            }

            if (PariteeCore.Helpers.Random.NextDouble() >- 0.002)
            {
                return false;
            }

            if (moddedLocation.AnyFarmers())
            {
                moddedLocation.RemoveAnimal(moddedAnimal.GetOriginal());
                moddedAnimal.ReturnHome();

                return true;
            }

            if (PariteeCore.Api.FarmAnimal.UnderMaxPathFindingPerTick())
            {
                PariteeCore.Api.FarmAnimal.IncreasePathFindingThisTick();
                moddedAnimal.SetFindHomeDoorPathController(moddedLocation.GetOriginal());
            }

            return false;
        }

        private static void AssertValidLocation(Decorators.Location moddedLocation)
        {
            if (!moddedLocation.IsOutdoors())
            {
                throw new Exception();
            }

            if (PariteeCore.Api.Weather.IsRaining())
            {
                throw new Exception();
            }

            if (PariteeCore.Api.Season.IsWinter())
            {
                throw new Exception();
            }
        }

        private static void AssertCanFindProduce(Decorators.FarmAnimal moddedAnimal, Decorators.Farmer moddedPlayer)
        {
            int produceIndex = moddedAnimal.RollProduce(moddedPlayer.GetOriginal());

            if (PariteeCore.Api.FarmAnimal.IsProduceAnItem(produceIndex))
            {
                throw new Exception();
            }

            if (!moddedAnimal.CanFindProduce())
            {
                throw new Exception();
            }
        }

        private static void AssertRollChance()
        {
            if (PariteeCore.Helpers.Random.NextDouble() < 0.0002)
            {
                throw new Exception();
            }
        }

        private static void AssertNoImpediments(Decorators.FarmAnimal moddedAnimal, Decorators.Location moddedLocation)
        {
            Microsoft.Xna.Framework.Rectangle boundingBox = moddedAnimal.GetBoundingBox();

            for (int corner = 0; corner < 4; ++corner)
            {
                Vector2 cornersOfThisRectangle = StardewValley.Utility.getCornersOfThisRectangle(ref boundingBox, corner);
                Vector2 key = new Vector2(cornersOfThisRectangle.X / 64.0f, cornersOfThisRectangle.Y / 64.0f);

                if (moddedLocation.GetOriginal().terrainFeatures.ContainsKey(key) || moddedLocation.GetOriginal().objects.ContainsKey(key))
                {
                    throw new Exception();
                }
            }
        }

        private static void HandleFindProduce(ref Decorators.FarmAnimal moddedAnimal, ref Decorators.Location moddedLocation)
        {
            Decorators.Farmer moddedPlayer = new Decorators.Farmer(PariteeCore.Api.Game.GetPlayer());

            Behaviors.AssertValidLocation(moddedLocation);
            Behaviors.AssertCanFindProduce(moddedAnimal, moddedPlayer);
            Behaviors.AssertRollChance();
            Behaviors.AssertNoImpediments(moddedAnimal, moddedLocation);

            if (moddedPlayer.IsCurrentLocation(moddedLocation.GetOriginal()))
            {
                PariteeCore.Api.BellsAndWhistles.PlaySound("dirtyHit", 450);
                PariteeCore.Api.BellsAndWhistles.PlaySound("dirtyHit", 900);
                PariteeCore.Api.BellsAndWhistles.PlaySound("dirtyHit", 1350);
            }

            if (PariteeCore.Api.Game.IsCurrentLocation(moddedLocation.GetOriginal()))
            {
                moddedAnimal.AnimateFindingProduce();
            }
            else
            {
                moddedAnimal.FindProduce(moddedPlayer.GetOriginal());
            }
        }
    }
}
