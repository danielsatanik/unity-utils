using UnityEngine;
using UnityUtils.Utilities.Extensions;
using System.Linq;
using System;

namespace UnityUtils.Debugging
{
    static class LoggerAutoLoader
    {
        [RuntimeInitializeOnLoadMethod]
        static void OnLoad()
        {
            LoggerSettings settings = Resources.Load("LoggerSettings") as LoggerSettings;
            #if DEBUG
            if (settings == null)
            {
                UnityEngine.Debug.LogError("Logger Settings object not found. Pleas create it via Unity Utils/Create/Logger Settings and place it under any Resources-folder");
                UnityEngine.Debug.Break();
            }
            #endif
            if (settings.AutoLoad)
            {
                if (string.IsNullOrEmpty(Application.loadedLevelName) ||
                    Application.loadedLevelName.In(settings.SceneNames.Values))
                {
                    #pragma warning disable 0168
                    var tmp = Logger.Instance;
                    #pragma warning restore 0168
                }
            }
        }
    }
}