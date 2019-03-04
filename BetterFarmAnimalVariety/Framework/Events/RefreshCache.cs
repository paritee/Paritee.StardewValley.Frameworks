using StardewModdingAPI.Events;
using System.Collections.Generic;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Events
{
    class RefreshCache
    {
        public static void OnGameLaunched(GameLaunchedEventArgs e)
        {
            // Seed a new cache with the vanilla animals; content packs loaded 
            // later will modify these animals
            RefreshCache.SeedCacheWithVanillaFarmAnimals();
        }

        private static void SeedCacheWithVanillaFarmAnimals()
        {
            // Seed with all of the vanilla farm animals
            List<Cache.FarmAnimalCategory> categories = PariteeCore.Constants.VanillaFarmAnimalCategory.All()
                .Select(o => new Cache.FarmAnimalCategory(PariteeCore.Constants.Mod.Path, o))
                .ToList();

            // Reset the cache
            Cache.FarmAnimals cache = new Cache.FarmAnimals(categories);

            // Commit the seed
            Helpers.FarmAnimals.Write(cache);
        }
    }
}
