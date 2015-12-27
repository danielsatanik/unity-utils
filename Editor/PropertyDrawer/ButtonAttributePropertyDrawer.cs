﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityUtils.Engine.Attributes;
using System.Reflection;
using UnityUtils.Debugging;

namespace UnityUtils.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(ButtonAttribute))]
    public class ButtonAttributePropertyDrawer : PropertyDrawer
    {
        MethodInfo _eventMethodInfo = null;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            
            var attrib = attribute as ButtonAttribute;

            var ownerType = property.serializedObject.targetObject.GetType();
            var ownerName = ownerType.FullName;

            var field = ownerType.GetField(property.name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var fieldName = field.Name;

            if (attrib.Name != null)
            {
                if (_eventMethodInfo == null)
                    _eventMethodInfo = ownerType.GetMethod(attrib.Name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                if (_eventMethodInfo != null)
                {
                    if (property.propertyType == SerializedPropertyType.Boolean)
                    {
                        if (Application.isPlaying ^ attrib.PlayModeOnly)
                            GUI.enabled = false;
                        EditorGUI.BeginProperty(position, label, property);
                        if (GUI.Button(position, attrib.ButtonName))
                        {
                            _eventMethodInfo.Invoke(property.serializedObject.targetObject, null);
                        }
                        EditorGUI.EndProperty();
                        GUI.enabled = true;
                        return;
                    }
                    else
                        Debug.LogWarningFormat("Field {0}.{1} has to be of type bool", ownerName, fieldName);
                }
                else
                    Debug.LogWarningFormat("The function {0} for the field {1}.{2} has to exist", attrib.Name, ownerName, fieldName);
            }
            else
                Debug.LogWarningFormat("Field's ({0}.{1}) ButtonAttribute has to have a not-null name", ownerName, fieldName);
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
#endif