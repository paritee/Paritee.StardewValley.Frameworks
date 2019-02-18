﻿using StardewValley;
using StardewValley.Menus;
using xTile.Dimensions;

namespace BetterFarmAnimalVariety.Framework.Api
{
    class Game
    {
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
    }
}