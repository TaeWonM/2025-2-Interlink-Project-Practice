using Cameras;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.XR;

public class OnCommute : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Image[] Images;
    private Image Human;
    private Image Hand;
    private Camera OnCommute_Cammera;
    private Material matInstance;
    void Start()
    {
        Images = this.GetComponentsInChildren<Image>(true);
        Human = Images[1];
        Human.material = null;
        Hand = Images[2];
        Hand.material = null;
        matInstance = Resources.Load<Material>("New Material");
        OnCommute_Cammera = GetComponentInChildren<Camera>(true);
        CameraDict.cameras.Add("OnCommute", OnCommute_Cammera);
    }

    void Update()
    {
        if (OnCommute_Cammera.gameObject.activeSelf)
        {
            if (!Human.gameObject.activeSelf && !Hand.gameObject.activeSelf) componentSetActive(true);
            Highlight();
        }
    }
    void Highlight()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        if (RectTransformUtility.RectangleContainsScreenPoint(Hand.rectTransform, mousePosition, OnCommute_Cammera))
        {
            Hand.material = matInstance;
        }
        else
        {
            Hand.material = null;
        }
        if (RectTransformUtility.RectangleContainsScreenPoint(Human.rectTransform, mousePosition, OnCommute_Cammera))
        {
            Human.material = matInstance;
        }
        else Human.material = null;
    }
    public void OnClickHand()
    {
        OnCommute_Cammera.gameObject.SetActive(false);
        componentSetActive(false);
        CameraDict.cameras["OnWork"].gameObject.SetActive(true);
    }
    void componentSetActive(bool active)
    {
        Human.gameObject.SetActive(active);
        Hand.gameObject.SetActive(active);
    }
}