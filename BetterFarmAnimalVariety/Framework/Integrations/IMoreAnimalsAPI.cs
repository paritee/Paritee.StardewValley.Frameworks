namespace BetterFarmAnimalVariety.Framework.Integrations
{
    public interface IMoreAnimalsAPI
    {
        void RegisterAnimalType(string id, bool hasBaby = true, bool canShear = false);
    }
}
