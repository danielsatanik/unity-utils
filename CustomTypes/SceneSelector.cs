using System;

namespace UnityUtils.CustomTypes
{
    [Serializable]
    public class SceneList
    {
        public string[] Values;

        public int Length { get { return Values.Length; } }
    }
}