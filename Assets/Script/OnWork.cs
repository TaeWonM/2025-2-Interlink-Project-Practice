using Cameras;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.XR;
using static UnityEditor.Progress;
using static UnityEngine.ParticleSystem;

public class OnWork : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Image[] Images;
    private Image Hand;
    private Camera OnWork_Cammera;
    private Material matInstance;
    private Toggle SettingMotionTogle;
    private Transform HandController;
    private CursorTracker CusorObject;
    private GameObject Articles;
    private bool isarticlestate = false;
    private bool issubmit = false;
    private Image[] Article = new Image[2]; 
    private Image[] Clickable = new Image[4];
    delegate void Clickable_func();
    private Clickable_func [] Clickable_functions = new Clickable_func[4];
    void Start()
    {
        Images = this.GetComponentsInChildren<Image>(true);
        Hand = Images[1];
        Hand.material = null;
        matInstance = Resources.Load<Material>("New Material");
        OnWork_Cammera = GetComponentInChildren<Camera>(true);
        CameraDict.cameras.Add("OnWork", OnWork_Cammera);
        GameObject Settting = GameObject.Find("Setting");
        SettingMotionTogle = Settting.GetComponentsInChildren<Canvas>(true)[1].GetComponentsInChildren<Toggle>(true)[1];
        if (SettingMotionTogle == null)
        {
            Debug.LogError("SettingMotionTogle is Null");
        }
        GameObject temp =  GameObject.Find("Motion Capture");
        HandController = temp.GetComponentsInChildren<Transform>(true)[1];
        if (HandController == null)
        {
            Debug.LogError("HandController is Null");
        }
        CusorObject = temp.GetComponentInChildren<CursorTracker>(true);
        if (CusorObject == null)
        {
            Debug.LogError("CusorObject is Null");
        }
        Articles = GameObject.Find("Articles");
        if (Articles == null)
        {
            Debug.LogError("Articles is Null");
        }
        Articles.SetActive(false);
        Image[] images = Articles.GetComponentsInChildren<Image>(true);
        int i = 0;
        foreach (Image image in images)
        {
            if (image.name.Contains("Article "))
            {
                Article[i] = image;
                i+=1;
            }
        }
        Button[] Buttons = GetComponentsInChildren<Button>(true);
        Clickable[0] = Article[0]; Clickable_functions[0] = OnArticleClick;
        Clickable[1] = Article[1]; Clickable_functions[1] = OnArticle2Click;
        Clickable[2] = Buttons[0].image; Clickable_functions[2] = OnQuitClick;
        Clickable[3] = Buttons[1].image; Clickable_functions[3] = OnSumitClick;
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
        Vector2 mousePosition;
        mousePosition = Mouse.current.position.ReadValue();
        if (isarticlestate)
        {
            if (SettingMotionTogle.isOn)
            {
                if (CusorObject.vectorPos != null) mousePosition = CusorObject.vectorPos;
            }
            foreach (Image image in Article)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(image.rectTransform, mousePosition, OnWork_Cammera))
                    {
                        image.material = matInstance;
                        if (issubmit) { 
                            image.GetComponentsInChildren<Image>(true)[1].gameObject.SetActive(true); 
                        }
                    }
                else
                {
                    image.material = null;
                    if (issubmit)
                    {
                        image.GetComponentsInChildren<Image>(true)[1].gameObject.SetActive(false);
                    }
                }
            }
            if (CusorObject.dedected_bool)
            {
                int i = 0;
                foreach (Image tmp in Clickable)
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(tmp.rectTransform, CusorObject.vectorPos, OnWork_Cammera))
                    {
                        Clickable_functions[i]();
                        CusorObject.dedected_bool = false;
                    }
                    i = i + 1;
                }
            }
        }
        else
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(Hand.rectTransform, mousePosition, OnWork_Cammera))
            {
                Hand.material = matInstance;
            }
            else
            {
                Hand.material = null;
            }
        }
    }
    void componentSetActive(bool active)
    {
        Hand.gameObject.SetActive(active);
    }
    public void OnHandClick()
    {
        if (SettingMotionTogle.isOn)
        {
            HandController.gameObject.SetActive(true);
            CusorObject.gameObject.SetActive(true);
        }
        isarticlestate = true;
        Articles.SetActive(true);
    }
    public void OnQuitClick()
    {
        if (SettingMotionTogle.isOn)
        {
            HandController.gameObject.SetActive(false);
            CusorObject.gameObject.SetActive(false);
        }
        isarticlestate = false;
        Articles.SetActive(false);
    }
    public void OnSumitClick()
    {
        if (issubmit) issubmit = false;
        else issubmit = true;
    }
    public void OnArticleClick()
    {
        if (issubmit)
        {
            OnQuitClick();
            issubmit = false;
            Articles.SetActive(false);
            componentSetActive(false);
            CameraDict.SwitchCamera("OnWork", "AfterWork");
        }
    }

    public void OnArticle2Click()
    {
        if (issubmit)
        {
            OnQuitClick();
            issubmit = false;
            Articles.SetActive(false);
            componentSetActive(false);
            CameraDict.SwitchCamera("OnWork", "AfterWork");
        }
    }
}