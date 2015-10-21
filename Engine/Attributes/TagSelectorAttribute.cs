using UnityEngine;

namespace UnityUtils.Engine.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public sealed class TagSelectorAttribute : PropertyAttribute
    {
        public bool ReadOnly;
        public bool AllowUntagged;

        public TagSelectorAttribute(bool allowUntagged = false, bool readOnly = false)
        {
            AllowUntagged = allowUntagged;
            ReadOnly = readOnly;
        }
    }
}