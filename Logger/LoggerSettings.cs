using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityUtils.Engine.Attributes;

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

            public static string operator+(string text, Style style)
            {
                if (style.Bold)
                    text = string.Format("<b>{0}</b>", text);

                return string.Format("<color=#{0}>{1}</color>", UnityEngine.ColorUtility.ToHtmlStringRGBA(style.Color), text);
            }

            public static string operator+(Style style, string text)
            {
                return text + style;
            }
        }

        Style[] _logLevel =
            Enum.GetValues(typeof(LoggerLogLevel))
                .Cast<LoggerLogLevel>()
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
    }

    [Serializable]
    public sealed class LoggerSettings : ScriptableObject
    {
        public string CustomPrefix;
        [Flag]
        public LoggerOption Options = 
            LoggerOption.ADD_TIME_STAMP |
            LoggerOption.BREAK_ON_ASSERT |
            LoggerOption.BREAK_ON_ERROR |
            LoggerOption.ECHO_TO_CONSOLE;
        [Flag]
        public LoggerLogLevel LogType = 
            LoggerLogLevel.Info |
            LoggerLogLevel.Warn |
            LoggerLogLevel.Error |
            LoggerLogLevel.Assert |
            LoggerLogLevel.Exception;
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
            Styles.LogLevel[LoggerLogLevel.Exception].Color = c;
            Styles.LogLevel[LoggerLogLevel.Exception].Bold = true;

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
        }
    }
}