﻿using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using UnityUtils.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityUtils.Engine.Utilities;

namespace UnityUtils.Debugging
{
    public class LogHandler : UnityEngine.ILogHandler
    {
        class ExceptionContext : UnityEngine.Object
        {
            public System.Exception Exception { get; set; }
        }

        readonly UnityEngine.ILogHandler _defaultLogHandler;
        string _customLogfilePrefix;
        Dictionary<LoggerLogLevel, string> _logFilePaths;

        LoggerSettings Settings { get; set; }

        bool AddTimeStamp { get { return (Settings.Options & LoggerOption.ADD_TIME_STAMP) == LoggerOption.ADD_TIME_STAMP; } }

        bool BreakOnAssert { get { return (Settings.Options & LoggerOption.BREAK_ON_ASSERT) == LoggerOption.BREAK_ON_ASSERT; } }

        bool BreakOnError { get { return (Settings.Options & LoggerOption.BREAK_ON_ERROR) == LoggerOption.BREAK_ON_ERROR; } }

        bool EchoToConsole { get { return (Settings.Options & LoggerOption.ECHO_TO_CONSOLE) == LoggerOption.ECHO_TO_CONSOLE; } }

        string CustomLogfilePrefix { get { return Settings.CustomPrefix; } }

        uint LogRotateSize { get { return Settings.RotateSize; } }

        LoggerLogLevel LogLevel { get { return Settings.LogType; } }

        LoggerSettingsStyles Styles { get { return Settings.Styles; } }

        Dictionary<LoggerLogLevel, string> LogFilePaths
        {
            get
            {
                if (_customLogfilePrefix == null ||
                    _customLogfilePrefix == "" && !string.IsNullOrEmpty(CustomLogfilePrefix) ||
                    _customLogfilePrefix != CustomLogfilePrefix)
                {
                    _customLogfilePrefix = CustomLogfilePrefix ?? "";
                    if (!string.IsNullOrEmpty(_customLogfilePrefix))
                        _customLogfilePrefix += ".";

                    _logFilePaths = new Dictionary<LoggerLogLevel, string>();
                    foreach (var level in System.Enum.GetValues(typeof(LoggerLogLevel)).Cast<LoggerLogLevel>())
                        _logFilePaths.Add(level, PathUtility.GetPath(PathUtility.ProjectRootPath, "/Logs/") + _customLogfilePrefix + level.ToString().ToLower() + ".log");
                }
                return _logFilePaths;
            }
        }

        public LogHandler(UnityEngine.ILogHandler defaultLoggerHandler)
        {
            _defaultLogHandler = defaultLoggerHandler;
            Settings = UnityEngine.Resources.Load<LoggerSettings>("LoggerSettings");

            foreach (var type in System.Enum.GetValues(typeof(LoggerLogLevel)))
            {
                File.AppendText(LogFilePaths[(LoggerLogLevel)type]).Close();
            }
        }

        public void LogFormat(UnityEngine.LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            var level = logType.Cast();

            string message = string.Format(format, args);
            string cmessage;

            PrepareMessageText(ref message, out cmessage);
            PrefixTimeStamp(ref message, ref cmessage);
            PostfixDeclaringType(ref message, ref cmessage);
            PostfixLogLevelInfo(ref message, ref cmessage, level);
            PostfixStackTrace(ref message, ref cmessage, level, context);

            using (var file = File.AppendText(LogFilePaths[level]))
                file.WriteLine(message);
            
            LogRotate(level);

            if (EchoToConsole && (LogLevel & level) > 0)
            {
                cmessage = cmessage.Replace("{", "{{").Replace("}", "}}");
                if (logType == UnityEngine.LogType.Exception)
                    logType = UnityEngine.LogType.Error;
                _defaultLogHandler.LogFormat(logType, context, @cmessage);
            }

            #if DEBUG || PROFILE
            if (level == LoggerLogLevel.Assert || level == LoggerLogLevel.Exception)
            {
                if (BreakOnAssert)
                    UnityEngine.Debug.Break();
            }
            else if (level == LoggerLogLevel.Error && BreakOnError)
            {
                UnityEngine.Debug.Break();
            }
            #endif
        }

