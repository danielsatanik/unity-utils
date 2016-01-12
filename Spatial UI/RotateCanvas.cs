using UnityEngine;

namespace UnityUtils.SpatialUI
{
    [ExecuteInEditMode, DisallowMultipleComponent]
    public class RotateCanvas : MonoBehaviour
    {
        [SerializeField]float mDistanceFromCamera;
        Camera m_cam;

        void OnEnable()
        {
            var go = GameObject.FindGameObjectWithTag("MainCamera");
            Debug.Assert(go != null, "There has to be a MainCamera");
            GetComponent<Canvas>().worldCamera = go.GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            if (m_cam != null)
            {
                transform.LookAt(-(m_cam.transform.forward));
                transform.rotation = m_cam.transform.rotation;
                var d = (transform.parent.position - m_cam.transform.position).normalized;
                transform.position = m_cam.transform.position + d * mDistanceFromCamera;
            }
            else if (GetComponent<Canvas>().worldCamera != null)
            {
                m_cam = GetComponent<Canvas>().worldCamera;
            }
        }
    }
}