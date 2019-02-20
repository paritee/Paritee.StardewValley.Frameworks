using StardewValley;
using StardewValley.Events;

namespace BetterFarmAnimalVariety.Framework.Api
{
    class Event
    {
        public static StardewValley.Event GetEventInLocation(ref GameLocation location)
        {
            return location.currentEvent;
        }

        public static bool IsEventOccurringInLocation(GameLocation location)
        {
            return Api.Event.GetEventInLocation(ref location) != null;
        }

        public static void GoToNextEventCommandInLocation(ref GameLocation location)
        {
            if (Api.Event.IsEventOccurringInLocation(location))
            {
                ++Api.Event.GetEventInLocation(ref location).CurrentCommand;
            }
        }

        public static bool TryGetFarmEvent<T>(out T farmEvent)
        {
            farmEvent = default(T);

            if (Game1.farmEvent == null)
            {
                return false;
            }

            if (!(Game1.farmEvent is T))
            {
                return false;
            }

            farmEvent = (T)Game1.farmEvent;

            return true;
        }

        public static bool IsFarmEventOccurring<T>(out T farmEvent)
        {
            return Api.Event.TryGetFarmEvent<T>(out farmEvent);
        }

        public static void ForceQuestionEventToProceed(ref QuestionEvent questionEvent)
        {
            questionEvent.forceProceed = true;
        }
    }
}
