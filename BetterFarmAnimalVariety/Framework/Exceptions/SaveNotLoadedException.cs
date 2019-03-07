using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterFarmAnimalVariety.Framework.Exceptions
{
    class SaveNotLoadedException : Exception
    {
        public SaveNotLoadedException() : base($"Save has not been loaded") { }
    }
}
