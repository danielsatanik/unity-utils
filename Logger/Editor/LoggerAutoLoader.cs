#if UNITY_EDITOR
using UnityEditor;
using UnityUtils.Editor.Utilities;

namespace UnityUtils.Debugging
{
    [InitializeOnLoad]
    static class LoggerAutoLoader
    {
        static LoggerAutoLoader()
        {
            ScriptableObjectUtility.CreateAssetSafe<LoggerSettings>("Assets/Unity Utils/Logger/Resources");
        }
    }
}
#endif