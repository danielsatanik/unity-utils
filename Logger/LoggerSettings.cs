using UnityEngine;
using System;
using UnityUtils.CustomTypes;
using UnityUtils.Engine.Attributes;
using UnityUtils.Attributes;
using System.Linq;
using System.Collections.Generic;

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

        public Style Brackets = new Style();
        public Style Timestamp = new Style();
        public Style Text = new Style();

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

    [AutoCreateAsset("Assets/Unity Utils/Logger/Resources")]
    [Serializable]
    public sealed class LoggerSettings : ScriptableObject
    {
        public bool AutoLoad;
        public SceneList SceneNames;
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
        public uint RotateSize;
        public LoggerSettingsStyles Styles;
    }
}