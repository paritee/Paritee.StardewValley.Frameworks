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

            FindTruffle.AttemptToSpawnProduce(ref moddedAnimal, PariteeCore.Api.Game.GetMasterPlayer());

            if (FindTruffle.ShouldStopFindingProduce(ref moddedAnimal))
            {
                moddedAnimal.SetCurrentProduce(PariteeCore.Constants.FarmAnimal.NoProduce);
            }

            return false;
        }

        private static bool ShouldStopFindingProduce(ref Decorators.FarmAnimal moddedAnimal)
        {
            int seed = (int)(moddedAnimal.GetUniqueId() / 2 + PariteeCore.Api.Game.GetDaysPlayed() + PariteeCore.Api.Game.GetTimeOfDay());

            return PariteeCore.Helpers.Random.Seed(seed).NextDouble() > moddedAnimal.GetFriendship() / 1500.0;
        }

        private static bool AttemptToSpawnProduce(ref Decorators.FarmAnimal moddedAnimal, Farmer who)
        {
            Vector2 tileLocation = StardewValley.Utility.getTranslatedVector2(moddedAnimal.GetTileLocation(), moddedAnimal.GetFacingDirection(), 1f);
            int quantity = 1;
            int produceIndex = moddedAnimal.RollProduce(who);


            if (!PariteeCore.Api.FarmAnimal.IsProduceAnItem(produceIndex))
            {
                return false;
            }

            StardewValley.Object obj = new StardewValley.Object(moddedAnimal.GetTileLocation(), produceIndex, quantity);

            PariteeCore.Api.Location.SpawnObject(PariteeCore.Api.Game.GetFarm(), tileLocation, obj);

            return true;
        }
    }
}
