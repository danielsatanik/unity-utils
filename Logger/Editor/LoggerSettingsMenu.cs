#if UNITY_EDITOR
using UnityEditor;
using UnityUtils.Editor.Utilities;

namespace UnityUtils.Debugging.Editor
{
    static class LoggerSettingsMenu
    {
        [MenuItem("Unity Utils/Logger/Open Settings", false, 1)]
        static void OpenLoggerSettings()
        {
            string path = "Assets/Unity Utils/Logger/Resources";
            ScriptableObjectUtility.ShowAsset<LoggerSettings>(path);
        }
    }
}
#endif