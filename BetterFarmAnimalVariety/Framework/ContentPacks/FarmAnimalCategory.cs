using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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

        [JsonConverter(typeof(StringEnumConverter))]
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
    }
}
