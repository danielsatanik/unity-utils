#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityUtils.Engine.Attributes;

namespace UnityUtils.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(TagSelectorAttribute))]
    public class TagSelectorPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attrib = this.attribute as TagSelectorAttribute;
            if (attrib.ReadOnly && Application.isPlaying)
                GUI.enabled = false;
            if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.BeginProperty(position, label, property);

                if (attrib.AllowUntagged)
                {
                    property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
                }
                else
                {
                    var tags = (from s in UnityEditorInternal.InternalEditorUtility.tags
                                               where s != "Untagged"
                                               select new GUIContent(s)).ToArray();
                    var stag = property.stringValue;
                    int index = -1;
                    for (int i = 0; i < tags.Length; i++)
                    {
                        if (tags[i].text == stag)
                        {
                            index = i;
                            break;
                        }
                    }
                    index = EditorGUI.Popup(position, label, index, tags);
                    if (index >= 0)
                    {
                        property.stringValue = tags[index].text;
                    }
                    else
                    {
                        property.stringValue = null;
                    }
                }

                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
            GUI.enabled = true;
        }
    }
}
#endif