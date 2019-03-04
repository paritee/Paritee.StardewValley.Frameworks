using StardewModdingAPI;
using StardewModdingAPI.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BetterFarmAnimalVariety.Framework.Events
{
    class LoadContentPacks
    {
        public static void SetUpContentPacks(IEnumerable<IContentPack> contentPacks, IMonitor monitor)
        {
            foreach (IContentPack contentPack in contentPacks)
            {
                monitor.Log($"Reading content pack: {contentPack.Manifest.Name} {contentPack.Manifest.Version} from {contentPack.DirectoryPath}", LogLevel.Trace);

                try
                {
                    // Check if the content JSON is there
                    if (!File.Exists(Path.Combine(contentPack.DirectoryPath, Constants.Mod.ContentPackContentFileName)))
                    {
                        throw new FileNotFoundException($"{Constants.Mod.ContentPackContentFileName} not found.");
                    }

                    // Read the content
                    ContentPacks.FarmAnimals content = contentPack.ReadJsonFile<ContentPacks.FarmAnimals>(Constants.Mod.ContentPackContentFileName);

                    content.SetUp(contentPack);
                }
                catch (Exception exception)
                {
                    monitor.Log($"{contentPack.Manifest.Name} will not load: " + exception.Message, LogLevel.Warn);
                }
            }
        }
    }
}
