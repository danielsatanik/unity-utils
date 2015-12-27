using System.Reflection;
using System.Runtime.CompilerServices;
using System.Linq;

namespace UnityUtils.Utilities.Extensions
{
    public static class PropertyInfoExtension
    {
        public static bool IsAutoImplemented(this PropertyInfo prop)
        {
            bool mightBe = prop.GetGetMethod()
                .GetCustomAttributes(
                               typeof(CompilerGeneratedAttribute),
                               true
                           )
                .Any();
            if (!mightBe)
            {
                return false;
            }


            bool maybe = prop.DeclaringType
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.Name.Contains(prop.Name))
                .Where(f => f.Name.Contains("BackingField"))
                .Any(
                             f => f.GetCustomAttributes(
                                 typeof(CompilerGeneratedAttribute),
                                 true
                             ).Any()
                         );

            return maybe;
        }

        public static bool IsFullProperty(this PropertyInfo prop)
        {
            return prop.CanRead && prop.CanWrite;
        }

        public static bool IsAbstract(this PropertyInfo prop)
        {
            return prop.PropertyType.IsAbstract;
        }

        public static bool IsNotifyingPrimitive(this PropertyInfo prop)
        {
            var propType = prop.PropertyType;
            return
                !prop.IsAutoImplemented() &&
            (
                propType.IsPrimitive ||
                propType == typeof(decimal) ||
                propType == typeof(string)
            );
        }
    }
}