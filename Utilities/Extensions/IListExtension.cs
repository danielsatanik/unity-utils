using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace UnityUtils.Utilities.Extensions
{
    public static class IListExtensions
    {
        public static void AddMultiple<T>(this IList<T> list, Func<T> f, int nr)
        {
            for (int i = 0; i < nr; ++i)
                list.Add(f());
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                var box = new byte[1];
                do
                    provider.GetBytes(box);
                while (box[0] >= n * (Byte.MaxValue / n));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static string ToStringContent<T>(this IList<T> list)
        {
            string res = "";

            if (list.Count > 0)
            {
                res = list[0].ToString();
            
                for (int i = 1; i < list.Count; ++i)
                    res = string.Join(",\n", new [] { res, list[i].ToString() });
            }

            return string.Format("{{\n{0}\n}}", res);
        }
    }

}