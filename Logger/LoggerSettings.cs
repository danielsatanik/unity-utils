using UnityEngine;
using System;
using UnityUtils.CustomTypes;
using UnityUtils.Engine.Attributes;
using UnityUtils.Attributes;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;

namespace UnityUtils.Debugging
{
    [Serializable]
    public class LoggerSettingsStyles
    {
        [Serializable]
        public class Style
        {
            public Color Color = Color.white;
            public bool Bold;
        }

        Style[] _logLevel =
            Enum.GetValues(typeof(LoggerLogLevel))
                .Cast<LoggerLogLevel>()
                .Select(lvl => new Style())
                .ToArray();

        Style[] _logType =
            Enum.GetValues(typeof(LoggerLogType))
                .Cast<LoggerLogType>()
                .Select(lvl => new Style())
                .ToArray();

        public Style Brackets = new Style();
        public Style Timestamp = new Style();
        public Style Type = new Style();
        public Style Text = new Style();
        public Style ListKey = new Style();
        public Style ListValue = new Style();

        public Dictionary<LoggerLogLevel, Style> LogLevel
        {
            get
            {
                var dict = new Dictionary<LoggerLogLevel, Style>();
                for (var i = 0; i < _logLevel.Length; ++i)
                    dict.Add((LoggerLogLevel)(Enum.GetValues(typeof(LoggerLogLevel)).GetValue(i)), _logLevel[i]);
                return dict;
            }
        }

        public Dictionary<LoggerLogType, Style> LogType
        {
            get
            {
                var dict = new Dictionary<LoggerLogType, Style>();
                for (var i = 0; i < _logType.Length; ++i)
                    dict.Add((LoggerLogType)(Enum.GetValues(typeof(LoggerLogType)).GetValue(i)), _logType[i]);
                return dict;
            }
        }
    }

    [Serializable]
    [InitializeOnLoad]
    public sealed class LoggerSettings : ScriptableObject
    {
        //        public bool AutoLoad;
        //        public SceneList SceneNames;
        public string CustomPrefix;
        [Flag]
        public LoggerOption Options = 
            LoggerOption.ADD_TIME_STAMP |
            LoggerOption.BREAK_ON_ASSERT |
            LoggerOption.BREAK_ON_ERROR |
            LoggerOption.ECHO_TO_CONSOLE;
        [Flag]
        public LoggerLogLevel LogLevel =
            LoggerLogLevel.Trace |
            LoggerLogLevel.Info |
            LoggerLogLevel.Warn |
            LoggerLogLevel.Error |
            LoggerLogLevel.Assert;
        [Flag]
        public LoggerLogType LogType = 
            LoggerLogType.Info |
            LoggerLogType.Warn |
            LoggerLogType.Error |
            LoggerLogType.Assert |
            LoggerLogType.Exception;
        public uint RotateSize;

        public LoggerSettingsStyles Styles { get; private set; }

        public LoggerSettings()
        {
            Styles = new LoggerSettingsStyles();
            ResetStyle();
        }

        public void ResetStyle()
        {
            Color c;
            ColorUtility.TryParseHtmlString("#7F8C8DFF", out c);
            Styles.Brackets.Color = c;
            Styles.Brackets.Bold = false;
            Styles.Timestamp.Color = c;
            Styles.Timestamp.Bold = false;

            ColorUtility.TryParseHtmlString("#27AE60FF", out c);
            Styles.Type.Color = c;
            Styles.Type.Bold = true;

            ColorUtility.TryParseHtmlString("#ECF0F1FF", out c);
            Styles.Text.Color = c;
            Styles.Text.Bold = false;

            ColorUtility.TryParseHtmlString("#BDC3C7FF", out c);
            Styles.ListKey.Color = c;
            Styles.ListKey.Bold = false;

            ColorUtility.TryParseHtmlString("#BDC3C7FF", out c);
            Styles.ListValue.Color = c;
            Styles.ListValue.Bold = false;

            ColorUtility.TryParseHtmlString("#BDC3C7FF", out c);
            Styles.LogLevel[LoggerLogLevel.Trace].Color = c;
            Styles.LogLevel[LoggerLogLevel.Trace].Bold = true;

            ColorUtility.TryParseHtmlString("#3498DBFF", out c);
            Styles.LogLevel[LoggerLogLevel.Info].Color = c;
            Styles.LogLevel[LoggerLogLevel.Info].Bold = true;

            ColorUtility.TryParseHtmlString("#F1C40FFF", out c);
            Styles.LogLevel[LoggerLogLevel.Warn].Color = c;
            Styles.LogLevel[LoggerLogLevel.Warn].Bold = true;

            ColorUtility.TryParseHtmlString("#C0392BFF", out c);
            Styles.LogLevel[LoggerLogLevel.Error].Color = c;
            Styles.LogLevel[LoggerLogLevel.Error].Bold = true;

            ColorUtility.TryParseHtmlString("#FF5342FF", out c);
            Styles.LogLevel[LoggerLogLevel.Assert].Color = c;
            Styles.LogLevel[LoggerLogLevel.Assert].Bold = true;


            ColorUtility.TryParseHtmlString("#BDC3C7FF", out c);
            Styles.LogType[LoggerLogType.Exception].Color = c;
            Styles.LogType[LoggerLogType.Exception].Bold = true;

            ColorUtility.TryParseHtmlString("#3498DBFF", out c);
            Styles.LogType[LoggerLogType.Info].Color = c;
            Styles.LogType[LoggerLogType.Info].Bold = true;

            ColorUtility.TryParseHtmlString("#F1C40FFF", out c);
            Styles.LogType[LoggerLogType.Warn].Color = c;
            Styles.LogType[LoggerLogType.Warn].Bold = true;

            ColorUtility.TryParseHtmlString("#C0392BFF", out c);
            Styles.LogType[LoggerLogType.Error].Color = c;
            Styles.LogType[LoggerLogType.Error].Bold = true;

            ColorUtility.TryParseHtmlString("#FF5342FF", out c);
            Styles.LogType[LoggerLogType.Assert].Color = c;
            Styles.LogType[LoggerLogType.Assert].Bold = true;
        }
    }
}