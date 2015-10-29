#if UNITY_EDITOR
using UnityEditor;
using UnityUtils.Manager;
using UnityUtils.Editor.Utilities;
using UnityUtils.Debugging;
using UnityEngine;

namespace UnityUtils.Manager.Editor
{
	static class PluginManagerMenu
	{
		[MenuItem ("Unity Utils/Update", false, 100)]
		static void Update ()
		{
			Application.OpenURL ("http://git.satanik.at/private/unity-utils/wikis/home");
		}

		[MenuItem ("Unity Utils/Update", true)]
		static bool ValidateUpdate ()
		{
			return true;
		}

		#if UNITY_UTILS_DEVELOPMENT
        [MenuItem("Unity Utils/Export", false, 108)]
        static void Export()
        {
            AssetDatabase.ExportPackage("Assets/Unity Utils", "Assets/Unity Utils/Unity Utils.unitypackage", ExportPackageOptions.Recurse | ExportPackageOptions.Interactive);
        }
        #endif
        
		[MenuItem ("Unity Utils/About", false, 109)]
		static void About ()
		{
			string path = "Assets/Unity Utils/PluginManager/Resources";
			var settings = AssetDatabase.LoadAssetAtPath<PluginInformation> (path + "/PluginInformation.asset");

			if (settings == null)
				settings = ScriptableObjectUtility.CreateAsset<PluginInformation> (path);

			EditorUtility.FocusProjectWindow ();
			Selection.activeObject = settings;
		}

		#if !UNITY_UTILS_DEVELOPMENT
		[MenuItem ("Unity Utils/About", true)]
		static bool ValidateAbout ()
		{
			string path = "Assets/Unity Utils/PluginManager/Resources";
			var settings = AssetDatabase.LoadAssetAtPath<PluginInformation> (path + "/PluginInformation.asset");

			return settings != null;
		}
		#endif
	}
}
#endif