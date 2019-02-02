using Microsoft.Xna.Framework;
using Netcode;
using StardewValley;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SustainableGrazing
{
    class FarmAnimal_Patch
    {
        public static void grassEndPointFunction_PostFix(PathNode currentPoint, Point endPoint, GameLocation location, Character c, ref bool __result)
        {
            if (__result == false)
            {
                return;
            }

            // Check again to see if the grass is tall enough
            Vector2 key = new Vector2((float)currentPoint.x, (float)currentPoint.y);
            TerrainFeature terrainFeature;

            if (!location.terrainFeatures.TryGetValue(key, out terrainFeature))
            {
                __result = false;
                return;
            }

            if (!(terrainFeature is Grass))
            {
                __result = false;
                return;
            }

            Grass grass = terrainFeature as Grass;

            if (grass.numberOfWeeds.Value < 4)
            {
                __result = false;
                return;
            }

            __result = true;
        }

        public static bool eatGrass_PreFix(ref GameLocation environment, FarmAnimal __instance)
        {
            Microsoft.Xna.Framework.Rectangle boundingBox = __instance.GetBoundingBox();
            double num1 = (double)(boundingBox.Center.X / 64);
            double num2 = (double)(boundingBox.Center.Y / 64);
            Vector2 local = new Vector2((float)num1, (float)num2);

            if (!environment.terrainFeatures.ContainsKey(local))
            {
                return false;
            }

            if (!(environment.terrainFeatures[local] is Grass))
            {
                return false;
            }

            Grass grass = (Grass)environment.terrainFeatures[local];

            // Only eat mature grass
            if (grass.numberOfWeeds.Value < 4)
            {
                return false;
            }

            FieldInfo isEating = __instance.GetType().GetField("isEating", BindingFlags.Instance | BindingFlags.NonPublic);

            isEating.SetValue(__instance, new NetBool(true));

            // Never remove the grass from grazing
            int reduceBy = __instance.isCoopDweller() ? 2 : 3;

            grass.numberOfWeeds.Value = (int)((NetFieldBase<int, NetInt>)grass.numberOfWeeds) - 1;


            Game1.showGlobalMessage($"numberOfWeeds: {grass.numberOfWeeds.Value}");


            // grass.reduceBy(reduceBy, local, environment.Equals(Game1.currentLocation));

            __instance.Sprite.loop = false;
            __instance.fullness.Value = byte.MaxValue;

            if (__instance.moodMessage.Value == 5 || __instance.moodMessage.Value == 6 || Game1.isRaining)
            {
                return false;
            }

            __instance.happiness.Value = byte.MaxValue;
            __instance.friendshipTowardFarmer.Value = Math.Min(1000, __instance.friendshipTowardFarmer.Value + 8);

            // rewrote function in whole, no need to continue
            return false;
        }
    }
}
