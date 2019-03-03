namespace BetterFarmAnimalVariety.Framework.ContentPacks
{
    class FarmAnimalCategory : Config.FarmAnimalCategory
    {
        public enum Actions
        {
            Create,
            Update,
            Remove
        }

        public Actions Action = Actions.Update;
        public bool ForceRemoveFromShop = false;
        public bool ForceOverrideTypes = false;
        public bool ForceOverrideBuildings = false;
        public bool ForceOverrideExclude = false;

        public FarmAnimalCategory() : base() { }

        public FarmAnimalCategory(Actions action, bool forceRemoveFromShop) : base()
        {
            this.Action = action;
            this.ForceRemoveFromShop = forceRemoveFromShop;
        }
    }
}
