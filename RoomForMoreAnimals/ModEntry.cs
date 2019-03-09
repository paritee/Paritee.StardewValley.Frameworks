using Harmony;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace RoomForMoreAnimals
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            this.Helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            var harmony = HarmonyInstance.Create(this.ModManifest.UniqueID);
            var original = typeof(AnimalHouse).GetMethod("isFull");
            var prefix = typeof(ModEntry).GetMethod("IsFull");
            harmony.Patch(original, new HarmonyMethod(prefix), null);
        }

        public static bool IsFull(ref AnimalHouse __instance, ref bool __result)
        {
            __result = false;

            return false;
        }
    }
}
