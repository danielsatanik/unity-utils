using UnityEngine;

namespace UnityUtils.Engine.UI
{
    public static class EditorStyles
    {
        public static GUIStyle Title
        {
            get
            {
                GUIStyle s = new GUIStyle();
                s.alignment = TextAnchor.MiddleCenter;
                s.fontSize = 20;
//        s.normal.background = MakeTex(9999, 40, Color.white);
                s.normal.textColor = Color.white;
                return s;
            }
        }

        public static GUIStyle BottomBox
        {
            get
            {
                GUIStyle s = new GUIStyle();
                s.alignment = TextAnchor.LowerLeft;
                s.normal.textColor = Color.white;
                return s;
            }
        }

        static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }
    }
}