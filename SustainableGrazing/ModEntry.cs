using Harmony;
using Microsoft.Xna.Framework;
using Netcode;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SustainableGrazing
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            var harmony = HarmonyInstance.Create("paritee.sustainablegrazing");

            MethodInfo targetMethod;
            HarmonyMethod postfixMethod, prefixMethod;

            targetMethod = AccessTools.Method(typeof(FarmAnimal), "grassEndPointFunction");
            postfixMethod = new HarmonyMethod(typeof(FarmAnimal_Patch).GetMethod("grassEndPointFunction_PostFix"));
            harmony.Patch(targetMethod, null, postfixMethod);

            targetMethod = AccessTools.Method(typeof(FarmAnimal), "eatGrass");
            prefixMethod = new HarmonyMethod(typeof(FarmAnimal_Patch).GetMethod("eatGrass_PreFix"));
            harmony.Patch(targetMethod, prefixMethod, null);

            //helper.Events.GameLoop.UpdateTicking += this.OnUpdateTicking;
        }
        
        public void OnUpdateTicking(object sender, UpdateTickingEventArgs e)
        {
            // Ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
            {
                return;
            }

            if (!(Game1.currentLocation is Farm))
            {
                return;
            }

            Farm farm = Game1.currentLocation as Farm;

            foreach (KeyValuePair<long, FarmAnimal> pair in farm.animals.Pairs)
            {
                bool isEating = this.Helper.Reflection.GetField<NetBool>(pair.Value, "isEating").GetValue();

                if (!isEating)
                {
                    continue;
                }
                
                Microsoft.Xna.Framework.Rectangle boundingBox = pair.Value.GetBoundingBox();
                double num1 = (double)(boundingBox.Center.X / 64);
                double num2 = (double)(boundingBox.Center.Y / 64);
                Vector2 local = new Vector2((float)num1, (float)num2);

                this.Monitor.Log($"{pair.Value.Name} is eating", LogLevel.Debug);

                // Check if the grass has been eaten yet
                if (farm.terrainFeatures.ContainsKey(local) && (farm.terrainFeatures[local] is Grass))
                {
                    // not eaten yet
                    this.Monitor.Log($"grass not eaten yet", LogLevel.Debug);
                }
                else
                {
                    this.Monitor.Log($"ate grass", LogLevel.Debug);

                    // Add it back as short
                    Grass grass = new Grass(Grass.springGrass, 1);
                    farm.terrainFeatures.Add(local, grass);
                }
            }
        }
    }
}
