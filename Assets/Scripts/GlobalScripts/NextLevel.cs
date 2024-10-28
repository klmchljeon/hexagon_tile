using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : SceneChange
{
    protected override void TransitionToScene()
    {
        StageManager.Instance.stageIndex++;
        StageManager.Instance.currentStageData = StageLoader.LoadStage(StageManager.Instance.stageIndex / 20, StageManager.Instance.stageIndex % 20);
        

        if (StageManager.Instance.currentStageData == null)
        {
            Debug.Log("마지막 스테이지");
            return;
        }

        base.TransitionToScene();
    }
}
