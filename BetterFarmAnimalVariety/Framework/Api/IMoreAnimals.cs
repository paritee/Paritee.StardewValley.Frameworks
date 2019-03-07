namespace BetterFarmAnimalVariety.Framework.Api
{
    public interface IMoreAnimals
    {
        void RegisterAnimalType(string id, bool hasBaby = true, bool canShear = false);
    }
}
