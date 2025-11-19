using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.XR;

public class AfterWork : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Image[] Images;
    private Image Hand;
    private Camera AfterWork_Camera;
    private Material matInstance;
    void Start()
    {
        Images = this.GetComponentsInChildren<Image>(true);
        Hand = Images[1];
        Hand.material = null;
        matInstance = Resources.Load<Material>("New Material");
        AfterWork_Camera = GetComponentInChildren<Camera>(true);
    }

    void Update()
    {
        if (AfterWork_Camera != null) highlight();
    }
    void highlight()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        if (RectTransformUtility.RectangleContainsScreenPoint(Hand.rectTransform, mousePosition, AfterWork_Camera))
        {
            Hand.material = matInstance;
        }
        else
        {
            Hand.material = null;
        }
    }
}