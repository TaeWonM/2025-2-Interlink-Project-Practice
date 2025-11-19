using Cameras;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.XR;

public class OnWork : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Image[] Images;
    private Image Hand;
    private Camera OnWork_Cammera;
    private Material matInstance;
    void Start()
    {
        Images = this.GetComponentsInChildren<Image>(true);
        Hand = Images[1];
        Hand.material = null;
        matInstance = Resources.Load<Material>("New Material");
        OnWork_Cammera = GetComponentInChildren<Camera>(true);
        CameraDict.cameras.Add("OnWork", OnWork_Cammera);
    }

    void Update()
    {
        if (OnWork_Cammera.gameObject.activeSelf)
        {
            if (!Hand.gameObject.activeSelf) componentSetActive(true);
            highlight();
        }
    }
    void highlight()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        if (RectTransformUtility.RectangleContainsScreenPoint(Hand.rectTransform, mousePosition, OnWork_Cammera))
        {
            Hand.material = matInstance;
        }
        else
        {
            Hand.material = null;
        }
    }
    void componentSetActive(bool active)
    {
        Hand.gameObject.SetActive(active);
    }
}