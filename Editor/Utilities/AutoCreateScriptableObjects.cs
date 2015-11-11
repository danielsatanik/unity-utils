#if UNITY_EDITOR
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityUtils.Attributes;

namespace UnityUtils.Editor.Utilities
{
    [InitializeOnLoad]
    static class AutoCreateScriptableObjects
    {
        static AutoCreateScriptableObjects()
        {
            IEnumerable<Type> types = from a in AppDomain.CurrentDomain.GetAssemblies()
                                               from t in a.GetTypes()
                                               where t.IsDefined(typeof(AutoCreateAssetAttribute), false)
                                               select t;

            foreach (var t in types)
            {
                string path = (t.GetCustomAttributes(typeof(AutoCreateAssetAttribute), false)[0] as AutoCreateAssetAttribute).Path;
                if (t.IsSubclassOf(typeof(ScriptableObject)))
                {
                    ScriptableObjectUtility.CreateAssetSafe(t, path);
                }
            }
        }
    }
}
#endif