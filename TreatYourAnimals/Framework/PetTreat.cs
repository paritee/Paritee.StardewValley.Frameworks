using System;
using StardewValley.Characters;

namespace TreatYourAnimals.Framework
{
    internal class PetTreat : CharacterTreat
    {
        private const int FriendshipPointsMax = 1000;
        private const int FriendshipPointsStep = 6; // matches water bowl value

        private const int ChocolateCake = 220;

        private readonly Pet _pet;

        public PetTreat(Pet pet, ModConfig config) : base(config)
        {
            _pet = pet;
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
            _pet.friendshipTowardFarmer.Value = Math.Max(0, Math.Min(FriendshipPointsMax, _pet.friendshipTowardFarmer.Value + points));

            // Chance to show the "pet loves you" global message
            AttemptToExpressLove(_pet, _pet.friendshipTowardFarmer.Value, FriendshipPointsMax, "petLoveMessage");
        }

        public void RefuseTreat(bool penalty = false)
        {
            var pointsLoss = penalty ? -1 * (FriendshipPointsStep / 2) : 0;

            base.RefuseTreat(_pet, pointsLoss);
        }

        public override void DoEmote()
        {
            DoEmote(_pet);
        }

        protected override void PlaySound()
        {
            _pet.playContentSound();
        }
    }
}
