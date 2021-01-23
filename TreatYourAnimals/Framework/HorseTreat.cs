using StardewValley;
using StardewValley.Characters;

namespace TreatYourAnimals.Framework
{
    class HorseTreat : CharacterTreat
    {
        private const int FriendshipPointsMax = 2000; // 2000 = Bouquet; 2500 = Sea Amulet
        private const int FriendshipPointsStep = 12; // same percentage as the other animals

        private readonly Horse _horse;

        public HorseTreat(Horse horse, ModConfig config) : base(config)
        {
            _horse = horse;
        }

        public override void GiveTreat()
        {
            ReduceActiveItemByOne();
            ChangeFriendship();
            DoEmote();
            PlaySound();
        }

        protected override void ChangeFriendship(int points = FriendshipPointsStep)
        {
            // Block all friendship for the horse
            if (!Config.EnableHorseFriendship)
            {
                return;
            }

            // WARNING:
            // - Completing socialize quests will also boost your Horse's friendship poiints
            // - Counts towards friendship achievements
            // - Affects percentage that contributes towards percentGameComplete
            // - Will not show up on the social menu (@TODO: make this happen?)

            // A horse's name gets appended with spaces if it matches an already existing NPC name
            // Horse.cs:nameHorse(string name)
            // if (allCharacter.isVillager() && allCharacter.Name.Equals(name))
            //    name += " ";
            if (!Game1.player.friendshipData.ContainsKey(_horse.Name))
            {
                Game1.player.friendshipData.Add(_horse.Name, new Friendship());
            }

            // Treat the horse as if it's a social NPC
            Game1.player.changeFriendship(points, _horse);

            // Chance to show the "pet loves you" global message
            AttemptToExpressLove(_horse, Game1.player.getFriendshipLevelForNPC(_horse.Name), FriendshipPointsMax, "horseLoveMessage");
        }

        public void RefuseTreat(bool penalty)
        {
            var pointsLoss = penalty ? -1 * (FriendshipPointsStep / 2) : 0;

            base.RefuseTreat(_horse, pointsLoss);
        }

        public override void DoEmote()
        {
            DoEmote(_horse);
        }

        protected override void PlaySound()
        {
            Game1.playSound("grunt");
        }
    }
}
