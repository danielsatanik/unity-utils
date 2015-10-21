#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UnityUtils.Debugging.Editor
{
    [CustomEditor(typeof(LoggerSettings))]
    public class LoggerSettingsEditor : UnityEditor.Editor
    {
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
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ErrorType"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("RotateSize"));
            EditorGUILayout.HelpBox("The rotation size is in KB. 0 KB means, never rotate.", MessageType.None);

            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif