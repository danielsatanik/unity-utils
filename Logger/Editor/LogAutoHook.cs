#if UNITY_EDITOR
using UnityUtils.Editor.Utilities;

namespace UnityUtils.Debugging.Editor
{
    [UnityEditor.InitializeOnLoad]
    static class LogAutoHook
    {
        static LogAutoHook()
        {
            ScriptableObjectUtility.CreateAssetSafe<LoggerSettings>("Assets/Unity Utils/Logger/Resources");
            var defaultLoggerHandler = UnityEngine.Debug.logger.logHandler;
            UnityEngine.Debug.logger.logHandler = new LogHandler(defaultLoggerHandler);
        }
    }
}
#endif