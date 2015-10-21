#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityUtils.CustomTypes;

namespace UnityUtils.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(PercentSplitterAttribute))]
    public class PercentSplitterPropertyDrawer : PropertyDrawer
    {
        bool show = true;
        List<int> values;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attrib = attribute as PercentSplitterAttribute;

            var array = property.FindPropertyRelative("Percents");
            if (array.arraySize != attrib.Split)
            {
                if (array.arraySize < attrib.Split)
                    for (int i = array.arraySize; i < attrib.Split; ++i)
                    {
                        array.InsertArrayElementAtIndex(0);
                    }
                else
                    for (int i = attrib.Split; i < array.arraySize; ++i)
                    {
                        array.DeleteArrayElementAtIndex(i);
                    }
            }

            EditorGUILayout.BeginVertical();
            show = EditorGUILayout.Foldout(show, property.displayName);
            if (show)
            {
                var cValues = new List<int>();
                int sum = 0;
                for (int i = 0; i < array.arraySize; ++i)
                {
                    var el = array.GetArrayElementAtIndex(i);
                    var field = el.FindPropertyRelative("Object");
                    var value = el.FindPropertyRelative("Value");

                    EditorGUILayout.BeginHorizontal();

                    if (typeof(UnityEngine.Object).IsAssignableFrom(attrib.ObjectType))
                    {
                        field.objectReferenceValue = EditorGUILayout.ObjectField(
                            field.objectReferenceValue,
                            attrib.ObjectType,
                            false,
                            GUILayout.MaxWidth(200));
                    } else if (typeof(int) == attrib.ObjectType)
                    {
                        field.intValue = EditorGUILayout.IntField(
                            field.intValue,
                            GUILayout.MaxWidth(200)
                        );
                    } else if (typeof(float) == attrib.ObjectType)
                    {
                        field.floatValue = EditorGUILayout.FloatField(
                            field.floatValue,
                            GUILayout.MaxWidth(200)
                        );
                    } else if (typeof(long) == attrib.ObjectType)
                    {
                        field.longValue = EditorGUILayout.LongField(
                            field.longValue,
                            GUILayout.MaxWidth(200)
                        );
                    }
                    EditorGUILayout.LabelField(attrib.Min.ToString(), GUILayout.MaxWidth(30));

                    var temp = EditorGUIUtility.fieldWidth;
                    EditorGUIUtility.fieldWidth = 30;
                    cValues.Add(EditorGUILayout.IntSlider(value.intValue, attrib.Min, attrib.Max, GUILayout.MinWidth(40)));
                    EditorGUIUtility.fieldWidth = temp;

                    sum += cValues [i];

                    EditorGUILayout.LabelField(attrib.Max.ToString(), GUILayout.MaxWidth(30));

                    EditorGUILayout.EndHorizontal();
                }

                if (sum <= attrib.Max)
                {
                    for (int i = 0; i < array.arraySize; ++i)
                    {
                        var el = array.GetArrayElementAtIndex(i);
                        var value = el.FindPropertyRelative("Value");

                        value.intValue = cValues [i];
                    }
                } else if (values != null)
                {
                    int max = 0;
                    int maxI = 0;
                    for (int i = 0; i < array.arraySize; ++i)
                    {
                        if (Math.Abs(values [i] - cValues [i]) > max)
                        {
                            max = Math.Abs(values [i] - cValues [i]);
                            maxI = i;
                        }
                    }
                    array.GetArrayElementAtIndex(maxI)
                        .FindPropertyRelative("Value")
                        .intValue = values [maxI];
                }
                values = cValues;
            }
            EditorGUILayout.EndVertical();
        }
    }
}
#endif
