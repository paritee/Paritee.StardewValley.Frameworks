using StardewValley;
using StardewValley.Characters;
using System;

namespace TreatYourAnimals.Framework
{
    class HorseTreat : CharacterTreat
    {
        private const int FRIENDSHIP_POINTS_MAX = 2000; // 2000 = Bouquet; 2500 = Sea Amulet
        private const int FRIENDSHIP_POINTS_STEP = 12; // same percentage as the other animals

        private Horse Horse;

        public HorseTreat(Horse horse, ModConfig config) : base(config)
        {
            this.Horse = horse;
        }

        public override void GiveTreat()
        {
            this.ReduceActiveItemByOne();
            this.ChangeFriendship();
            this.DoEmote();
            this.PlaySound();
        }

        public override void ChangeFriendship(int points = HorseTreat.FRIENDSHIP_POINTS_STEP)
        {
            // Block all friendship for the horse
            if (!this.Config.EnableHorseFriendship)
            {
                return;
            }

            string originalName = this.Horse.Name;
            string friendshipName = String.Join("_", new string[] { this.Horse.GetType().ToString(), originalName });

            // Temporarily change the horse's name to use the social NPC functions
            // This will be reset before exiting
            this.Horse.Name = friendshipName;

            // WARNING:
            // - Completing socialize quests will also boost your Horse's friendship poiints
            // - Counts towards friendship achievements
            // - Affects percentage that contributes towards percentGameComplete
            // - Will not show up on the social menu (@TODO: make this happen?)
            if (!Game1.player.friendshipData.ContainsKey(this.Horse.Name))
            {
                Game1.player.friendshipData.Add(this.Horse.Name, new Friendship());
                //Game1.player.hasPlayerTalkedToNPC(this.Horse.Name);
            }

            // Treat the horse as if it's a social NPC
            Game1.player.changeFriendship(points, this.Horse);

            // Set the name back
            this.Horse.Name = originalName;

            // Chance to show the "pet loves you" global message
            this.AttemptToExpressLove(this.Horse, Game1.player.getFriendshipLevelForNPC(friendshipName), HorseTreat.FRIENDSHIP_POINTS_MAX, "horseLoveMessage");
        }

        public void RefuseTreat(bool penalty)
        {
            int pointsLoss = penalty ? -1 * (HorseTreat.FRIENDSHIP_POINTS_STEP / 2) : 0;

            base.RefuseTreat(this.Horse, pointsLoss);
        }

        public override void DoEmote()
        {
            base.DoEmote(this.Horse);
        }

        public override void PlaySound()
        {
            Game1.playSound("grunt");
        }
    }
}
