using Newtonsoft.Json;

namespace BetterFarmAnimalVariety.Framework.ContentPacks
{
    class FarmAnimalCategory : Cache.FarmAnimalCategory
    {
        public enum Actions
        {
            Create,
            Update,
            Remove
        }

        public Actions Action = Actions.Update;

        [JsonProperty(Order = 999)]
        public bool ForceRemoveFromShop = false;
        [JsonProperty(Order = 999)]
        public bool ForceOverrideTypes = false;
        [JsonProperty(Order = 999)]
        public bool ForceOverrideBuildings = false;
        [JsonProperty(Order = 999)]
        public bool ForceOverrideExclude = false;

        public FarmAnimalCategory() : base() { }

        public FarmAnimalCategory(Actions action) : base()
        {
            this.Action = action;
        }

        public bool ShouldSerializeAction()
        {
            return false;
        }
    }
}
