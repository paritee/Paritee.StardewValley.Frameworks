using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Api
{
    class Farmer
    {
        public static bool CanAfford(StardewValley.Farmer farmer, int amount)
        {
            return farmer.Money >= amount;
        }

        public static void SpendMoney(StardewValley.Farmer farmer, int amount)
        {
            farmer.Money -= amount;
        }

        public static long GetUniqueId(StardewValley.Farmer farmer)
        {
            return farmer.UniqueMultiplayerID;
        }

        public static bool HasSeenEvent(StardewValley.Farmer farmer, int eventId)
        {
            return farmer.eventsSeen.Contains(eventId);
        }

        public static bool HasCompletedQuest(StardewValley.Farmer farmer, int questId)
        {
            return farmer.questLog.Where(o => o.id.Value.Equals(questId) && o.completed.Value).Any();
        }
    }
}
