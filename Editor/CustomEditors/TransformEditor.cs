#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UnityUtils.Editor.CustomEditors
{
	[CustomEditor (typeof(Transform))]
	public class TransformEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI ()
		{
			Transform t = (Transform)target;
			// Replicate the standard transform inspector gui
			EditorGUIUtility.LookLikeControls ();
			GUILayout.BeginHorizontal ();
			bool resetPos = GUILayout.Button ("P", GUILayout.Width (20f));
			Vector3 position = EditorGUILayout.Vector3Field ("", t.localPosition, GUILayout.MaxHeight (20));
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			bool resetRot = GUILayout.Button ("R", GUILayout.Width (20f));
			Vector3 eulerAngles = EditorGUILayout.Vector3Field ("", t.localEulerAngles, GUILayout.MaxHeight (20));
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			bool resetScale = GUILayout.Button ("S", GUILayout.Width (20f));
			Vector3 scale = EditorGUILayout.Vector3Field ("", t.localScale, GUILayout.MaxHeight (20));
			GUILayout.EndHorizontal ();
			#pragma warning disable 0618
			EditorGUIUtility.LookLikeInspector ();
			#pragma warning restore 0618
    
			if (GUI.changed) {
				Undo.RecordObject (t, "Transform Change");
    
				if (!resetPos)
					t.localPosition = FixIfNaN (position);
				else
					t.localPosition = Vector3.zero;
				if (!resetRot)
					t.localEulerAngles = FixIfNaN (eulerAngles);
				else
					t.localEulerAngles = Vector3.zero;
				if (!resetScale)
					t.localScale = FixIfNaN (scale);
				else
					t.localScale = Vector3.zero;
			}
		}

		Vector3 FixIfNaN (Vector3 v)
		{
			if (float.IsNaN (v.x))
				v.x = 0;
			if (float.IsNaN (v.y))
				v.y = 0;
			if (float.IsNaN (v.z))
				v.z = 0;
			return v;
		}
	}
}
#endif