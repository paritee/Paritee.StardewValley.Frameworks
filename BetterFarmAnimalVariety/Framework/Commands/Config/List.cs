using Newtonsoft.Json;
using StardewModdingAPI;

namespace BetterFarmAnimalVariety.Framework.Commands.Config
{
    class List : Command
    {
        public List(IModHelper helper, IMonitor monitor, ModConfig config) 
            : base("bfav_conf_list", "List the config.json settings.\nUsage: bfav_conf_list", helper, monitor, config) { }

        /// <param name="command">The name of the command invoked.</param>
        /// <param name="args">The arguments received by the command. Each word after the command name is a separate argument.</param>
        public override void Callback(string command, string[] args)
        {
            this.Monitor.Log(JsonConvert.SerializeObject(this.Config), LogLevel.Info);
        }
    }
}
