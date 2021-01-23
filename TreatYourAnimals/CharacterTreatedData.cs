using System.Collections.Generic;

namespace TreatYourAnimals
{
    internal class CharacterTreatedData
    {
        public readonly List<string> Characters = new List<string>();

        public static string FormatEntry(string type, string id)
        {
            return string.Join("_", type, id);
        }
    }
}
