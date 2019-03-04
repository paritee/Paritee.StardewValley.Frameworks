using StardewModdingAPI;
using System;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.Loaders
{
    class FarmAnimalSprites : IAssetLoader
    {
        /// <summary>Get whether this instance can load the initial version of the given asset.</summary>
        /// <param name="asset">Basic metadata about the asset being loaded.</param>
        public bool CanLoad<T>(IAssetInfo asset)
        {
            // Check if trying to access the animal's sprite
            foreach(Cache.FarmAnimalType type in Helpers.FarmAnimals.GetCategories().SelectMany(o => o.Types))
            {
                // Check if trying to access the Animals/<type.Name>
                if (type.AdultSprite != null && asset.AssetNameEquals(PariteeCore.Api.FarmAnimal.BuildSpriteAssetName(type.Type, false, false)))
                {
                    return true;
                }

                // Check if trying to access the Animals/Baby<type.Name>
                if (type.BabySprite != null && asset.AssetNameEquals(PariteeCore.Api.FarmAnimal.BuildSpriteAssetName(type.Type, true, false)))
                {
                    return true;
                }

                // Check if trying to access the Animals/Sheared<type.Name>
                if (type.ShearedSprite != null && asset.AssetNameEquals(PariteeCore.Api.FarmAnimal.BuildSpriteAssetName(type.Type, false, true)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>Load a matched asset.</summary>
        /// <param name="asset">Basic metadata about the asset being loaded.</param>
        public T Load<T>(IAssetInfo asset)
        {
            // Check if trying to access the animal's sprite
            foreach (Cache.FarmAnimalType type in Helpers.FarmAnimals.GetCategories().SelectMany(o => o.Types))
            {
                // Check if trying to access the Animals/<type.Name>
                if (type.AdultSprite != null && asset.AssetNameEquals(PariteeCore.Api.FarmAnimal.BuildSpriteAssetName(type.Type, false, false)))
                {
                    // Check if the image exists; othewise use the default
                    return (T)(object)PariteeCore.Api.Mod.LoadTexture(type.AdultSprite);
                }

                // Check if trying to access the Animals/Baby<type.Name>
                if (type.BabySprite != null && asset.AssetNameEquals(PariteeCore.Api.FarmAnimal.BuildSpriteAssetName(type.Type, true, false)))
                {
                    return (T)(object)PariteeCore.Api.Mod.LoadTexture(type.BabySprite);
                }

                // Check if trying to access the Animals/Sheared<type.Name>
                if (type.ShearedSprite != null && asset.AssetNameEquals(PariteeCore.Api.FarmAnimal.BuildSpriteAssetName(type.Type, false, true)))
                {
                    return (T)(object)PariteeCore.Api.Mod.LoadTexture(type.ShearedSprite);
                }
            }

            throw new InvalidOperationException($"Unexpected asset '{asset.AssetName}'.");
        }
    }
}
