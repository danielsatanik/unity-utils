using UnityEngine;

namespace UnityUtils.Engine.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class ButtonAttribute : PropertyAttribute
    {
        public bool PlayModeOnly { get; set; }

        public string Name { get; set; }

        public string ButtonName { get; set; }

        public ButtonAttribute(bool playModeOnly, string name, string buttonName = null)
        {
            PlayModeOnly = playModeOnly;
            Name = name;
            if (buttonName == null)
                buttonName = name;
            ButtonName = buttonName;
        }
    }
}