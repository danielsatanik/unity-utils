using UnityEngine;

using System;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Collections.Generic;
using UnityUtils.Utilities.Extensions;
using UnityUtils.Utilities.Behaviours;

namespace UnityUtils.Debugging
{
    using Tags = Dictionary<string, byte>;

    public sealed class Logger : SingletonBehaviour<Logger>
    {
        LoggerSettings Settings;

        bool AddTimeStamp { get { return (Settings.Options & LoggerOption.ADD_TIME_STAMP) == LoggerOption.ADD_TIME_STAMP; } }

        bool BreakOnAssert { get { return (Settings.Options & LoggerOption.BREAK_ON_ASSERT) == LoggerOption.BREAK_ON_ASSERT; } }

        bool BreakOnError { get { return (Settings.Options & LoggerOption.BREAK_ON_ERROR) == LoggerOption.BREAK_ON_ERROR; } }

        bool EchoToConsole { get { return (Settings.Options & LoggerOption.ECHO_TO_CONSOLE) == LoggerOption.ECHO_TO_CONSOLE; } }

        string _directoryRoot;

        string CustomLogfilePrefix { get { return Settings.CustomPrefix; } }

        uint LogRotateSize { get { return Settings.RotateSize; } }

        Dictionary<LoggerLogLevel, StreamWriter> _outputstreams;

        LoggerLogLevel LogLevel { get { return Settings.LogLevel; } }

        LoggerSettingsStyles Styles { get { return Settings.Styles; } }

        #region private interface

        protected override void Initialize()
        {
            Settings = Resources.Load<LoggerSettings>("LoggerSettings");
            #if DEBUG
            if (Settings == null)
            {
                UnityEngine.Debug.LogError("Logger Settings object not found. Pleas create it via Unity Utils/Create/Logger Settings");
                UnityEngine.Debug.Break();
            }
            #endif

            _directoryRoot = GetDirectoryRoot();

            _outputstreams = new Dictionary<LoggerLogLevel, StreamWriter>();
            foreach (var type in Enum.GetValues(typeof(LoggerLogLevel)))
            {
                _outputstreams.Add((LoggerLogLevel)type, File.AppendText(GetLogFileName((LoggerLogLevel)type)));
            }
        }

        void OnDestroy()
        {
            #if !FINAL
            if (_outputstreams != null)
            {
                foreach (var stream in _outputstreams.Values)
                    stream.Close();
                _outputstreams = null;
            }
            #endif
        }

        string Color(string text, LoggerSettingsStyles.Style style)
        {
            if (style.Bold)
                text = string.Format("<b>{0}</b>", text);

            return string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(style.Color), text);
        }

        string GetLogFileName(LoggerLogLevel type)
        {
            #if PROFILE
            const string prefix = "profile";
            #elif DEBUG
            const string prefix = "debug";
            #else
            const string prefix = "";
            #endif
            string typeName = Enum.GetName(typeof(LoggerLogLevel), type);
            return String.Format("{0}{1}.{2}.{3}.log", _directoryRoot, prefix, CustomLogfilePrefix, typeName.ToLower());
        }

