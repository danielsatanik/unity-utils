using System.Diagnostics;
using System;

namespace UnityUtils.Debugging
{
    [Obsolete("Use UnityEngine.Debug.Log instead", true)]
    public static class Logger
    {
        #region public interface

        [Conditional("DEBUG"), Conditional("PROFILE")]
        public static void Trace(string message, params object[] list)
        {
            #if !FINAL
            UnityEngine.Debug.LogFormat(message, list);
            #endif
        }

        [Conditional("DEBUG"), Conditional("PROFILE")]
        public static void Info(string message, params object[] list)
        {
            #if !FINAL
            UnityEngine.Debug.LogFormat(message, list);
            #endif
        }

        [Conditional("DEBUG"), Conditional("PROFILE")]
        public static void Warn(string message, params object[] list)
        {
            #if !FINAL
            UnityEngine.Debug.LogWarningFormat(message, list);
            #endif
        }

        [Conditional("DEBUG"), Conditional("PROFILE")]
        public static void Error(string message, params object[] list)
        {
            #if !FINAL
            UnityEngine.Debug.LogErrorFormat(message, list);
            #endif
        }

        [Conditional("DEBUG"), Conditional("PROFILE")]
        public static void Assert(bool condition, string message, params object[] list)
        {
            #if !FINAL
            UnityEngine.Debug.AssertFormat(condition, message, list);
            #endif
        }

        #endregion // public interface
    }
}

