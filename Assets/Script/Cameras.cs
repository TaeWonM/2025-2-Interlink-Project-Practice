using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cameras
{
    public class CameraDict : MonoBehaviour
    {
        public static Dictionary<string, Camera> cameras = new Dictionary<string, Camera>();
    }
}