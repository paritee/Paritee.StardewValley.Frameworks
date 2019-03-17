using System.Collections.Generic;
using StardewModdingAPI;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Editors
{
    class AnimalBirth : IAssetEditor
    {
        private readonly IModHelper Helper;

        public AnimalBirth(IModHelper helper)
        {
            this.Helper = helper;
        }

        /// <summary>Get whether this instance can edit the given asset.</summary>
        /// <param name="asset">Basic metadata about the asset being loaded.</param>
        public bool CanEdit<T>(IAssetInfo asset)
        {
            if (asset.AssetNameEquals("Strings/Events"))
            {
                return true;
            }


            if (asset.AssetNameEquals("Strings/Locations"))
            {
                return true;
            }

            return false;
        }

        /// <summary>Edit a matched asset.</summary>
        /// <param name="asset">A helper which encapsulates metadata about an asset and enables changes to it.</param>
        public void Edit<T>(IAssetData asset)
        {
            if (asset.AssetNameEquals("Strings/Events"))
            {
                IDictionary<string, string> strings = asset.AsDictionary<string, string>().Data;

                // Remove the short parent type to allow for potential to expand outside the parent's type
                strings["AnimalBirth"] = this.Helper.Translation.Get("Strings.Events.AnimalBirth");
                strings["AnimalNamingTitle"] = PariteeCore.Utilities.Content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11357");
            }

            if (asset.AssetNameEquals("Strings/Locations"))
            {
                IDictionary<string, string> strings = asset.AsDictionary<string, string>().Data;

                // Set all of the hatching strings to be the same and type-agnostic
                strings["AnimalHouse_Incubator_Hatch_RegularEgg"] = this.Helper.Translation.Get("Strings.Locations.AnimalHouse_Incubator_Hatch");
                strings["AnimalHouse_Incubator_Hatch_VoidEgg"] = this.Helper.Translation.Get("Strings.Locations.AnimalHouse_Incubator_Hatch");
                strings["AnimalHouse_Incubator_Hatch_DuckEgg"] = this.Helper.Translation.Get("Strings.Locations.AnimalHouse_Incubator_Hatch");
                strings["AnimalHouse_Incubator_Hatch_DinosaurEgg"] = this.Helper.Translation.Get("Strings.Locations.AnimalHouse_Incubator_Hatch");
            }
        }
    }
}
