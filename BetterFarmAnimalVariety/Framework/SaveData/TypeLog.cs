using System.Collections.Generic;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.SaveData
{
    public class TypeLog
    {
        public readonly string Current;
        public readonly string Saved;

        public TypeLog(string current, string saved)
        {
            this.Current = current;
            this.Saved = saved;
        }

        public bool IsDirty()
        {
            return !PariteeCore.Characters.FarmAnimal.IsVanillaType(this.Saved);
        }

        public bool IsVanilla()
        {
            return PariteeCore.Characters.FarmAnimal.IsVanillaType(this.Current);
        }

        public KeyValuePair<string, string> ConvertToKeyValuePair()
        {
            return new KeyValuePair<string, string>(this.Current, this.Saved);
        }
    }
}
