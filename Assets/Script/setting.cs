using TMPro;
using UnityEngine;
using UnityEngine.UI;

class SettingUI : MonoBehaviour
{
    private Scrollbar Reliablilty;
    private TextMeshProUGUI Persent;
    private void Start()
    {
        Reliablilty = GetComponentInChildren<Scrollbar>();
        foreach(TextMeshProUGUI tmp in GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (tmp != null || tmp.name.Contains("Per")) Persent = tmp;
        }
        if (Persent == null)
        {
            Debug.LogError("Persent (TextMeshProUGUI) 컴포넌트를 찾지 못했습니다. 이름 및 타입 확인 필요.");
        }
    }
    public void OnScrollChanged ()
    {
        float tmp = Reliablilty.value;
        string percentageText = (tmp * 100f).ToString("F0") + "%";
        Persent.text = percentageText;
    }
}
