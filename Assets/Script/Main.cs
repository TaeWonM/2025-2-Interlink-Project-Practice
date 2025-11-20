using UnityEngine;
using Cameras;
public class Main : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Camera Main_Camera;
    void Start()
    {
        Main_Camera = GetComponentInChildren<Camera>(true);
        if (Main_Camera == null) Debug.Log("Null point in Main_Camera");
        CameraDict.cameras.Add("Main", Main_Camera);
        CameraDict.AddComponent(Main_Camera);
    }

    // Update is called once per frame
    public void OnClicked()
    {
        CameraDict.SwitchCamera("Main", "Tutorial");
    }
}
