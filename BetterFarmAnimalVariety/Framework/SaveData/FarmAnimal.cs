namespace BetterFarmAnimalVariety.Framework.SaveData
{
    public class FarmAnimal
    {
        public readonly long Id;
        public TypeLog TypeLog;

        public FarmAnimal(long id, TypeLog typeLog)
        {
            this.Id = id;
            this.TypeLog = typeLog;
        }

        public string GetSavedType()
        {
            return this.TypeLog.Saved;
        }
    }
}
