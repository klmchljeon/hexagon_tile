using UnityEngine;
using UnityEngine.Events;

public class AndroidBackButtonHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent onBackButtonPressed;

    void Update()
    {
        // 안드로이드의 뒤로가기 키가 눌렸는지 확인
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 뒤로가기 키 이벤트 발생
            onBackButtonPressed?.Invoke();
        }
    }

    // 예시: 뒤로가기 키 이벤트를 수동으로 등록하는 메서드
    public void AddBackButtonListener(UnityAction action)
    {
        onBackButtonPressed.AddListener(action);
    }

    public void RemoveBackButtonListener(UnityAction action)
    {
        onBackButtonPressed.RemoveListener(action);
    }
}
