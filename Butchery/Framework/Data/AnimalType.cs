namespace Butchery.Framework.Data
{
    class AnimalType
    {
        public string Name;
        public int MinMeat;
        public int MaxMeat;

        public AnimalType(string name, int minMeat, int maxMeat)
        {
            this.Name = name;
            this.MinMeat = minMeat;
            this.MaxMeat = maxMeat;
        }
    }
}
