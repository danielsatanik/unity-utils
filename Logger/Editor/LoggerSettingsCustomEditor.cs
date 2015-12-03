#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;

namespace UnityUtils.Debugging.Editor
{
    [CustomEditor(typeof(LoggerSettings))]
    public class LoggerSettingsEditor : UnityEditor.Editor
    {
        LoggerSettings _settings;

        Vector2 _logPosition;

        readonly GUILayoutOption _maxWidth = GUILayout.MaxWidth(9999);
        readonly GUILayoutOption _toggleWidth = GUILayout.MinWidth(EditorGUIUtility.labelWidth - 40);

        void OnEnable()
        {
            _settings = target as LoggerSettings;
        }

        string C(string text, LoggerSettingsStyles.Style style)
        {
            if (style.Bold)
                text = string.Format("<b>{0}</b>", text);

            return string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(style.Color), text);
        }

        public void PlaceStyle(LoggerSettingsStyles.Style style, string label = null)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            if (label == null)
                label = typeof(LoggerSettingsStyles)
                .GetFields()
                .Single(x =>
                    {
                        var value = x.GetValue(_settings.Styles) as LoggerSettingsStyles.Style;
                        return value != null && value == style;
                    }).Name;
            style.Bold = EditorGUILayout.ToggleLeft("   " + label, style.Bold, _toggleWidth);
            style.Color = EditorGUILayout.ColorField(style.Color, _maxWidth);
            EditorGUILayout.EndHorizontal();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("LOGGER SETTINGS", UnityUtils.Engine.UI.EditorStyles.Title, GUILayout.Height(40));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("AutoLoad"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("SceneNames"));

            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("CustomPrefix"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Options"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("LogLevel"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("RotateSize"));
            EditorGUILayout.HelpBox("The rotation size is in KB. 0 KB means, never rotate.", MessageType.None);

            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            #if UNITY_UTILS_DEVELOPMENT
            GUI.changed = false;
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Bold", UnityUtils.Engine.UI.EditorStyles.HeaderLeft, GUILayout.MinWidth(EditorGUIUtility.labelWidth));
            EditorGUILayout.LabelField("Colors", UnityUtils.Engine.UI.EditorStyles.HeaderLeft, _maxWidth);
            EditorGUILayout.EndHorizontal();

            PlaceStyle(_settings.Styles.Brackets);
            PlaceStyle(_settings.Styles.Timestamp);
            PlaceStyle(_settings.Styles.Text);

            EditorGUILayout.Separator();

            foreach (var pair in _settings.Styles.LogLevel)
                PlaceStyle(pair.Value, pair.Key.ToString());

            EditorGUILayout.Separator();

            var style = new GUIStyle();
            style.richText = true;

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            _logPosition = EditorGUILayout.BeginScrollView(_logPosition, UnityUtils.Engine.UI.EditorStyles.VeryDarkBackground, GUILayout.ExpandHeight(false));
            var i = 0;
            foreach (var pair in _settings.Styles.LogLevel)
            {
                if (i % 2 == 0)
                    EditorGUILayout.BeginHorizontal(UnityUtils.Engine.UI.EditorStyles.DarkBackground);
                else
                    EditorGUILayout.BeginHorizontal(UnityUtils.Engine.UI.EditorStyles.LightBackground);
                EditorGUILayout.LabelField(
                    C("[", _settings.Styles.Brackets) +
                    C("12:29:08.028", _settings.Styles.Timestamp) +
                    C("]", _settings.Styles.Brackets) +
                    C(" [" + pair.Key.ToString().ToUpper() + "] ", pair.Value) +
                    C("This is a log message!\nFilename: miracle_file.cs\n", _settings.Styles.Text),
                    style, GUILayout.Height(30));

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Log"))
            {
                Logger.Trace("This is a trace log");
                Logger.Info("This is a info log");
                Logger.Warn("This is a warn log");
                Logger.Error("This is a error log");
            }

            if (GUILayout.Button("Reset Style"))
            {
                _settings.ResetStyle();
            }
            EditorGUILayout.EndHorizontal();

            if (GUI.changed)
                EditorUtility.SetDirty(target);
            #endif
        }
    }
}
#endif