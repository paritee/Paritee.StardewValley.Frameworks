using Harmony;
using Microsoft.Xna.Framework;
using StardewValley;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Patches.FarmAnimal
{
    [HarmonyPatch(typeof(StardewValley.FarmAnimal), "findTruffle")]
    class FindTruffle : Patch
    {
        public static bool Prefix(ref StardewValley.FarmAnimal __instance, ref Farmer who)
        {
            Decorators.FarmAnimal moddedAnimal = new Decorators.FarmAnimal(__instance);

            FindTruffle.AttemptToSpawnProduce(ref moddedAnimal, PariteeCore.Utilities.Game.GetMasterPlayer());

            if (FindTruffle.ShouldStopFindingProduce(ref moddedAnimal))
            {
                moddedAnimal.SetCurrentProduce(PariteeCore.Characters.FarmAnimal.NoProduce);
            }

            return false;
        }

        private static bool ShouldStopFindingProduce(ref Decorators.FarmAnimal moddedAnimal)
        {
            int seed = (int)(moddedAnimal.GetUniqueId() / 2 + PariteeCore.Utilities.Game.GetDaysPlayed() + PariteeCore.Utilities.Game.GetTimeOfDay());

            return PariteeCore.Utilities.Random.Seed(seed).NextDouble() > moddedAnimal.GetFriendship() / 1500.0;
        }

        private static bool AttemptToSpawnProduce(ref Decorators.FarmAnimal moddedAnimal, Farmer who)
        {
            Vector2 tileLocation = StardewValley.Utility.getTranslatedVector2(moddedAnimal.GetTileLocation(), moddedAnimal.GetFacingDirection(), 1f);

            // Don't roll the produce since we set this in dayUpdate postfix
            int produceIndex = moddedAnimal.GetCurrentProduce();

            if (!PariteeCore.Characters.FarmAnimal.IsProduceAnItem(produceIndex))
            {
                return false;
            }

            // Create the item
            StardewValley.Object obj = new StardewValley.Object(Vector2.Zero, produceIndex, (string)null, false, true, false, true)
            {
                Quality = moddedAnimal.GetProduceQuality()
            };

            // Spawn the item
            PariteeCore.Locations.Location.SpawnObject(PariteeCore.Utilities.Game.GetFarm(), tileLocation, obj);

            return true;
        }
    }
}
