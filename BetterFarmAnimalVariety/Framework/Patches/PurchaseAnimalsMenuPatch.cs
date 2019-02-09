namespace BetterFarmAnimalVariety.Framework.Patches
{
    class PurchaseAnimalsMenuPatch : Patch
    {
        protected static bool TryParse(string str, out string[] parts)
        {
            parts = str.Split('_');

            return parts.Length >= 2;
        }
    }
}
