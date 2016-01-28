namespace UnityUtils.Utilities.Extensions
{
    public static class ArrayExtension
    {
        public static string ToStringContent<T>(this T[] array)
        {
            string content = "";
            if (array.Length > 0)
            {
                content = array[0].ToString();
                for (int i = 1; i < array.Length; ++i)
                {
                    content = string.Join(",\n", new []{ content, array[i].ToString() });
                }
            }
            return string.Format("[\n{0}\n]", content);
        }
    }
}