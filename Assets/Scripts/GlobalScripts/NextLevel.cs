using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : SceneChange
{
    protected override void TransitionToScene()
    {
        if (StageManager.Instance.stageIndex == StageManager.Instance.stageDatas.Length - 1)
        {
            Debug.Log("마지막 스테이지");
            return;
        }

        StageManager.Instance.currentStageData = StageManager.Instance.stageDatas[++StageManager.Instance.stageIndex];

        base.TransitionToScene();
    }
}