        void LogRotate(LoggerLogLevel type)
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
                _outputstreams[type].Close();
                Compress(logFile);
                logFile.Delete();
                _outputstreams[type] = new StreamWriter(fileName, true);
            }
        }

        void Write(LoggerLogLevel level, string message)
        {
            message = Color(message, Styles.Text);

            var logName = level.ToString().ToUpper();
            message = Color(" [" + logName + "] ", Styles.LogLevel[level]) + message;
            
            if (AddTimeStamp)
            {
                #if UNITY_EDITOR
                GetStackTraceString(ref message, 4);
                #endif
                DateTime now = DateTime.Now;
                message =
                    Color("[", Styles.Brackets) +
                Color(string.Format("{0:H:mm:ss.fff}", now), Styles.Timestamp) +
                Color("]", Styles.Brackets) + message;
            }

            LogRotate(level);
            _outputstreams[level].WriteLine(message);
            _outputstreams[level].Flush();

            if (EchoToConsole)
            {
                if ((LogLevel & level) == LoggerLogLevel.Trace || (LogLevel & level) == LoggerLogLevel.Info) // Both Trace and Info go here
                    UnityEngine.Debug.Log(message);
                else if ((LogLevel & level) == LoggerLogLevel.Warn)
                    UnityEngine.Debug.LogWarning(message);
                else if ((LogLevel & level) == LoggerLogLevel.Error || (LogLevel & level) == LoggerLogLevel.Assert) // Both Error and Assert go here.
                    UnityEngine.Debug.LogError(message);
            }
        }

        static void WrapWrite(LoggerLogLevel type, string message, bool onBreak = false)
        {
            if (Logger.Instance != null)
            {
                Logger.Instance.Write(type, message);
                if (onBreak)
                    UnityEngine.Debug.Break();
            }
            else
            {
                UnityEngine.Debug.Log(message);
            }
        }

        #endregion // private interface

        #region public interface

        [Conditional("DEBUG"), Conditional("PROFILE")]
        public static void Trace(string message, params object[] list)
        {
            #if !FINAL
            message = String.Format(message, list);
            WrapWrite(LoggerLogLevel.Trace, message);
            #endif
        }

        [Conditional("DEBUG"), Conditional("PROFILE")]
        public static void Info(string message, params object[] list)
        {
            #if !FINAL
            message = String.Format(message, list);
            WrapWrite(LoggerLogLevel.Info, message);
            #endif
        }

        [Conditional("DEBUG"), Conditional("PROFILE")]
        public static void Warn(string message, params object[] list)
        {
            #if !FINAL
            message = String.Format(message, list);
            WrapWrite(LoggerLogLevel.Warn, message);
            #endif
        }

        [Conditional("DEBUG"), Conditional("PROFILE")]
        public static void Error(string message, params object[] list)
        {
            #if !FINAL
            message = String.Format(message, list);
            WrapWrite(LoggerLogLevel.Error, message, Logger.Instance.BreakOnError);
            #endif
        }

        [Conditional("DEBUG"), Conditional("PROFILE")]
        public static void Assert(bool condition, string message, params object[] list)
        {
            #if !FINAL
            if (condition)
                return;

            message = String.Format(message, list);
            WrapWrite(LoggerLogLevel.Assert, message);

            #if UNITY_EDITOR
            var myTrace = new StackTrace(true);
            StackFrame myFrame = myTrace.GetFrame(1);
            GetStackTraceString(ref message, 2);

            if (Logger.Instance.BreakOnAssert)
                UnityEngine.Debug.Break();

            if (UnityEditor.EditorUtility.DisplayDialog("Assert!", message, "Show", "Cancel"))
            {
                UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(myFrame.GetFileName(), myFrame.GetFileLineNumber());
            }
            #endif

            #endif
        }

        #endregion // public interface

        #region helper

        static void GetStackTraceString(ref string origMessage, int depth)
        {
            #if UNITY_EDITOR
            var myTrace = new StackTrace(true);
            StackFrame myFrame = myTrace.GetFrame(depth);
            string messageInformation = String.Format(
                                            "Filename: {0}\nNamespace: {1}\nMethod: {2}.{3}\nLine: {4}",
                                            myFrame.GetFileName(),
                                            myFrame.GetMethod().DeclaringType.Namespace,
                                            myFrame.GetMethod().DeclaringType.Name,
                                            myFrame.GetMethod().ToString().Split(' ')[1],
                                            myFrame.GetFileLineNumber());
            origMessage = origMessage + "\n" + messageInformation;
            #endif
        }

        static string GetDirectoryRoot()
        {
            string path;
            #if UNITY_EDITOR
            string dataPath = Application.dataPath;
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
                    string destFileName = fi.FullName.Substring(0, fi.FullName.LastIndexOf('.')) + DateTime.Now.ToString("yyyyMMddHHmmfff") + fi.FullName.Substring(fi.FullName.LastIndexOf('.')) + ".gz";
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

