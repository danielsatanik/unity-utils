#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using System.Diagnostics;

namespace UnityUtils.Editor.Utilities
{
    public static class DialogUtility
    {
        public static void OpenStackJumpDialog(string title, string message, int depth = 0)
        {
            if (EditorUtility.DisplayDialog(title, message, "Show", "Cancel"))
            {
                var myTrace = new StackTrace(true);
                StackFrame myFrame = myTrace.GetFrame(depth + 1);
                InternalEditorUtility.OpenFileAtLineExternal(myFrame.GetFileName(), myFrame.GetFileLineNumber());
            }
        }
    }
}
#endif