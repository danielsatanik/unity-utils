using UnityEngine;
using System;

namespace UnityUtils.Manager
{
    [Serializable]
    public class PluginInformation : ScriptableObject
    {
        public string Title;
        public string Version;
        public string BottomBox;
    }
}