using System;

namespace UnityUtils.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoCreateAssetAttribute : System.Attribute
    {
        public string Path { get; set; }

        public AutoCreateAssetAttribute(string path)
        {
            Path = path;
        }
    }
}