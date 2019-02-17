using Paritee.StardewValleyAPI.FarmAnimals.Variations;
using Paritee.StardewValleyAPI.Players;
using Paritee.StardewValleyAPI.Players.Actions;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety
{
    public class ModApi
    {
        private readonly ModConfig Config;
        private readonly ISemanticVersion ModVersion;

        public ModApi(ModConfig config, ISemanticVersion modVersion)
        {
            this.Config = config;
            this.ModVersion = modVersion;
        }

        /// <returns>Returns bool</returns>
        public bool IsEnabled(string version)
        {
            if (!this.IsVersionSupported(version))
            {
                throw new NotSupportedException();
            }

            return this.Config.IsEnabled;
        }

        /// <param name="version">string</param>
        /// <returns>Returns Dictionary<string, string[]></returns>
        public Dictionary<string, string[]> GetFarmAnimalsByCategory(string version)
        {
            if (!this.IsVersionSupported(version))
            {
                throw new NotSupportedException();
            }

            return this.Config.FarmAnimals.ToDictionary(entry => entry.Key, entry => entry.Value.Types);
        }

        /// <param name="version">string</param>
        /// <param name="player">Paritee.StardewValleyAPI.Players</param>
        /// <returns>Returns Paritee.StardewValleyAPI.FarmAnimals.Variations.Blue</returns>
        public BlueVariation GetBlueFarmAnimals(string version, Player player)
        {
            if (!this.IsVersionSupported(version))
            {
                throw new NotSupportedException();
            }

            BlueConfig blueConfig = new BlueConfig(player.HasSeenEvent(BlueVariation.EVENT_ID));

            return new BlueVariation(blueConfig);
        }
        
        /// <param name="version">string</param>
        /// <param name="player">Paritee.StardewValleyAPI.Players</param>
        /// <param name="blueFarmAnimals">Paritee.StardewValleyAPI.FarmAnimals.Variations.BlueVariation</param>
        /// <returns>Returns Paritee.StardewValleyAPI.Players.Actions.BreedFarmAnimal</returns>
        public BreedFarmAnimal GetBreedFarmAnimal(string version, Player player, BlueVariation blueFarmAnimals)
        {
            if (!this.IsVersionSupported(version))
            {
                throw new NotSupportedException();
            }

            Dictionary<string, List<string>> farmAnimals = this.Config.GroupTypesByCategory();
            BreedFarmAnimalConfig breedFarmAnimalConfig = new BreedFarmAnimalConfig(farmAnimals, blueFarmAnimals, this.Config.RandomizeNewbornFromCategory, this.Config.RandomizeHatchlingFromCategory, this.Config.IgnoreParentProduceCheck);
            return new BreedFarmAnimal(player, breedFarmAnimalConfig);
        }

        private bool IsVersionSupported(string version)
        {
            // Must match the major version
            return version == this.ModVersion.MajorVersion.ToString();
        }
    }
}
