#if UNITY_EDITOR
using UnityUtils.Editor.Utilities;


namespace UnityUtils.Debugging.Editor
{
    public class EditorLogHandler : UnityEngine.ILogHandler
    {
        LogHandler _handler;

        public EditorLogHandler(UnityEngine.ILogHandler defaultLogHandler)
        {
            _handler = new LogHandler(defaultLogHandler);
        }

        public void LogFormat(UnityEngine.LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            _handler.LogFormat(logType, context, format, args);

            var level = logType.Cast();

            #if DEBUG || PROFILE
            if (level == LoggerLogLevel.Assert || level == LoggerLogLevel.Exception)
            {
                DialogUtility.OpenStackJumpDialog(System.Enum.GetName(typeof(LoggerLogLevel), level) + "!", string.Format(format, args), 3);
            }
            #endif
        }

        public void LogException(System.Exception exception, UnityEngine.Object context)
        {
            _handler.LogException(exception, context);
        }
    }
}
#endif