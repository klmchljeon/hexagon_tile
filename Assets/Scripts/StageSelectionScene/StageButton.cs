using System;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : SceneChange
{
    [SerializeField]
    private int idx;

    // 버튼이 클릭되었을 때 실행되는 함수
    protected override void TransitionToScene()
    {
        // 클릭된 버튼의 StageData를 StageManager에 저장
        StageManager.Instance.stageIndex = idx;
        StageManager.Instance.currentStageData = StageManager.Instance.stageDatas[idx];

        // 게임 씬으로 이동
        StartCoroutine(FadeOutAndLoadScene());
    }
}
