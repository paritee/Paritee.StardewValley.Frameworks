using Harmony;
using Microsoft.Xna.Framework;
using StardewValley;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Patches.FarmAnimal
{
    [HarmonyPatch(typeof(StardewValley.FarmAnimal), "behaviors")]
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

            Behaviors.HandleFindGrassToEat(ref moddedAnimal, ref moddedLocation);

            if (Behaviors.HandleNightTimeRoutine(ref moddedAnimal, ref moddedLocation))
            {
                __result = true;
            }
            else
            {
                Behaviors.HandleFindProduce(ref moddedAnimal, ref moddedLocation);

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

        private static bool IsValidLocation(Decorators.Location moddedLocation)
        {
            return moddedLocation.IsOutdoors() && !PariteeCore.Api.Weather.IsRaining() && !PariteeCore.Api.Season.IsWinter();
        }

        private static bool CanFindProduce(Decorators.FarmAnimal moddedAnimal, Decorators.Farmer moddedPlayer)
        {
            if (moddedAnimal.IsBaby())
            {
                return false;
            }

            if (!moddedAnimal.CanFindProduce())
            {
                return false;
            }

            int currentProduce = moddedAnimal.GetCurrentProduce();

            return PariteeCore.Api.FarmAnimal.IsProduceAnItem(currentProduce);
        }
        
        private static bool HasNoImpediments(Decorators.FarmAnimal moddedAnimal, Decorators.Location moddedLocation)
        {
            Microsoft.Xna.Framework.Rectangle boundingBox = moddedAnimal.GetBoundingBox();

            for (int corner = 0; corner < 4; ++corner)
            {
                Vector2 cornersOfThisRectangle = StardewValley.Utility.getCornersOfThisRectangle(ref boundingBox, corner);
                Vector2 key = new Vector2(cornersOfThisRectangle.X / 64.0f, cornersOfThisRectangle.Y / 64.0f);

                if (moddedLocation.GetOriginal().terrainFeatures.ContainsKey(key) || moddedLocation.GetOriginal().objects.ContainsKey(key))
                {
                    return false;
                }
            }

            return true;
        }

        private static void HandleFindProduce(ref Decorators.FarmAnimal moddedAnimal, ref Decorators.Location moddedLocation)
        {
            Decorators.Farmer moddedPlayer = new Decorators.Farmer(PariteeCore.Api.Game.GetPlayer());

            if (!Behaviors.IsValidLocation(moddedLocation))
            {
                return;
            }

            if (!Behaviors.CanFindProduce(moddedAnimal, moddedPlayer))
            {
                return;
            }

            if (PariteeCore.Helpers.Random.NextDouble() < 0.0002)
            {
                return;
            }

            if (!Behaviors.HasNoImpediments(moddedAnimal, moddedLocation))
            {
                return;
            }

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