        public virtual void LogException(System.Exception exception, UnityEngine.Object context)
        {
            try
            {
                LogFormat(UnityEngine.LogType.Exception, new ExceptionContext { Exception = exception }, exception.Message);
            }
            catch (System.Exception e)
            {
                _defaultLogHandler.LogException(e, context);
            }
        }

        #region helper

        void PrepareMessageText(ref string message, out string cmessage)
        {
            cmessage = string.Copy(message) + Styles.Text;
        }

        void PrefixTimeStamp(ref string message, ref string cmessage)
        {
            if (AddTimeStamp)
            {
                var now = string.Format("{0:H:mm:ss.fff}", System.DateTime.Now);
                message = "[" + now + "]" + message;
                cmessage =
                    ("[" + Styles.Brackets) +
                (now + Styles.Timestamp) +
                ("]" + Styles.Brackets) + cmessage;
            }
        }

        void PostfixDeclaringType(ref string message, ref string cmessage)
        {
            var type = new StackTrace().GetFrame(4).GetMethod().DeclaringType;
            if (type.DeclaringType != null)
                type = type.DeclaringType;

            message = type.Name + ": " + message;
            cmessage = (type.Name + ": " + Styles.Type) + cmessage;
        }

        void PostfixLogLevelInfo(ref string message, ref string cmessage, LoggerLogLevel level)
        {
            var logName = level.ToString().ToUpper();
            message = " [" + logName + "] " + message;
            cmessage = (" [" + logName + "] " + Styles.LogLevel[level]) + cmessage;
        }

        void PostfixStackTrace(ref string message, ref string cmessage, LoggerLogLevel level, UnityEngine.Object context)
        {
            if (level != LoggerLogLevel.Exception)
            {
                var myTrace = new StackTrace(true);
                StackFrame myFrame = myTrace.GetFrame(5);
                var filename = myFrame.GetFileName().Replace(UnityEngine.Application.dataPath + "/", "");
                var ns = myFrame.GetMethod().DeclaringType.Namespace;
                var methodname = myFrame.GetMethod().DeclaringType.Name + "." + myFrame.GetMethod().ToString().Split(' ')[1];
                var linenumber = myFrame.GetFileLineNumber();

                message += string.Format("\n{0}{1}\n{2}{3}\n{4}{5}\n{6}{7}",
                    "Filename: ", filename,
                    "Namespace: ", ns,
                    "Method: ", methodname,
                    "Line: ", linenumber);

                var keyStyle = Styles.ListKey;
                var valStyle = Styles.ListValue;

                cmessage += string.Format("\n{0}{1}\n{2}{3}\n{4}{5}\n{6}{7}",
                    "Filename: " + keyStyle, filename + valStyle,
                    "Namespace: " + keyStyle, ns + valStyle,
                    "Method: " + keyStyle, methodname + valStyle,
                    "Line: " + keyStyle, (linenumber + "") + valStyle);
            }
            else
            {
                message += "\n" + ((ExceptionContext)context).Exception.StackTrace;
                cmessage += "\n" + ((ExceptionContext)context).Exception.StackTrace;
            }
        }

        void LogRotate(LoggerLogLevel type)
        {
            string fileName = LogFilePaths[type];
            var logFile = new FileInfo(fileName);
            UnityEngine.Debug.AssertFormat(logFile.Exists, "Logfile {0} has to exist", fileName);

            // 0 size means, do not rotate
            if (LogRotateSize > 0 && logFile.Length >= 1024 * LogRotateSize)
            {
                logFile.Archive();
            }
        }

        #endregion // helper
    }
}