﻿#if UNITY_EDITOR
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

            EditorGUILayout.PropertyField(serializedObject.FindProperty("CustomPrefix"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Options"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("LogType"));
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
            PlaceStyle(_settings.Styles.Type);
            PlaceStyle(_settings.Styles.Text);
            PlaceStyle(_settings.Styles.ListKey);
            PlaceStyle(_settings.Styles.ListValue);

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
                if (EditorGUIUtility.isProSkin)
                {
                    if (i == 1)
                        EditorGUILayout.BeginHorizontal(UnityUtils.Engine.UI.EditorStyles.DarkSelectedBackground);
                    else if (i % 2 == 0)
                        EditorGUILayout.BeginHorizontal(UnityUtils.Engine.UI.EditorStyles.DarkBackground);
                    else
                        EditorGUILayout.BeginHorizontal(UnityUtils.Engine.UI.EditorStyles.VeryDarkBackground);
                }
                else
                {
                    if (i == 1)
                        EditorGUILayout.BeginHorizontal(UnityUtils.Engine.UI.EditorStyles.LightSelectedBackground);
                    else if (i % 2 == 0)
                        EditorGUILayout.BeginHorizontal(UnityUtils.Engine.UI.EditorStyles.LightBackground);
                    else
                        EditorGUILayout.BeginHorizontal(UnityUtils.Engine.UI.EditorStyles.VeryLightBackground);
                }
                i++;
                EditorGUILayout.LabelField(
                    C("[", _settings.Styles.Brackets) +
                    C("16-02-18 12:29:08.028", _settings.Styles.Timestamp) +
                    C("]", _settings.Styles.Brackets) +
                    C(" [" + pair.Key.ToString().ToUpper() + "] ", pair.Value) +
                    C("ExampleClass: ", _settings.Styles.Type) +
                    C("This is a log message!\n", _settings.Styles.Text) +
                    C("Filename: ", _settings.Styles.ListKey) +
                    C("miracle_file.cs\n", _settings.Styles.ListValue),
                    style, GUILayout.Height(30));

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Log"))
            {
                Debug.Log("This is an info log");
                Debug.LogFormat("this is a parametrized info {{{0}}}", "log");
                Debug.LogWarning("This is a warn log");
                Debug.LogError("This is an error log");
                Debug.Assert(false, "This is an assert log");
                throw new Exception("This is an exception");
            }
            GUI.enabled = true;

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