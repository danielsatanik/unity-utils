using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;

using UnityUtils.Utilities.Extensions;

namespace UnityUtils.Debugging
{
    public class LogHandler : UnityEngine.ILogHandler
    {
        readonly UnityEngine.ILogHandler _defaultLogHandler;

        LoggerSettings Settings { get; set; }

        bool AddTimeStamp { get { return (Settings.Options & LoggerOption.ADD_TIME_STAMP) == LoggerOption.ADD_TIME_STAMP; } }

        bool BreakOnAssert { get { return (Settings.Options & LoggerOption.BREAK_ON_ASSERT) == LoggerOption.BREAK_ON_ASSERT; } }

        bool BreakOnError { get { return (Settings.Options & LoggerOption.BREAK_ON_ERROR) == LoggerOption.BREAK_ON_ERROR; } }

        bool EchoToConsole { get { return (Settings.Options & LoggerOption.ECHO_TO_CONSOLE) == LoggerOption.ECHO_TO_CONSOLE; } }

        string _directoryRoot;

        string CustomLogfilePrefix { get { return Settings.CustomPrefix; } }

        uint LogRotateSize { get { return Settings.RotateSize; } }

        LoggerLogType LogLevel { get { return Settings.LogType; } }

        LoggerSettingsStyles Styles { get { return Settings.Styles; } }

        public LogHandler(UnityEngine.ILogHandler defaultLoggerHandler)
        {
            _defaultLogHandler = defaultLoggerHandler;
            Settings = UnityEngine.Resources.Load<LoggerSettings>("LoggerSettings");

            _directoryRoot = GetDirectoryRoot();

            foreach (var type in System.Enum.GetValues(typeof(LoggerLogType)))
            {
                File.AppendText(GetLogFileName((LoggerLogType)type)).Close();
            }
        }

        public void LogFormat(UnityEngine.LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            var level = Convert(logType);

            var message = string.Format(format, args);

            var origMessage = string.Copy(message);

            var cmessage = string.Copy(message);
            cmessage = Color(cmessage, Styles.Text);

            var type = new StackTrace().GetFrame(3).GetMethod().DeclaringType;
            if (type.DeclaringType != null)
                type = type.DeclaringType;

            message = ": " + message;
            cmessage = Color(type.Name + ": ", Styles.Type) + cmessage;

            var logName = level.ToString().ToUpper();
            message = " [" + logName + "] " + message;
            cmessage = Color(" [" + logName + "] ", Styles.LogType[level]) + cmessage;

            #if UNITY_EDITOR
            GetStackTraceString(ref message, 4, Settings);
            GetColoredStackTraceString(ref cmessage, 4, Settings);
            #endif
            if (AddTimeStamp)
            {
                var now = System.DateTime.Now;
                message =
                    "[" + string.Format("{0:H:mm:ss.fff}", now) + "]" + message;
                cmessage =
                    Color("[", Styles.Brackets) +
                Color(string.Format("{0:H:mm:ss.fff}", now), Styles.Timestamp) +
                Color("]", Styles.Brackets) + cmessage;
            }

            LogRotate(level);
            var file = File.AppendText(GetLogFileName(level));
            file.WriteLine(message);
            file.Close();

            if (EchoToConsole && (LogLevel & level) > 0)
            {
                _defaultLogHandler.LogFormat(logType, context, cmessage);
            }

            #if !FINAL && UNITY_EDITOR
            if (level == LoggerLogType.Assert)
            {
                if (BreakOnAssert)
                    UnityEngine.Debug.Break();

                if (UnityEditor.EditorUtility.DisplayDialog("Assert!", origMessage, "Show", "Cancel"))
                {
                    var myTrace = new StackTrace(true);
                    StackFrame myFrame = myTrace.GetFrame(1);
                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(myFrame.GetFileName(), myFrame.GetFileLineNumber());
                }
            }
            #endif
        }

        public void LogException(System.Exception exception, UnityEngine.Object context)
        {
//            LogFormat(UnityEngine.LogType.Exception, context, exception.ToString());
            _defaultLogHandler.LogException(exception, context);
        }

        #region helper

        static LoggerLogType Convert(UnityEngine.LogType logType)
        {
            switch (logType)
            {
                case UnityEngine.LogType.Log:
                    return LoggerLogType.Info;
                case UnityEngine.LogType.Warning:
                    return LoggerLogType.Warn;
                case UnityEngine.LogType.Error:
                    return LoggerLogType.Error;
                case UnityEngine.LogType.Assert:
                    return LoggerLogType.Assert;
            }
            return LoggerLogType.Exception;
        }

        string GetLogFileName(LoggerLogType type)
        {
            #if PROFILE
            const string prefix = "profile";
            #elif DEBUG
            const string prefix = "debug";
            #else
            const string prefix = "";
            #endif
            string typeName = System.Enum.GetName(typeof(LoggerLogType), type);
            var customPrefix = CustomLogfilePrefix;
            if (!string.IsNullOrEmpty(customPrefix))
                customPrefix += ".";
            return string.Format("{0}{1}.{2}{3}.log", _directoryRoot, prefix, customPrefix, typeName.ToLower());
        }

