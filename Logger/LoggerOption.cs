namespace UnityUtils.Debugging
{
  [System.Flags]
  public enum LoggerOption
  {
    ADD_TIME_STAMP = 1 << 0,
    BREAK_ON_ASSERT = 1 << 1,
    BREAK_ON_ERROR = 1 << 2,
    ECHO_TO_CONSOLE = 1 << 3
  }
}