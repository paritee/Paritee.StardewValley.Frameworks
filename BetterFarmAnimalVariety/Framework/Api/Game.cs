using Newtonsoft.Json;
using StardewValley;
using StardewValley.Menus;
using System.Reflection;
using xTile.Dimensions;

namespace BetterFarmAnimalVariety.Framework.Api
{
    class Game
    {
        public static StardewValley.Multiplayer GetMultiplayer()
        {
            return Helpers.Reflection.GetFieldValue<StardewValley.Multiplayer>(typeof(Game1), "multiplayer", BindingFlags.Static | BindingFlags.NonPublic);
        }

        public static long GetNewId()
        {
            return Api.Game.GetMultiplayer().getNewID();
        }

        public static StardewValley.Farmer GetPlayer()
        {
            return Game1.player;
        }

        public static Farm GetFarm()
        {
            return Game1.getFarm();
        }

        public static bool IsSaveLoaded()
        {
            return Game1.hasLoadedGame;
        }

        public static bool ActiveMenuExists()
        {
            return Api.Game.GetActiveMenu() == null;
        }

        public static IClickableMenu GetActiveMenu()
        {
            return Game1.activeClickableMenu;
        }

        public static void ExitActiveMenu()
        {
            Game1.exitActiveMenu();
        }

        public static void NextEventCommand()
        {
            if (Game1.currentLocation.currentEvent != null)
            {
                ++Game1.currentLocation.currentEvent.CurrentCommand;
            }
        }

        public static bool IsFarmEvent<T>(out T farmEvent)
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

        public static Rectangle GetViewport()
        {
            return Game1.viewport;
        }

        public static T ReadSaveData<T>(string key)
        {
            return Game1.CustomData.TryGetValue(key, out string value)
                ? JsonConvert.DeserializeObject<T>(value)
                : default(T);
        }

        public static void WriteSaveData<T>(string key, T data)
        {
            if (data != null)
            {
                Game1.CustomData[key] = JsonConvert.SerializeObject(data, Formatting.None);
            }
            else
            {
                Game1.CustomData.Remove(key);
            }
        }
    }
}
