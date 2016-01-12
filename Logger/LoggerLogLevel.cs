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
}