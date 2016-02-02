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
                s.normal.textColor = Color.white;
                return s;
            }
        }

        public static GUIStyle HeaderLeft
        {
            get
            {
                GUIStyle s = new GUIStyle();
                s.alignment = TextAnchor.MiddleLeft;
                s.fontSize = 14;
                s.normal.textColor = Color.white;
                return s;
            }
        }

        public static GUIStyle HeaderCenter
        {
            get
            {
                GUIStyle s = new GUIStyle();
                s.alignment = TextAnchor.MiddleCenter;
                s.fontSize = 14;
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

        public static GUIStyle VeryLightBackground
        {
            get
            {
                GUIStyle s = new GUIStyle();
                Color color;
                if (!ColorUtility.TryParseHtmlString("#DEDEDE", out color))
                    color = Color.blue;
                s.normal.background = MakeTex(9999, 40, color);
                s.clipping = TextClipping.Clip;
                return s;
            }
        }

        public static GUIStyle LightBackground
        {
            get
            {
                GUIStyle s = new GUIStyle();
                Color color;
                if (!ColorUtility.TryParseHtmlString("#D8D8D8", out color))
                    color = Color.blue;
                s.normal.background = MakeTex(9999, 40, color);
                s.clipping = TextClipping.Clip;
                return s;
            }
        }

        public static GUIStyle DarkBackground
        {
            get
            {
                GUIStyle s = new GUIStyle();
                Color color;
                if (!ColorUtility.TryParseHtmlString("#3C3C3C", out color))
                    color = Color.blue;
                s.normal.background = MakeTex(9999, 40, color);
                s.clipping = TextClipping.Clip;
                return s;
            }
        }

        public static GUIStyle VeryDarkBackground
        {
            get
            {
                GUIStyle s = new GUIStyle();
                Color color;
                if (!ColorUtility.TryParseHtmlString("#383838", out color))
                    color = Color.blue;
                s.padding = new RectOffset(2, 2, 2, 2);
                s.clipping = TextClipping.Clip;
                s.normal.background = MakeTex(9999, 40, color);
                return s;
            }
        }

        public static GUIStyle LightSelectedBackground
        {
            get
            {
                GUIStyle s = new GUIStyle();
                Color color;
                if (!ColorUtility.TryParseHtmlString("#397AEA", out color))
                    color = Color.blue;
                s.padding = new RectOffset(2, 2, 2, 2);
                s.clipping = TextClipping.Clip;
                s.normal.background = MakeTex(9999, 40, color);
                return s;
            }
        }

        public static GUIStyle DarkSelectedBackground
        {
            get
            {
                GUIStyle s = new GUIStyle();
                Color color;
                if (!ColorUtility.TryParseHtmlString("#3D5E98", out color))
                    color = Color.blue;
                s.padding = new RectOffset(2, 2, 2, 2);
                s.clipping = TextClipping.Clip;
                s.normal.background = MakeTex(9999, 40, color);
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