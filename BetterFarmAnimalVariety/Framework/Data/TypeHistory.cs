using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterFarmAnimalVariety.Framework.Data
{
    class TypeHistory
    {
        public long FarmAnimalId;
        public string CurrentType;
        public string SavedType;

        public TypeHistory(long farmAnimalId, string currentType, string savedType)
        {
            this.FarmAnimalId = farmAnimalId;
            this.CurrentType = currentType;
            this.SavedType = savedType;
        }

        public bool IsDirty()
        {
            return !Api.FarmAnimal.IsVanilla(this.SavedType);
        }

        public bool IsVanilla()
        {
            return Api.FarmAnimal.IsVanilla(this.CurrentType);
        }
    }
}
