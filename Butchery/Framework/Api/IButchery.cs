namespace Butchery.Framework.Api
{
    public interface IButchery
    {
        void RegisterAnimalType(string type, int minMeat, int maxMeat);
    }
}