        void LogRotate(LoggerLogType type)
        {
            string fileName = GetLogFileName(type);
            var logFile = new FileInfo(fileName);
            if (!logFile.Exists)
            {
                UnityEngine.Debug.AssertFormat(false, "Logfile {0} has to exist", fileName);
                UnityEngine.Debug.Break();
            }

            // 0 size means, do not rotate
            if (LogRotateSize > 0 && logFile.Length >= 1024 * LogRotateSize)
            {
                Compress(logFile);
                logFile.Delete();
            }
        }

        static string Color(string text, LoggerSettingsStyles.Style style)
        {
            if (style.Bold)
                text = string.Format("<b>{0}</b>", text);

            return string.Format("<color=#{0}>{1}</color>", UnityEngine.ColorUtility.ToHtmlStringRGBA(style.Color), text);
        }

        static void GetStackTraceString(ref string origMessage, int depth, LoggerSettings settings)
        {
            #if UNITY_EDITOR
            var myTrace = new StackTrace(true);
            StackFrame myFrame = myTrace.GetFrame(depth);
            string messageInformation = string.Format(
                                            "{0}{4}\n{1}{5}\n{2}{6}\n{3}{7}",
                                            "Filename: ",
                                            "Namespace: ",
                                            "Method: ",
                                            "Line: ",
                                            myFrame.GetFileName().Replace(UnityEngine.Application.dataPath + "/", ""),
                                            myFrame.GetMethod().DeclaringType.Namespace,
                                            myFrame.GetMethod().DeclaringType.Name + "." + myFrame.GetMethod().ToString().Split(' ')[1],
                                            myFrame.GetFileLineNumber());
            origMessage = origMessage + "\n" + messageInformation;
            #endif
        }

        static void GetColoredStackTraceString(ref string origMessage, int depth, LoggerSettings settings)
        {
            #if UNITY_EDITOR
            var myTrace = new StackTrace(true);
            StackFrame myFrame = myTrace.GetFrame(depth);
            string messageInformation = string.Format(
                                            "{0}{4}\n{1}{5}\n{2}{6}\n{3}{7}",
                                            Color("Filename: ", settings.Styles.ListKey),
                                            Color("Namespace: ", settings.Styles.ListKey),
                                            Color("Method: ", settings.Styles.ListKey),
                                            Color("Line: ", settings.Styles.ListKey),
                                            Color(myFrame.GetFileName().Replace(UnityEngine.Application.dataPath + "/", ""), settings.Styles.ListValue),
                                            Color(myFrame.GetMethod().DeclaringType.Namespace, settings.Styles.ListValue),
                                            Color(myFrame.GetMethod().DeclaringType.Name + "." + myFrame.GetMethod().ToString().Split(' ')[1], settings.Styles.ListValue),
                                            Color(myFrame.GetFileLineNumber().ToString(), settings.Styles.ListValue));
            origMessage = origMessage + "\n" + messageInformation;
            #endif
        }

        static string GetDirectoryRoot()
        {
            string path;
            #if UNITY_EDITOR
            string dataPath = UnityEngine.Application.dataPath;
            path = dataPath.Remove(dataPath.Length - 7) + "/Logs/";
            Directory.CreateDirectory(path);
            #elif UNITY_ANDROID
            path = UnityEngine.Application.persistentDataPath;
            #elif UNITY_IOS
            path = UnityEngine.Application.persistentDataPath + "/Logs/";
            Directory.CreateDirectory(path);
            #else
            path = UnityEngine.Application.dataPath +"/";
            #endif

            return path;
        }

        static void Compress(FileInfo fi)
        {
            // Get the stream of the source file.
            using (FileStream inFile = fi.OpenRead())
            {
                // Prevent compressing hidden and 
                // already compressed files.
                if ((File.GetAttributes(fi.FullName)
                    & FileAttributes.Hidden)
                    != FileAttributes.Hidden & fi.Extension != ".gz")
                {
                    string destFileName = fi.FullName.Substring(0, fi.FullName.LastIndexOf('.')) +
                                          System.DateTime.Now.ToString("yyyyMMddHHmmfff") +
                                          fi.FullName.Substring(fi.FullName.LastIndexOf('.')) +
                                          ".gz";
                    // Create the compressed file.
                    using (FileStream outFile =
                               File.Create(destFileName))
                    {
                        using (var compress =
                                   new GZipStream(outFile,
                                       CompressionMode.Compress))
                        {
                            // Copy the source file into 
                            // the compression stream.
                            inFile.CopyTo(compress);
                        }
                    }
                }
            }
        }

        #endregion // helper
    }
}