using System.Linq;

namespace UnityUtils.Utilities.Extensions
{
    public static class TypeExtension
    {
        public static System.Collections.Generic.IEnumerable<System.Type> GetParentTypes(this System.Type type)
        {
            if (type.BaseType == null)
                return type.GetInterfaces();

            return Enumerable.Repeat(type.BaseType, 1)
                .Concat(type.GetInterfaces())
                .Concat(type.GetInterfaces().SelectMany<System.Type, System.Type>(GetParentTypes))
                .Concat(type.BaseType.GetParentTypes());
        }
    }
}