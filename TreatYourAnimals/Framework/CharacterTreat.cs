using System;
using System.Collections.Generic;
using StardewValley;
using Object = StardewValley.Object;

namespace TreatYourAnimals.Framework
{
    internal abstract class CharacterTreat
    {
        public const int InedibleThreshold = -300;

        private const string StringsLoveMessage = "Strings\\Characters:PetLovesYou";

        private enum Poisons
        {
            PaleAle = 303,
            Beer = 346,
            Wine = 348,
            Mead = 459,
            EnergyTonic = 349,
            MuscleRemedy = 351,
            Coffee = 395,
            ChocolateCake = 220,
            CoffeeBean = 433,
            OilOfGarlic = 772,
            IridiumMilk = 803,
            Garlic = 248,
            SpringOnion = 399,
            Leek = 20,
            Cherry = 638,
        }

        protected readonly ModConfig Config;

        protected CharacterTreat(ModConfig config)
        {
            Config = config;
        }

        protected static void ReduceActiveItemByOne()
        {
            Game1.player.reduceActiveItemByOne();
        }

        protected static void AttemptToExpressLove(Character character, int points, int pointsThreshold, string mailKey)
        {
            if (points < pointsThreshold || Game1.player.mailReceived.Contains(mailKey)) return;
            // "PetLovesYou": "{0} loves you. <"
            Game1.showGlobalMessage(Game1.content.LoadString(StringsLoveMessage, character.displayName));
            Game1.player.mailReceived.Add(mailKey);
        }

        protected void RefuseTreat(Character character, int points)
        {
            if (points != 0)
            {
                ChangeFriendship(points);
            }

            DoEmote(character, Emote.Emotes.Angry);
            PlaySound();
        }

        public abstract void GiveTreat();

        protected abstract void ChangeFriendship(int points);

        public abstract void DoEmote();

        protected static void DoEmote(Character character, Emote.Emotes emote = Emote.Emotes.Heart)
        {
            character.doEmote((int)emote);
        }

        protected abstract void PlaySound();

        public static bool IsPoisonous(Object item)
        {
            if (item.Edibility < 0)
            {
                return true;
            }

            // Animal poisons
            var poisons = new List<int>((int[])Enum.GetValues(typeof(Poisons)));

            return poisons.Contains(item.ParentSheetIndex);
        }

    }
}
