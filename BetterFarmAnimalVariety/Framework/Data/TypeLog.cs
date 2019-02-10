namespace BetterFarmAnimalVariety.Framework.Data
{
    class TypeLog
    {
        public string CurrentType;
        public string SavedType;

        public TypeLog(string currentType, string savedType)
        {
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
