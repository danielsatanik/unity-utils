using System;
using System.Linq;

namespace UnityUtils.Utilities.Extensions
{
    public static class StringExtension
    {
        public static string ToDelimiterCase(this string str, string delimiter)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? delimiter + x.ToString() : x.ToString()).ToArray());
        }

        public static string ToTitleCase(this string str)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
        }

        public static string ToKebabCase(this string str)
        {
            return ToDelimiterCase(str, "-").ToLower();
        }

        public static string ToSnakeCase(this string str)
        {
            return ToDelimiterCase(str, "_");
        }

        public static bool IEquals(this string str1, string str2)
        {
            return string.Equals(str1, str2, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}