using StardewModdingAPI;

namespace BetterFarmAnimalVariety
{
    public class ModConfig
    {
        public string Format;
        public bool IsEnabled;

        public ModConfig()
        {
            this.Format = null;
            this.IsEnabled = true;
        }

        public void Write(IModHelper helper)
        {
            helper.WriteConfig(this);
        }

        public void AssertValidFormat(string targetFormat)
        {
            Framework.Helpers.Assert.VersionIsSupported(this.Format, targetFormat);
        }
    }
}