#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

namespace UnityUtils.Editor.Utilities
{
    public static class ScriptableObjectUtility
    {
        /// <summary>
        //  This makes it easy to create, name and place unique new ScriptableObject asset files.
        /// </summary>
        public static T CreateAsset<T>(string path = null) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

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
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + typeof(T).Name + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return asset;
        }

        /// <summary>
        /// Create new asset from <see cref="ScriptableObject"/> type with unique name at
        /// selected folder in project window. Asset creation can be cancelled by pressing
        /// escape key when asset is initially being named.
        /// </summary>
        /// <typeparam name="T">Type of scriptable object.</typeparam>
        public static void CreateAssetWithDialog<T>() where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();
            ProjectWindowUtil.CreateAsset(asset, "New " + typeof(T).Name + ".asset");
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