using UnityEngine;
using UnityUtils.Utilities.Extensions;

#if UNITY_5_3
using UnityEngine.SceneManagement;
#endif

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
                Debug.LogError("Logger Settings object not found. Pleas create it via Unity Utils/Create/Logger Settings and place it under any Resources-folder");
                Debug.Break();
            }
            #endif
            if (settings.AutoLoad)
            {
                #if UNITY_5_3
                var scene = SceneManager.GetActiveScene();
                if (string.IsNullOrEmpty(scene.name) ||
                    scene.name.In(settings.SceneNames.Values))
                #else
                if (string.IsNullOrEmpty(Application.loadedLevelName) ||
                    Application.loadedLevelName.In(settings.SceneNames.Values))
                #endif
                {
                    #pragma warning disable 0168
                    var tmp = Logger.Instance;
                    #pragma warning restore 0168
                }
            }
        }
    }
}