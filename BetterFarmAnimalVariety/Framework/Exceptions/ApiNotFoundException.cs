using System;

namespace BetterFarmAnimalVariety.Framework.Exceptions
{
    class ApiNotFoundException : Exception
    {
        public ApiNotFoundException(string apiKey) : base($"{Constants.Integration.MoreAnimals} API not found") { }
    }
}
