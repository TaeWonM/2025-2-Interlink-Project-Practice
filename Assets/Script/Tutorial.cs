using Cameras;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.XR;

public class Tutorial : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Image [] Images;
    private Image Hand;
    private Image TV;
    private GameObject Phone;
    private Image Phone_Camera_Icon;
    private Image Phone_Email_Icon;
    private Image Phone_Call_Icon;
    private NewActions action;
    private Material matInstance;
    private Camera Tutorial_Cammera;
    private bool is_Phone_On = false;
    void Start()
    {
        Images = this.GetComponentsInChildren<Image>(true);
        Hand = Images[1];
        Hand.material = null;
        TV = Images[2];
        TV.material = null;
        matInstance = Resources.Load<Material>("New Material");
        Tutorial_Cammera = GetComponentInChildren<Camera>(true);
        Phone = GameObject.Find("Phone");
        Image[] I = Phone.GetComponentsInChildren<Image>(true);
        Phone_Camera_Icon = I[2];
        Phone_Email_Icon = I[3];
        Phone_Call_Icon = I[4];
        Phone.SetActive(false);
        CameraDict.cameras.Add("Tutorial", Tutorial_Cammera);
    }

    void Update()
    {
        if (Tutorial_Cammera.gameObject.activeSelf)
        {
            if (!TV.gameObject.activeSelf && !Hand.gameObject.activeSelf)
            {
                Debug.Log("True");
                componentSetActive(true);
            }
            highlight();
        }
    }
    private void highlight()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        if (is_Phone_On)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(Phone_Camera_Icon.rectTransform, mousePosition, Tutorial_Cammera))
            {
                Phone_Camera_Icon.material = matInstance;
            }
            else
            {
                Phone_Camera_Icon.material = null;
            }
            if (RectTransformUtility.RectangleContainsScreenPoint(Phone_Call_Icon.rectTransform, mousePosition, Tutorial_Cammera))
            {
                Phone_Call_Icon.material = matInstance;
            }
            else Phone_Call_Icon.material = null;
            if (RectTransformUtility.RectangleContainsScreenPoint(Phone_Email_Icon.rectTransform, mousePosition, Tutorial_Cammera))
            {
                Phone_Email_Icon.material = matInstance;
            }
            else Phone_Email_Icon.material = null;
        }
        else
        {
            // 2. [수정] RectTransformUtility 함수에 반드시 'uiCamera'를 전달합니다.
            if (RectTransformUtility.RectangleContainsScreenPoint(
                    TV.rectTransform,
                    mousePosition,
                    Tutorial_Cammera  // <--- 이 인자가 'Screen Space - Camera' 모드의 핵심입니다.
                ))
            {
                TV.material = matInstance;
            }
            else
            {
                TV.material = null;
            }
            if (RectTransformUtility.RectangleContainsScreenPoint(Hand.rectTransform, mousePosition, Tutorial_Cammera))
            {
                Hand.material = matInstance;
            }
            else Hand.material = null;
        }
    }
    public void OnClickhand()
    {
        is_Phone_On = true;
        Phone.gameObject.SetActive(true);
    }
    public void OnClickbtn()
    {
        is_Phone_On = false;
        Phone.gameObject.SetActive(false);
    }
    public void OnClickTV()
    {
        Tutorial_Cammera.gameObject.SetActive(false);
        componentSetActive(false);
        CameraDict.cameras["OnCommute"].gameObject.SetActive(true);
    }
    void componentSetActive(bool active)
    {
        Hand.gameObject.SetActive(active);
        TV.gameObject.SetActive(active);
    }
}

