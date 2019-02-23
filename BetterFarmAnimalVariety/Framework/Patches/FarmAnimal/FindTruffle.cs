using Harmony;
using Microsoft.Xna.Framework;
using StardewValley;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Patches.FarmAnimal
{
    //[HarmonyPatch(typeof(StardewValley.FarmAnimal), "findTruffle")]
    class FindTruffle : Patch
    {
        public static bool Prefix(ref StardewValley.FarmAnimal __instance, ref Farmer who)
        {
            FindTruffle.AttemptToSpawnProduce(ref __instance, who);

            if (FindTruffle.ShouldStopFindingProduce(ref __instance))
            {
                PariteeCore.Api.FarmAnimal.SetCurrentProduce(__instance, PariteeCore.Constants.FarmAnimal.NoProduce);
            }

            return false;
        }

        private static bool ShouldStopFindingProduce(ref StardewValley.FarmAnimal __instance)
        {
            int seed = (int)(PariteeCore.Api.FarmAnimal.GetId(__instance) / 2 + PariteeCore.Api.Game.GetDaysPlayed() + PariteeCore.Api.Game.GetTimeOfDay());

            return PariteeCore.Helpers.Random.Seed(seed).NextDouble() > PariteeCore.Api.FarmAnimal.GetFriendship(__instance) / 1500.0;
        }

        private static bool AttemptToSpawnProduce(ref StardewValley.FarmAnimal __instance, Farmer who)
        {
            Vector2 tileLocation = StardewValley.Utility.getTranslatedVector2(PariteeCore.Api.FarmAnimal.GetTileLocation(__instance), PariteeCore.Api.FarmAnimal.GetFacingDirection(__instance), 1f);
            int quantity = 1;
            int produceIndex = PariteeCore.Api.FarmAnimal.RollProduce(__instance, who);

            if (PariteeCore.Api.FarmAnimal.IsProduceAnItem(produceIndex))
            {
                return false;
            }

            StardewValley.Object obj = new StardewValley.Object(PariteeCore.Api.FarmAnimal.GetTileLocation(__instance), produceIndex, quantity);

            PariteeCore.Api.Location.SpawnObject(PariteeCore.Api.Game.GetFarm(), tileLocation, obj);

            return true;
        }
    }
}
