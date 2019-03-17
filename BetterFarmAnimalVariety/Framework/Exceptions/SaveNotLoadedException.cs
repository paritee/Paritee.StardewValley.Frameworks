using System;

namespace BetterFarmAnimalVariety.Framework.Exceptions
{
    [Serializable]
    class SaveNotLoadedException : Exception
    {
        public SaveNotLoadedException() : base($"Save has not been loaded") { }
    }
}
