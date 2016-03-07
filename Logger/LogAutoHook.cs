namespace UnityUtils.Debugging.Editor
{
    static class LogAutoHook
    {
        [UnityEngine.RuntimeInitializeOnLoadMethod]
        static void OnRuntimeMethodLoad()
        {
            #if !UNITY_EDITOR
            var defaultLoggerHandler = UnityEngine.Debug.logger.logHandler;

            UnityEngine.Debug.logger.logHandler = new LogHandler(defaultLoggerHandler);

            System.Console.SetOut(new LogWriter());
            System.Console.SetError(new LogWriter());
            #endif
        }
    }
}
