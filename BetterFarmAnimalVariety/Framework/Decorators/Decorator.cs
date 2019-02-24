namespace BetterFarmAnimalVariety.Framework.Decorators
{
    public class Decorator
    {
        protected object Original;

        protected Decorator(object original)
        {
            this.Original = original;
        }

        protected T GetOriginal<T>()
        {
            return (T)this.Original;
        }
    }
}
