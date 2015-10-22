#if UNITY_EDITOR
using UnityEditor;
using UnityUtils.Editor.Utilities;

namespace UnityUtils.Debugging.Editor
{
	static class LoggerSettingsMenu
	{
		[MenuItem ("Unity Utils/Logger/Open Settings", false, 1)]
		static void OpenLoggerSettings ()
		{
			string path = "Assets/Unity Utils/Logger/Resources";
			var settings = AssetDatabase.LoadAssetAtPath<LoggerSettings> (path + "/LoggerSettings.asset");

			if (settings == null) {
				settings = ScriptableObjectUtility.CreateAsset<LoggerSettings> (path);
			}

			EditorUtility.FocusProjectWindow ();
			Selection.activeObject = settings;
		}
	}
}
#endif