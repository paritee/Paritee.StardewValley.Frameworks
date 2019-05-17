using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StardewModdingAPI;

namespace BetterFarmAnimalVariety.Models
{
    public class ConfigFarmAnimalData
    {
        [JsonProperty(Order = 1)]
        public string Name { get; set; }

        [JsonProperty(Order = 2)]
        public string AnimalData { get; set; }

        [JsonProperty(Order = 3)]
        public string AdultImage { get; set; }

        [JsonProperty(Order = 4)]
        public string BabyImage{ get; set; }

        public ConfigFarmAnimalData()
        {

        }

        public void InjectData(IContentPack pack, IModHelper helper)
        {
            helper.Content.AssetLoaders.Add(new FarmAnimalAssetLoader("Animals/"+Name, pack.LoadAsset<Texture2D>(AdultImage)));
            helper.Content.AssetLoaders.Add(new FarmAnimalAssetLoader("Animals/Baby" + Name, pack.LoadAsset<Texture2D>(BabyImage)));
            helper.Content.AssetEditors.Add(new FarmAnimalAssetEditor(Name, AnimalData));
        }


    }
}
