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
            if (!PariteeCore.Api.FarmAnimal.HasHome(__instance))
            {
                return true;
            }

            if (PariteeCore.Api.FarmAnimal.IsEating(__instance))
            {
                return true;
            }

            if (Game1.IsClient || __instance.controller != null)
            {
                return true;
            }

            try
            {
                Behaviors.HandleFindGrassToEat(ref __instance, location);

                if (!Behaviors.HandleNightTimeRoutine(ref __instance, location))
                {
                    Behaviors.HandleFindProduce(ref __instance, location);
                }

                __result = true;
            }
            catch
            {
                __result = false;
            }

            return false;
        }

        private static void HandleFindGrassToEat(ref StardewValley.FarmAnimal __instance, StardewValley.GameLocation location)
        {
            if (PariteeCore.Api.Location.IsOutdoors(location) && PariteeCore.Api.FarmAnimal.GetFullness(__instance) < 195 && (PariteeCore.Helpers.Random.NextDouble() < 0.002 && PariteeCore.Api.FarmAnimal.UnderMaxPathFindingPerTick()))
            {
                PariteeCore.Api.FarmAnimal.IncreasePathFindingThisTick();
                PariteeCore.Api.FarmAnimal.SetFindGrassPathController(__instance, location);
            }
        }

        private static bool HandleNightTimeRoutine(ref StardewValley.FarmAnimal __instance, StardewValley.GameLocation location)
        {
            if (PariteeCore.Api.Game.GetTimeOfDay() >= 1700 && PariteeCore.Api.Location.IsOutdoors(location) && (__instance.controller == null && PariteeCore.Helpers.Random.NextDouble() < 0.002))
            {
                if (PariteeCore.Api.Location.AnyFarmers(location))
                {
                    PariteeCore.Api.Location.RemoveAnimal(location as Farm, __instance);
                    PariteeCore.Api.FarmAnimal.ReturnHome(__instance);

                    return true;
                }

                if (PariteeCore.Api.FarmAnimal.UnderMaxPathFindingPerTick())
                {
                    PariteeCore.Api.FarmAnimal.IncreasePathFindingThisTick();
                    PariteeCore.Api.FarmAnimal.SetFindHomeDoorPathController(__instance, location);
                }
            }

            return false;
        }

        private static void AssertValidLocation(StardewValley.GameLocation location)
        {
            if (!PariteeCore.Api.Location.IsOutdoors(location))
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

        private static void AssertCanFindProduce(StardewValley.FarmAnimal animal, Farmer player)
        {
            int produceIndex = PariteeCore.Api.FarmAnimal.RollProduce(animal, player);

            if (PariteeCore.Api.FarmAnimal.IsProduceAnItem(produceIndex))
            {
                throw new Exception();
            }

            if (!PariteeCore.Api.FarmAnimal.CanFindProduce(animal))
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

        private static void AssertNoImpediments(StardewValley.FarmAnimal animal, StardewValley.GameLocation location)
        {
            Microsoft.Xna.Framework.Rectangle boundingBox = animal.GetBoundingBox();

            for (int corner = 0; corner < 4; ++corner)
            {
                Vector2 cornersOfThisRectangle = StardewValley.Utility.getCornersOfThisRectangle(ref boundingBox, corner);
                Vector2 key = new Vector2(cornersOfThisRectangle.X / 64.0f, cornersOfThisRectangle.Y / 64.0f);

                if (location.terrainFeatures.ContainsKey(key) || location.objects.ContainsKey(key))
                {
                    throw new Exception();
                }
            }
        }

        private static void HandleFindProduce(ref StardewValley.FarmAnimal __instance, StardewValley.GameLocation location)
        {
            StardewValley.Farmer player = PariteeCore.Api.Game.GetPlayer();

            Behaviors.AssertValidLocation(location);
            Behaviors.AssertCanFindProduce(__instance, player);
            Behaviors.AssertRollChance();
            Behaviors.AssertNoImpediments(__instance, location);

            if (PariteeCore.Api.Farmer.IsCurrentLocation(player, location))
            {
                PariteeCore.Api.BellsAndWhistles.PlaySound("dirtyHit", 450);
                PariteeCore.Api.BellsAndWhistles.PlaySound("dirtyHit", 900);
                PariteeCore.Api.BellsAndWhistles.PlaySound("dirtyHit", 1350);
            }

            if (PariteeCore.Api.Game.IsCurrentLocation(location))
            {
                PariteeCore.Api.FarmAnimal.AnimateFindingProduce(__instance);
            }
            else
            {
                PariteeCore.Api.FarmAnimal.FindProduce(__instance, player);
            }
        }
    }
}
