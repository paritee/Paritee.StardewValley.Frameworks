using System;
using System.Reflection;

namespace BetterFarmAnimalVariety.Framework.Helpers
{
    class Reflection
    {
        public static PropertyInfo GetProperty(object obj, string property, BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            return obj is Type type
                ? type.GetProperty(property, bindingAttr)
                : obj.GetType().GetProperty(property, bindingAttr);
        }

        public static FieldInfo GetField(object obj, string field, BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            return obj is Type type
                ? type.GetField(field, bindingAttr)
                : obj.GetType().GetField(field, bindingAttr);
        }

        public static T GetFieldValue<T>(object obj, string field, BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            FieldInfo fieldInfo = Helpers.Reflection.GetField(obj, field, bindingAttr);
            obj = obj is Type type ? null : obj;

            return (T)fieldInfo.GetValue(obj);
        }

    }
}
