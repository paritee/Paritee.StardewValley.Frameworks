using System.Collections.Generic;

namespace Butchery.Framework.Constants
{
    class Meat
    {
        public static Meat Beef => new Meat(639, "Beef", 500, -10);
        public static Meat Pork => new Meat(640, "Pork", 2600, -10);
        public static Meat Chicken => new Meat(641, "Chicken", 300, -10);
        public static Meat Duck => new Meat(642, "Duck", 1300, -10);
        public static Meat Rabbit => new Meat(643, "Rabbit", 2600, -10);
        public static Meat Mutton => new Meat(644, "Mutton", 2300, -10);

        public readonly int Index;
        private readonly string Name;
        private readonly int Price;
        private readonly int Edibility;
        private readonly string DisplayName;
        private readonly string Description;

        private const string Type = "Basic";
        private const int Category = -14;

        private Meat(int index, string name, int price, int edibility)
        {
            this.Index = index;
            this.Name = name;
            this.Price = price;
            this.Edibility = edibility;
            this.DisplayName = Static.i18n.Get($"Meat.{this.Name}.DisplayName");
            this.Description = Static.i18n.Get($"Meat.{this.Name}.Description");
        }

        public static List<Meat> All()
        {
            return new List<Meat>()
            {
                Meat.Beef,
                Meat.Chicken,
                Meat.Duck,
                Meat.Mutton,
                Meat.Pork,
                Meat.Rabbit,
            };
        }

        public string FormatData()
        {
            return string.Format("{0}/{1}/{2}/{3} {4}/{5}/{6}", this.Name, this.Price, this.Edibility, Meat.Type, Meat.Category, this.DisplayName, this.Description);
        }
    }
}
