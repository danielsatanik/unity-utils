using System.Linq;
using System.Reflection;

namespace UnityUtils.Utilities.Extensions
{
    public static class Extension
    {
        public static bool In<T>(this T obj, params T[] args)
        {
            return args.Contains(obj);
        }

        public static bool NotIn<T>(this T obj, params T[] args)
        {
            return !obj.In(args);
        }

        public static bool Exists<T>(this T obj) where T : class
        {
            return obj != null;
        }

        public static MemberInfo[] Members<T>(this T obj) where T : class
        {
            return typeof(T).GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static object Call<T>(this T obj, System.Type type, string name, System.Type[] parameterTypes, object[] parameters)
        {
            var method = type.GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameterTypes, null);
            return method.Invoke(obj, parameters);
        }

        public static object CallGeneric<T>(this T obj, string name, System.Type methodType, System.Type[] parameterTypes, object[] parameters) where T : class
        {
            var method = typeof(T).GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameterTypes, null);
            var generic = method.MakeGenericMethod(methodType);
            return generic.Invoke(obj, parameters);
        }

        public static object CallStaticGeneric<T>(this T obj, string name, System.Type methodType, System.Type[] parameterTypes, object[] parameters) where T : class
        {
            var method = typeof(T).GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, parameterTypes, null);
            var generic = method.MakeGenericMethod(methodType);
            return generic.Invoke(null, parameters);
        }

        public static object CallStaticExtension<T>(this T obj, System.Type extendingType, string name, System.Type type, System.Type[] parameterTypes, object[] parameters) where T : class
        {
            var method = extendingType.GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, parameterTypes, null);
            var generic = method.MakeGenericMethod(type);
            return generic.Invoke(obj, parameters);
        }
    }
}