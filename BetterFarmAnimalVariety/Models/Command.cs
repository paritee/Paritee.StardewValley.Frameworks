using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterFarmAnimalVariety.Models
{
    class Command
    {
        public string Name;
        public string Documentation;
        public Action<string, string[]> Callback;

        public Command(string name, string documentation, Action<string, string[]> callback)
        {
            this.Name = name;
            this.Documentation = documentation;
            this.Callback = callback;
        }
    }
}
