using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace UnityUtils.Utilities.Extensions
{
    public static class MemberInfoExtension
    {
        public delegate void MemberInfoProvider(MemberInfo mem);

        public delegate void PropertyInfoProvider(PropertyInfo prop);

        public delegate void FieldInfoProvider(FieldInfo field);

        public static void Foreach(this MemberInfo[] mi, Action<MemberInfo> action)
        {
            foreach (var m in mi)
                action(m);
        }

        public static void Foreach(this IEnumerable<MemberInfo> mi, Action<MemberInfo> action)
        {
            foreach (var m in mi)
                action(m);
        }

        public static IEnumerable<MemberInfo> Foreach(
            this IEnumerable<MemberInfo> mi,
            PropertyInfoProvider propAction,
            FieldInfoProvider fieldAction,
            MemberInfoProvider action = null)
        {
            foreach (var m in mi)
            {
                if (m is PropertyInfo && propAction != null)
                    propAction(m as PropertyInfo);
                else if (m is FieldInfo && fieldAction != null)
                    fieldAction(m as FieldInfo);
                if (action != null)
                    action(m);
            }
            return mi;
        }

        public static T GetAttribute<T>(this MemberInfo mi) where T : Attribute
        {
            return mi.GetCustomAttributes(typeof(T), true).FirstOrDefault() as T;
        }

        public static void SetValue(this MemberInfo mi, object obj, object value)
        {
            if (mi is PropertyInfo)
                (mi as PropertyInfo).SetValue(obj, value, null);
            else if (mi is FieldInfo)
                (mi as FieldInfo).SetValue(obj, value);
        }

        public static object GetValue(this MemberInfo mi, object obj)
        {
            if (mi is PropertyInfo)
                return (mi as PropertyInfo).GetValue(obj, null);
            else if (mi is FieldInfo)
                return (mi as FieldInfo).GetValue(obj);
            return null;
        }

        public static Type GetMemberType(this MemberInfo mi)
        {
            if (mi is PropertyInfo)
                return (mi as PropertyInfo).PropertyType;
            else if (mi is FieldInfo)
                return (mi as FieldInfo).FieldType;
            return null;
        }

        public static MemberInfo[] Members<T>(this MemberInfo mi) where T : class
        {
            return mi.GetMemberType().GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}