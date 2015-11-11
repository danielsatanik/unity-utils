#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UnityUtils.Manager.Editor
{
    [CustomEditor(typeof(PluginInformation))]
    public class PluginInformationEditor : UnityEditor.Editor
    {
        #pragma warning disable 414
        SerializedObject mObject;
        SerializedProperty mVersion;

        PluginInformation mInfo;

        void OnEnable()
        {
            mObject = serializedObject;
            mVersion = serializedObject.FindProperty("Version");

            mInfo = target as PluginInformation;
        }

        public override void OnInspectorGUI()
        {
            #if UNITY_UTILS_DEVELOPMENT
            EditorGUI.BeginChangeCheck();
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
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
            #endif
        }
        #pragma warning restore 414
    }
}
#endif