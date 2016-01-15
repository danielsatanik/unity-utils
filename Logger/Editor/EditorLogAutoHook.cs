#if UNITY_EDITOR
namespace UnityUtils.Debugging.Editor
{
    [UnityEditor.InitializeOnLoad]
    static class EditorLogAutoHook
    {
        static EditorLogAutoHook()
        {
            var defaultLoggerHandler = UnityEngine.Debug.logger.logHandler;

            UnityEngine.Debug.logger.logHandler = new EditorLogHandler(defaultLoggerHandler);
        }
    }
}
#endif