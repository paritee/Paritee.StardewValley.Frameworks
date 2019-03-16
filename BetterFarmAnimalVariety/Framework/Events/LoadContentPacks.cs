using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.IO;

namespace BetterFarmAnimalVariety.Framework.Events
{
    class LoadContentPacks
    {
        public static void SetUpContentPacks(IEnumerable<IContentPack> contentPacks, IMonitor monitor)
        {
            foreach (IContentPack contentPack in contentPacks)
            {
                monitor.Log($"Reading content pack: {contentPack.Manifest.Name} {contentPack.Manifest.Version}", LogLevel.Trace);

                try
                {
                    // Check if the content JSON is there
                    if (!File.Exists(Path.Combine(contentPack.DirectoryPath, Constants.ContentPack.ContentFileName)))
                    {
                        throw new FileNotFoundException($"{Constants.ContentPack.ContentFileName} not found.");
                    }

                    // Read the content
                    ContentPacks.Content content = contentPack.ReadJsonFile<ContentPacks.Content>(Constants.ContentPack.ContentFileName);

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
