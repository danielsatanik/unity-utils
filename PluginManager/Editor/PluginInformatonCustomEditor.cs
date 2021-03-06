﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UnityUtils.Manager.Editor
{
    [CustomEditor(typeof(PluginInformation))]
    public class PluginInformationEditor : UnityEditor.Editor
    {
        #pragma warning disable 414 // object never used
        SerializedObject mObject;
        SerializedProperty mVersion;
        SerializedProperty mKeepCorrectDirectory;

        PluginInformation mInfo;

        void OnEnable()
        {
            mObject = serializedObject;
            mVersion = serializedObject.FindProperty("Version");
            mKeepCorrectDirectory = serializedObject.FindProperty("KeepCorrectDirectory");

            mInfo = target as PluginInformation;
        }

        public override void OnInspectorGUI()
        {
            #if UNITY_UTILS_DEVELOPMENT
            GUI.changed = false;
            #endif

            EditorGUILayout.BeginHorizontal();
            #if UNITY_UTILS_DEVELOPMENT
            mInfo.Title = EditorGUILayout.TextArea(mInfo.Title, UnityUtils.Engine.UI.EditorStyles.Title, GUILayout.Height(80));
            #else
            EditorGUILayout.LabelField(mInfo.Title, UnityUtils.Engine.UI.EditorStyles.Title, GUILayout.Height(80));
            #endif
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();


            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(mKeepCorrectDirectory);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Version", GUILayout.Width(50));
            #if UNITY_UTILS_DEVELOPMENT
            mInfo.Version = EditorGUILayout.TextField(mInfo.Version, GUILayout.ExpandWidth(true));
            #else
            EditorGUILayout.LabelField(mInfo.Version, GUILayout.ExpandWidth(true));
            #endif
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical();

            #if UNITY_UTILS_DEVELOPMENT
            mInfo.BottomBox = EditorGUILayout.TextArea(mInfo.BottomBox, UnityUtils.Engine.UI.EditorStyles.BottomBox, GUILayout.MaxHeight(9999), GUILayout.ExpandHeight(true));
            #else
            EditorGUILayout.SelectableLabel(mInfo.BottomBox, UnityUtils.Engine.UI.EditorStyles.BottomBox, GUILayout.MaxHeight(9999), GUILayout.ExpandHeight(true));
            #endif
            EditorGUILayout.EndVertical();

            #if UNITY_UTILS_DEVELOPMENT
            if (GUI.changed)
                EditorUtility.SetDirty(mInfo);
            #endif
        }
        #pragma warning restore 414
    }
}
#endif