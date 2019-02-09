namespace BetterFarmAnimalVariety.Framework.Patches
{
    class PurchaseAnimalsMenuPatch
    {
        public static bool GetAnimalTitlePrefix(ref string name, ref string __result)
        {
            string[] parts = name.Split('_');

            __result = parts[0];

            if (parts.Length < 2)
            {
                return true;
            }

            return false;
        }

        public static bool GetAnimalDescriptionPrefix(ref string name, ref string __result)
        {
            string[] parts = name.Split('_');

            if (parts.Length < 2)
            {
                return true;
            }

            __result = parts[1];

            return false;
        }
    }
}
