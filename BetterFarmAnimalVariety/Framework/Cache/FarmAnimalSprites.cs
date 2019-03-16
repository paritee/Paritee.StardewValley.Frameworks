namespace BetterFarmAnimalVariety.Framework.Cache
{
    class FarmAnimalSprites
    {
        public string Baby;
        public string Adult;
        public string ReadyForHarvest;

        public FarmAnimalSprites() { }

        public FarmAnimalSprites(string baby, string adult, string readyForHarvest)
        {
            this.Baby = baby;
            this.Adult = adult;
            this.ReadyForHarvest = readyForHarvest;
        }
    }
}
