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

        bool mAddTimeStamp;
        bool mBreakOnAssert;
        bool mBreakOnError;
        bool mEchoToConsole;

        string mDirectoryRoot;
        string mCustomLogfilePrefix;
        uint mLogRotateSize;
        List<StreamWriter> mOutputstreams;

        LoggerErrorType mLogLevel;

        public static LoggerErrorType LogLevel
        {
            get { return Logger.Instance.mLogLevel; }
            set { Logger.Instance.mLogLevel = value; }
        }

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

            mLogRotateSize = Settings.RotateSize;
            mCustomLogfilePrefix = Settings.CustomPrefix;

            mAddTimeStamp = (Settings.Options & LoggerOption.ADD_TIME_STAMP) == LoggerOption.ADD_TIME_STAMP;
            mBreakOnAssert = (Settings.Options & LoggerOption.BREAK_ON_ASSERT) == LoggerOption.BREAK_ON_ASSERT;
            mBreakOnError = (Settings.Options & LoggerOption.BREAK_ON_ERROR) == LoggerOption.BREAK_ON_ERROR;
            mEchoToConsole = (Settings.Options & LoggerOption.ECHO_TO_CONSOLE) == LoggerOption.ECHO_TO_CONSOLE;

            mLogLevel = LoggerErrorType.Trace;

            mDirectoryRoot = GetDirectoryRoot();

            mOutputstreams = new List<StreamWriter>();
            foreach (var type in Enum.GetValues(typeof(LoggerErrorType)))
            {
                mOutputstreams.Add(File.AppendText(GetLogFileName((LoggerErrorType)type)));
            }
        }

        void OnDestroy()
        {
            #if !FINAL
            if (mOutputstreams != null)
            {
                mOutputstreams.ForEach(stream => stream.Close());
                mOutputstreams = null;
            }
            #endif
        }

        string GetLogFileName(LoggerErrorType type)
        {
            #if PROFILE
            const string prefix = "profile";
            #elif DEBUG
            const string prefix = "debug";
            #else
            const string prefix = "";
            #endif
            string typeName = Enum.GetName(typeof(LoggerErrorType), type);
            return String.Format("{0}{1}.{2}.{3}.log", mDirectoryRoot, prefix, mCustomLogfilePrefix, typeName.ToLower());
        }

        void LogRotate(LoggerErrorType type)
        {
            string fileName = GetLogFileName(type);
            var logFile = new FileInfo(fileName);
            if (!logFile.Exists)
            {
                UnityEngine.Debug.Assert(false, "Logfile {0} has to exist", fileName);
                UnityEngine.Debug.Break();
            }

            // 0 size means, do not rotate
            if (mLogRotateSize > 0 && logFile.Length >= 1024 * mLogRotateSize)
            {
                mOutputstreams[(int)type].Close();
                Compress(logFile);
                logFile.Delete();
                mOutputstreams[(int)type] = new StreamWriter(fileName, true);
            }
        }

        void Write(LoggerErrorType type, string message)
        {
            message = String.Format("[{0}] {1}", (int)type, message);
            if (mAddTimeStamp)
            {
                #if UNITY_EDITOR
                GetStackTraceString(ref message, 4);
                #endif
                DateTime now = DateTime.Now;
                message = string.Format("[{0:H:mm:ss.fff}] {1}", now, message);
            }

            // use all log files to write message in all "weaker" levels
            for (int i = 0; i < ((int)type) + 1; ++i)
            {
                LogRotate((LoggerErrorType)i);
                mOutputstreams[i].WriteLine(message);
                mOutputstreams[i].Flush();
            }

            if (mEchoToConsole)
            {
                if ((int)type >= (int)mLogLevel && (int)type <= (int)LoggerErrorType.Info) // Both Trace and Info go here
                    UnityEngine.Debug.Log(message);
                else if ((int)type >= (int)mLogLevel && type == LoggerErrorType.Warning)
                    UnityEngine.Debug.LogWarning(message);
                else if ((int)type >= (int)mLogLevel) // Both Error and Assert go here.
                    UnityEngine.Debug.LogError(message);
            }
        }

        static void WrapWrite(LoggerErrorType type, string message, bool onBreak = false)
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
            WrapWrite(LoggerErrorType.Trace, message);
            #endif
        }

        [Conditional("DEBUG"), Conditional("PROFILE")]
        public static void Info(string message, params object[] list)
        {
            #if !FINAL
            message = String.Format(message, list);
            WrapWrite(LoggerErrorType.Info, message);
            #endif
        }

        [Conditional("DEBUG"), Conditional("PROFILE")]
        public static void Warn(string message, params object[] list)
        {
            #if !FINAL
            message = String.Format(message, list);
            WrapWrite(LoggerErrorType.Warning, message);
            #endif
        }

        [Conditional("DEBUG"), Conditional("PROFILE")]
        public static void Error(string message, params object[] list)
        {
            #if !FINAL
            message = String.Format(message, list);
            WrapWrite(LoggerErrorType.Error, message, Logger.Instance.mBreakOnError);
            #endif
        }

        [Conditional("DEBUG"), Conditional("PROFILE")]
        public static void Assert(bool condition, string message, params object[] list)
        {
            #if !FINAL
            if (condition)
                return;

            message = String.Format(message, list);
            WrapWrite(LoggerErrorType.Assert, message);

            #if UNITY_EDITOR
            var myTrace = new StackTrace(true);
            StackFrame myFrame = myTrace.GetFrame(1);
            GetStackTraceString(ref message, 2);

            if (Logger.Instance.mBreakOnAssert)
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

