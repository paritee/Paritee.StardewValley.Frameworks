using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using StardewValley;
using System;

namespace BetterFarmAnimalVariety.Framework.Api
{
    class BellsAndWhistles
    {
        public static void PlaySound(string cueName)
        {
            Game1.playSound(cueName);
        }

        public static bool HasSoundBank()
        {
            return Game1.soundBank != null;
        }

        public static void CueSound(string sound, string name, float value)
        {
            if (!Api.BellsAndWhistles.HasSoundBank())
            {
                return;
            }

            Cue cue = Game1.soundBank.GetCue(sound);
            cue.SetVariable(name, value);
            cue.Play();
        }

        public static void FadeToBlack(bool global = true, Action afterFade = null, float fadeSpeed = 0.02f)
        {
            if (global)
            {
                Game1.globalFadeToBlack(new Game1.afterFadeFunction(afterFade), fadeSpeed);
            }
            else
            {
                Game1.fadeScreenToBlack();
            }
        }

        public static bool IsFaded()
        {
            return Game1.globalFade;
        }

        public static void AddHudMessage(string message)
        {
            Game1.addHUDMessage(new HUDMessage(message));
        }

        public static void AddHudMessage(string message, bool achievement)
        {
            Game1.addHUDMessage(new HUDMessage(message, achievement));
        }

        public static void AddHudMessage(string message, int whatType)
        {
            Game1.addHUDMessage(new HUDMessage(message, whatType));
        }

        public static void AddHudMessage(string message, string leaveMeNull)
        {
            Game1.addHUDMessage(new HUDMessage(message, leaveMeNull));
        }

        public static void AddHudMessage(string message, Color color, float timeLeft, bool fadeIn = false)
        {
            Game1.addHUDMessage(new HUDMessage(message, color, timeLeft, fadeIn));
        }

        public static void AddHudMessage(string type, int number, bool add, Color color, Item messageSubject = null)
        {
            Game1.addHUDMessage(new HUDMessage(type, number, add, color, messageSubject));
        }
    }
}
