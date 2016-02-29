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

        public static string ToPascalCase(this string str, char delimiter)
        {
            var parts = str.Split(delimiter);
            return string.Join(string.Empty, parts.Select(s => s.ToTitleCase()).ToArray());
        }

        public static string ToKebabCase(this string str)
        {
            return ToDelimiterCase(str, "-").ToLower();
        }

        public static string ToSnakeCase(this string str)
        {
            return ToDelimiterCase(str, "_");
        }

        public static string ToPlural(this string singular)
        {
            // Multiple words in the form A of B : Apply the plural to the first word only (A)
            int index = singular.LastIndexOf(" of ");
            if (index > 0)
                return (singular.Substring(0, index)) + singular.Remove(0, index).ToPlural();

            // single Word rules
            //sibilant ending rule
            if (singular.EndsWith("sh"))
                return singular + "es";
            if (singular.EndsWith("ch"))
                return singular + "es";
            if (singular.EndsWith("us"))
                return singular + "es";
            if (singular.EndsWith("ss"))
                return singular + "es";
            //-ies rule
            if (singular.EndsWith("y"))
                return singular.Remove(singular.Length - 1, 1) + "ies";
            // -oes rule
            if (singular.EndsWith("o"))
                return singular.Remove(singular.Length - 1, 1) + "oes";
            // -s suffix rule
            return singular + "s";
        }

        public static bool IEquals(this string str1, string str2)
        {
            return string.Equals(str1, str2, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}