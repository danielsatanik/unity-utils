using System.IO;
using UnityEngine;

namespace UnityUtils.Engine.Utilities
{
    #if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
    #endif
    public static class PathUtility
    {

        public static string ProjectRootPath { get; private set; }

        public static string UnityApplicationPath { get; private set; }

        public static string Blub { get; private set; }

        static PathUtility()
        {
            ProjectRootPath = CreateProjectRootPath();
            UnityApplicationPath = Application.dataPath;
        }

        public static string GetPath(params string[] paths)
        {
            var path = string.Join("", paths);
            if (string.IsNullOrEmpty(path))
                throw new System.Exception("Path has to be a valid pathname");

            Directory.CreateDirectory(path);

            return path;
        }

        static string CreateProjectRootPath()
        {
            string path;
            #if UNITY_EDITOR
            string dataPath = Application.dataPath;
            path = dataPath.Remove(dataPath.Length - 7);
            Directory.CreateDirectory(path);
            #elif UNITY_ANDROID
            path = Application.persistentDataPath;
            #elif UNITY_IOS
            path = Application.persistentDataPath;
            #else
            path = Application.persistentDataPath;
            #endif

            return path;
        }
    }
}