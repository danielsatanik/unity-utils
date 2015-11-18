#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace UnityUtils.Manager.Editor
{
    [InitializeOnLoad]
    static class KeepInCorrectDirectory
    {
        static KeepInCorrectDirectory()
        {
            var Information = Resources.Load<PluginInformation>("PluginInformation");
            if (Information.KeepCorrectDirectory)
            {
                var path = AssetDatabase.GetAllAssetPaths()
				.FirstOrDefault(x => x.Contains("KeepInCorrectDirectory.cs"));
                var dirs = path.Split('/');
                var index = System.Array.IndexOf(dirs, "PluginManager");
                if (index > 0)
                {
                    var newPath = string.Join("/", dirs.Take(index).ToArray());
                    if (newPath != "Assets/Unity Utils")
                        AssetDatabase.MoveAsset(newPath, "Assets/Unity Utils");
                }
            }
        }
    }
}
#endif