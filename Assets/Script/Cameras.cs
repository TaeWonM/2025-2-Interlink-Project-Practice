using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Cameras
{
    public class CameraDict : MonoBehaviour
    {
        public static Dictionary<string, Camera> cameras = new Dictionary<string, Camera>();
        public static Camera Curcameras;
        public static void AddComponent(Camera cam)
        {
            Curcameras = cam;
        }
        public static void SwitchCamera(string from, string to)
        {
            if (cameras.ContainsKey(from) && cameras.ContainsKey(to))
            {
                cameras[from].gameObject.SetActive(false);
                AddComponent(cameras[to]);
                cameras[to].gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("Not Vaild Name");
            }
        }
    }
}