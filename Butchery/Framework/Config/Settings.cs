using StardewValley;
using System.Collections.Generic;

namespace Butchery.Framework.Config
{
    class Settings
    {
        public bool IsEnabled = true;
        public int StrikePower = 99;
        public int MoodTilesDistance = 3;
        public float MoodPenalty = 0.5f;
        public int FarmingExperience = 10;
        public List<int> Professions = new List<int>() { Farmer.rancher };
        public float ProfessionMultiplier = 1.2f;
        public float BabyPenalty = 0.25f;
        public float StarvingPenalty = 0.5f;
    }
}
