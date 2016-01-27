#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityUtils.Attributes;
using System.Reflection;

namespace UnityUtils.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(OnChangeAttribute))]
    public class OnChangeAttributePropertyDrawer : PropertyDrawer
    {
        object _val;
        MethodInfo _eventMethodInfo;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attrib = attribute as OnChangeAttribute;

            var ownerType = property.serializedObject.targetObject.GetType();
            var ownerName = ownerType.FullName;

            var fieldName = fieldInfo.Name;

            if (attrib.FunctionName != null)
            {
                if (_eventMethodInfo == null)
                    _eventMethodInfo = ownerType.GetMethod(attrib.FunctionName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                if (_eventMethodInfo != null)
                {
                    if (Application.isPlaying ^ attrib.PlayModeOnly)
                        GUI.enabled = false;
                    EditorGUI.PropertyField(position, property, label);
                    var temp = fieldInfo.GetValue(property.serializedObject.targetObject);
                    if (_val != temp)
                    {
                        _eventMethodInfo.Invoke(property.serializedObject.targetObject, null);
                        _val = temp;
                    }
                    GUI.enabled = true;
                    return;
                }
                else
                    Debug.LogWarningFormat("The function {0} for the field {1}.{2} has to exist", attrib.FunctionName, ownerName, fieldName);
            }
            else
                Debug.LogWarningFormat("Field's ({0}.{1}) ButtonAttribute has to have a not-null name", ownerName, fieldName);
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
#endif