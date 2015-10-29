﻿using System.Linq;
using System;

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
    }
}