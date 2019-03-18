using Butchery.Framework.Data;
using System.Linq;

namespace Butchery.Framework.Api
{
    public class Butchery : IButchery
    {
        public void RegisterAnimalType(string type, int minMeat, int maxMeat)
        {
            Data.Animals animals = Constants.Static.Helper.Data.ReadJsonFile<Data.Animals>(Framework.Constants.Mod.AnimalsData)
                ?? new Data.Animals();

            AnimalType animalType = animals.Types.FirstOrDefault(o => o.Name == type);

            if (animalType == null)
            {
                animals.Types.Add(new AnimalType(type, minMeat, maxMeat));
            }
            else
            {
                animalType.MinMeat = minMeat;
                animalType.MaxMeat = maxMeat;
            }

            Constants.Static.Helper.Data.WriteJsonFile<Framework.Data.Animals>(Framework.Constants.Mod.AnimalsData, animals);
        }
    }
}
