using StardewValley;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Butchery.Framework.Data
{
    class Animals
    {
        public List<AnimalType> Types = new List<AnimalType>();

        public static void Seed()
        {
            Api.Butchery api = new Api.Butchery();
            Dictionary<string, string> data = Game1.content.Load<Dictionary<string, string>>(Path.Combine("Data", "FarmAnimals"));
            Data.Animals animals = Constants.Static.Helper.Data.ReadJsonFile<Data.Animals>(Framework.Constants.Mod.AnimalsData)
                ?? new Data.Animals();

            foreach (string key in data.Keys)
            {
                if (!animals.Types.Where(o => o.Name == key).Any())
                {
                    string[] values = data[key].Split('/');
                    int width = int.Parse(values[16]);

                    api.RegisterAnimalType(key, width / 8, width / 2);
                }
            }
        }
    }
}
