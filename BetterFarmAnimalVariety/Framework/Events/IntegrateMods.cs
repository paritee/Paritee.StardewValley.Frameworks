using BetterFarmAnimalVariety.Framework.Integrations;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BetterFarmAnimalVariety.Framework.Events
{
    class IntegrateMods
    {
        public static void SetUpModIntegrations(IModHelper helper, IMonitor monitor)
        {
            // Supported integrations:
            // <integrator, integrator arguments>
            Dictionary<Type, object[]> integrations = new Dictionary<Type, object[]>()
            {
                { typeof(MoreAnimals), new object[] { monitor } }
            };

            // Get the "GetApi" method through reflection
            var getApi = helper.ModRegistry.GetType().GetMethods()
                .Where(x => x.Name == "GetApi")
                .First(x => x.ContainsGenericParameters);

            foreach (KeyValuePair<Type, object[]> integration in integrations)
            {
                // Get the API interface
                Type apiInterface = (Type)integration.Key.GetProperty("ApiInterface", BindingFlags.Public | BindingFlags.Static).GetValue(null, null);

                // Make it generic
                MethodInfo genericGetApi = getApi.MakeGenericMethod(apiInterface);

                // Get the mod's key
                string key = (string)integration.Key.GetProperty("Key", BindingFlags.Public | BindingFlags.Static).GetValue(null, null);

                // Get the API via interface
                var api = genericGetApi.Invoke(helper.ModRegistry, new object[] { key });

                if (api == null)
                {
                    monitor.Log($"{key} API not found", LogLevel.Trace);
                    continue;
                }

                // Check if the API exists
                if (!(api == null))
                {
                    // Create the integration class
                    var instance = Activator.CreateInstance(integration.Key, api);

                    // Get the set up method of the integration
                    MethodInfo method = integration.Key.GetMethod("SetUp");

                    // Set up the integration
                    method.Invoke(instance, integration.Value);
                }
            }
        }
    }
}
