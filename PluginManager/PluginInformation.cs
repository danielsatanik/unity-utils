using UnityEngine;
using System;
using UnityUtils.Attributes;

namespace UnityUtils.Manager
{
    [AutoCreateAsset("Assets/Unity Utils/PluginManager/Resources")]
    [Serializable]
    public class PluginInformation : ScriptableObject
    {
        public string Title;
        public string Version;
        public bool KeepCorrectDirectory = true;
        public string BottomBox;
    }
}