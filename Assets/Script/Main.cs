using Cameras;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
public class Main : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Camera Main_Camera;
    private Toggle Setting_Toggle;
    private Button Playing_Btn;
    void Start()
    {
        Setting_Toggle = GetComponentInChildren<Toggle>(true);
        Playing_Btn = GetComponentInChildren<Button>(true);
        Main_Camera = GetComponentInChildren<Camera>(true);
        if (Main_Camera == null) Debug.Log("Null point in Main_Camera");
        CameraDict.cameras.Add("Main", Main_Camera);
        CameraDict.AddComponent(Main_Camera);
        componentSetActive(true);
    }

    // Update is called once per frame
    public void OnClicked()
    {
        componentSetActive(false);
        CameraDict.SwitchCamera("Main", "Tutorial");
    }
    void componentSetActive(bool active)
    {
        Setting_Toggle.gameObject.SetActive(active);
        Playing_Btn.gameObject.SetActive(active);
    }
}
