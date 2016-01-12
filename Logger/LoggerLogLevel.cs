using UnityEngine;

namespace UnityUtils.Debugging
{
    [System.Flags]
    public enum LoggerLogLevel
    {
        Info = 1,
        Warn = 2,
        Error = 4,
        Assert = 8,
        Exception = 16
    }

    public static class LoggerLogLevelExtension
    {
        public static LoggerLogLevel Cast(this LogType level)
        {
            switch (level)
            {
                case LogType.Log:
                    return LoggerLogLevel.Info;
                case LogType.Warning:
                    return LoggerLogLevel.Warn;
                case LogType.Error:
                    return LoggerLogLevel.Error;
                case LogType.Assert:
                    return LoggerLogLevel.Assert;
            }
            return LoggerLogLevel.Exception;
        }
    }
}