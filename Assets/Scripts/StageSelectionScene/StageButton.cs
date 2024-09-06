using System;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    public StageData stageData; // 버튼에 할당할 StageData
    Button button;       // 유니티의 버튼 컴포넌트

    private void Start()
    {
        button = GetComponent<Button>();
        // 버튼 클릭 시 이벤트 할당
        button.onClick.AddListener(OnStageButtonClick);
    }

    // 버튼이 클릭되었을 때 실행되는 함수
    void OnStageButtonClick()
    {
        // 클릭된 버튼의 StageData를 StageManager에 저장
        StageManager.Instance.currentStageData = stageData;

        // 게임 씬으로 이동
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}
