using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIDebugger : MonoBehaviour
{
    void Update()
    {
        // 마우스 클릭을 했을 때 (혹은 매 프레임)
        if (UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckUI();
        }
    }

    void CheckUI()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = UnityEngine.InputSystem.Mouse.current.position.ReadValue();

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            Debug.Log("👇 [클릭 감지됨] 가장 위에 있는 UI: " + results[0].gameObject.name);
            foreach (var result in results)
            {
                Debug.Log("   - 뚫고 지나간 UI들: " + result.gameObject.name);
            }
        }
        else
        {
            Debug.Log("❌ [감지 실패] UI가 감지되지 않음 (허공을 클릭함)");
        }
    }
}