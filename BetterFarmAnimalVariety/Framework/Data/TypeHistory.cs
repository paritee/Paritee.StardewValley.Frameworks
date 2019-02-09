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
            return !Helpers.Utilities.IsVanillaFarmAnimalType(this.SavedType);
        }

        public bool IsVanilla()
        {
            return Helpers.Utilities.IsVanillaFarmAnimalType(this.CurrentType);
        }
    }
}
