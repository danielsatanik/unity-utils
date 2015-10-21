#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityUtils.SpatialUI;

namespace UnityUtils.SpatialUI.Editor
{
    static class SpatialUIMenu
    {
        [MenuItem("Unity Utils/Spatial UI/Add Spatial UI Canvas", false, 2)]
        static void AddSpatialUICanvas()
        {
            // create spatial ui canvas
            GameObject go = GameObject.Instantiate(Resources.Load("prefabs/Spatial UI Canvas")) as GameObject;
            go.transform.SetParent(Selection.activeTransform);
            go.AddComponent<RotateCanvas>();
            go.name = "Spatial UI Canvas";

            // destroy old camera
            var oldCamera = GameObject.FindObjectOfType<Camera>();
            if (oldCamera != null &&
                oldCamera.tag != "MainCamera" &&
                oldCamera.tag != "Spatial UI Camera")
            {
                GameObject.DestroyImmediate(oldCamera.gameObject);
                oldCamera = null;
            }
            if (oldCamera == null)
            {
                // add new spatial main camera
                var newCamera = GameObject.Instantiate(Resources.Load("prefabs/Spatial Main Camera")) as GameObject;
                newCamera.name = "Spatial Main Camera";
                newCamera.tag = "MainCamera";
            }
            var currentCamera = GameObject.FindGameObjectWithTag("MainCamera");
            Debug.Assert(currentCamera != null, "Something wicked happened here.");
            go.GetComponent<Canvas>().worldCamera = currentCamera.GetComponent<Camera>();

            // select newly created object
            Selection.activeObject = go;
        }

        [MenuItem("Unity Utils/Spatial UI/Add Spatial UI Canvas", true)]
        static bool ValidateAddSpatialUICanvas()
        {
            return Selection.activeGameObject != null &&
            Selection.activeGameObject.GetComponent<Camera>() == null &&
            Selection.activeGameObject.layer != LayerMask.NameToLayer("UI");
        }
    }
}
#endif