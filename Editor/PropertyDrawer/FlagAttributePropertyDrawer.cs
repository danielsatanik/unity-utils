#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;
using UnityUtils.Engine.Attributes;

namespace UnityUtils.Editor.PropertyDrawers
{
	[CustomPropertyDrawer (typeof(FlagAttribute))]
	public class EnumFlagDrawer : PropertyDrawer
	{
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			var attrib = attribute as FlagAttribute;
			Enum targetEnum = GetBaseProperty<Enum> (property);

			string propName = attrib.EnumName;
			if (string.IsNullOrEmpty (propName))
				propName = property.name;

			EditorGUI.BeginProperty (position, label, property);
			Enum enumNew = EditorGUI.EnumMaskField (position, propName, targetEnum);
			property.intValue = (int)Convert.ChangeType (enumNew, targetEnum.GetType ());
			EditorGUI.EndProperty ();
		}

		static T GetBaseProperty<T> (SerializedProperty prop)
		{
			// Separate the steps it takes to get to this property
			string[] separatedPaths = prop.propertyPath.Split ('.');

			// Go down to the root of this serialized property
			System.Object reflectionTarget = prop.serializedObject.targetObject;
			// Walk down the path to get the target object
			foreach (var path in separatedPaths) {
				FieldInfo fieldInfo = reflectionTarget.GetType ().GetField (path);
				reflectionTarget = fieldInfo.GetValue (reflectionTarget);
			}
			return (T)reflectionTarget;
		}
	}
}
#endif