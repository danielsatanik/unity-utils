#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityUtils.CustomTypes;
using System.Collections.Generic;

namespace UnityUtils.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(SceneList))]
    public class SceneSelectorPropertyDrawer : PropertyDrawer
    {
        int value = 0;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var array = property.FindPropertyRelative("Values");

            if (array == null)
                return;

            if (array.isArray)
            {
                EditorGUI.BeginProperty(position, label, property);

                var scenes = EditorBuildSettings.scenes;

                List<string> names = new List<string>();
                for (var i = 0; i < scenes.Length; ++i)
                {
                    if (scenes [i].enabled)
                    {
                        var name = scenes [i].path.Substring(scenes [i].path.LastIndexOf("/") + 1);
                        name = name.Substring(0, name.Length - 6);
                        names.Add(name);
                    }
                }

                value = EditorGUI.MaskField(position, label, value, names.ToArray());

                array.ClearArray();
                for (int i = 0, c = 0; i < names.Count && value != 0; ++i)
                {
                    if ((value & (1 << i)) > 0)
                    {
                        array.InsertArrayElementAtIndex(c);
                        var el = array.GetArrayElementAtIndex(c);
                        el.stringValue = names [i];
                        c++;
                    }
                }

                EditorGUI.EndProperty();
            } else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}
#endif