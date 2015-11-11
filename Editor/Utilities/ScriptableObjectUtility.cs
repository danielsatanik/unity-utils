#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System;

namespace UnityUtils.Editor.Utilities
{
    public static class ScriptableObjectUtility
    {
        public static ScriptableObject CreateAsset(Type type, string path = null)
        {
            ScriptableObject asset = ScriptableObject.CreateInstance(type);

            if (string.IsNullOrEmpty(path))
            {
                path = AssetDatabase.GetAssetPath(Selection.activeObject);
                if (path == "")
                    path = "Assets";
                else if (Path.GetExtension(path) != "")
                    path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + type.Name + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return asset;
        }

        public static T CreateAsset<T>(string path = null) where T : ScriptableObject
        {
            return CreateAsset(typeof(T), path) as T;
        }

        public static ScriptableObject CreateAssetSafe(Type type, string path)
        {
            var obj =
                AssetDatabase.LoadAssetAtPath(path + "/" + type.Name + ".asset", type) ??
                CreateAsset(type, path);

            return obj as ScriptableObject;
        }

        public static T CreateAssetSafe<T>(string path) where T : ScriptableObject
        {
            return CreateAssetSafe(typeof(T), path) as T;
        }

        public static void ShowAsset<T>(string path) where T : ScriptableObject
        {
            var obj =
                AssetDatabase.LoadAssetAtPath<T>(path + "/" + typeof(T).Name + ".asset") ??
                CreateAsset<T>(path);

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = obj;
        }
    }
}
#endif