using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CloseUI : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ClosePanel);
    }

    private void OnDisable()
    {
        gameObject.GetComponent<Button>().onClick.RemoveListener(ClosePanel);
    }

    void ClosePanel()
    {
        if (IsPointerOverUIObject() && !IsPointerOverChildUI())
        {
            gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        // Raycast를 통해 클릭된 모든 UI 요소를 가져옴
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            // 클릭된 객체가 설정 창 그 자체인지 확인 (자식 제외)
            if (result.gameObject == gameObject)
            {
                return true; // 클릭된 객체가 해당 UI 요소라면 true 반환
            }
        }

        return false; // UI 요소가 아니거나 자식일 경우 false 반환
    }

    private bool IsPointerOverChildUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            // 클릭된 객체가 부모 객체의 자식인지 확인
            if (result.gameObject.transform.IsChildOf(gameObject.transform) && result.gameObject.transform != gameObject.transform)
            {
                return true; // 자식이 클릭되었다면 true 반환
            }
        }
        return false;
    }
}
