using System;
using StardewValley;

namespace TreatYourAnimals.Framework
{
    internal class FarmAnimalTreat : CharacterTreat
    {
        private const int FriendshipPointsMax = 1000;
        private const int FriendshipPointsStep = 6; // matches water bowl for pets
        private const int ExperiencePoints = 5; // matches pet() value

        private readonly FarmAnimal _farmAnimal;

        public FarmAnimalTreat(FarmAnimal farmAnimal, ModConfig config) : base(config)
        {
            _farmAnimal = farmAnimal;
        }

        public override void GiveTreat()
        {
            ReduceActiveItemByOne();
            ChangeFriendship();

            // Extras
            ChangeFullness();
            ChangeHappiness();
            GainExperience();

            DoEmote();
            PlaySound();
        }

        private void ChangeHappiness()
        {
            // same logic used in pet()
            if (ReceiveProfessionBoost())
            {
                _farmAnimal.happiness.Value = (byte)Math.Min(byte.MaxValue, _farmAnimal.happiness.Value + Math.Max(5, 40 - _farmAnimal.happinessDrain.Value));
            }
        }

        private void GainExperience()
        {
            // same logic used in pet()
            Game1.player.gainExperience((int)Skill.Skills.Farming, ExperiencePoints);
        }

        private void ChangeFullness()
        {
            _farmAnimal.fullness.Value = byte.MaxValue;
        }

        protected override void ChangeFriendship(int points = FriendshipPointsStep)
        {
            _farmAnimal.friendshipTowardFarmer.Value = Math.Max(0, Math.Min(FriendshipPointsMax, _farmAnimal.friendshipTowardFarmer.Value + points));

            var mailId = "farmAnimalLoveMessage" + _farmAnimal.myID.Value;

            // Chance to show the "pet loves you" global message
            AttemptToExpressLove(_farmAnimal, _farmAnimal.friendshipTowardFarmer.Value, FriendshipPointsMax, mailId);
        }

        private bool ReceiveProfessionBoost()
        {
            if (Game1.player.professions.Contains((int)Profession.Professions.Shepherd) && !_farmAnimal.isCoopDweller())
            {
                return true;
            }

            if (Game1.player.professions.Contains((int)Profession.Professions.Coopmaster) && _farmAnimal.isCoopDweller())
            {
                return true;
            }

            return false;
        }

        public override void DoEmote()
        {
            DoEmote(_farmAnimal);
        }

        public void RefuseTreat(bool penalty)
        {
            var pointsLoss = penalty ? -1 * (FriendshipPointsStep / 2) : 0;

            base.RefuseTreat(_farmAnimal, pointsLoss);
        }

        protected override void PlaySound()
        {
            _farmAnimal.makeSound();
        }
    }
}
