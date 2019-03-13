using Harmony;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RoomForMoreAnimals
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        public const byte FullnessFull = 200;
        public const int NightTime = 1700;
        private const string TroughProperty = "Trough";
        private const string BackLayer = "Back";
        private const int HayIndex = 178;

        public override void Entry(IModHelper helper)
        {
            this.Helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            var harmony = HarmonyInstance.Create(this.ModManifest.UniqueID);

            MethodInfo original, prefix;

            original = typeof(AnimalHouse).GetMethod("isFull");
            prefix = typeof(ModEntry).GetMethod("IsFull");
            harmony.Patch(original, new HarmonyMethod(prefix), null);

            original = typeof(FarmAnimal).GetMethod("dayUpdate");
            prefix = typeof(ModEntry).GetMethod("DayUpdate");
            harmony.Patch(original, new HarmonyMethod(prefix), null);
        }

        // Prefix FarmAnimal.dayUpdate
        public static bool DayUpdate(ref FarmAnimal __instance, ref GameLocation environtment)
        {
            if (!(environtment is AnimalHouse animalHouse))
            {
                return true;
            }

            if (__instance.fullness.Value >= FullnessFull)
            {
                return true;
            }

            if (Game1.timeOfDay >= NightTime)
            {
                return true;
            }

            bool hasTrough = false;

            for (int xTile = 0; xTile < environtment.map.Layers[0].LayerWidth; ++xTile)
            {
                for (int yTile = 0; yTile < environtment.map.Layers[0].LayerHeight; ++yTile)
                {
                    if (environtment.doesTileHaveProperty(xTile, yTile, TroughProperty, BackLayer) != null)
                    {
                        hasTrough = true;

                        break;
                    }
                }

                if (hasTrough)
                {
                    break;
                }
            }

            if (!hasTrough)
            {
                return true;
            }

            IEnumerable<KeyValuePair<Vector2, Object>> hayObjects = environtment.objects.Pairs
                .Where(kvp => kvp.Value.ParentSheetIndex == HayIndex);

            if (hayObjects.Any())
            {
                environtment.objects.Remove(hayObjects.First().Key);
            }
            else if (Game1.getFarm().piecesOfHay.Value <= 0)
            {
                return true;
            }
            else
            {
                --Game1.getFarm().piecesOfHay.Value;
            }

            __instance.fullness.Value = byte.MaxValue;

            return true;
        }

        // Prefix: Building.isFull
        public static bool IsFull(ref AnimalHouse __instance, ref bool __result)
        {
            __result = false;

            return false;
        }
    }
}
