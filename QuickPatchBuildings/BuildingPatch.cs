using Newtonsoft.Json;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickPatchBuildings
{
    class BuildingPatch
    {
        public string Format;
        public string Type;
        public string Data;
        public string Asset;
        public bool Seasonal;

        public string GetSeasonalAsset(string season)
        {
            List<string> parts = this.Asset.Split('/').ToList();

            parts[parts.Count - 1] = $"{parts[parts.Count - 1]}_{season}";

            return String.Join("/", parts).ToLower();
        }
    }
}