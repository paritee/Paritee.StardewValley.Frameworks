using StardewModdingAPI;

namespace BetterFarmAnimalVariety.Framework.Commands
{
    class List : Command
    {
        public List(IModHelper helper, IMonitor monitor) 
            : base("bfav_list", "List the farm animal categories and types.\nUsage: bfav_list", helper, monitor) { }

        /// <param name="command">The name of the command invoked.</param>
        /// <param name="args">The arguments received by the command. Each word after the command name is a separate argument.</param>
        public override void Callback(string command, string[] args)
        {
            string output = "Listing farm animals\n";

            // Load the cache
            Cache.FarmAnimals cache = Helpers.FarmAnimals.ReadCache();

            foreach (Framework.Cache.FarmAnimalCategory animal in Helpers.FarmAnimals.GetCategories())
            {
                output += this.DescribeFarmAnimalCategory(animal);
            }

            this.Monitor.Log(output, LogLevel.Info);
        }
    }
}
