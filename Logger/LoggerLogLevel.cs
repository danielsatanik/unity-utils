namespace UnityUtils.Debugging
{
    [System.Flags]
    public enum LoggerLogLevel
    {
        Trace = 1,
        Info = 2,
        Warn = 4,
        Error = 8,
        Assert = 16
    }
}