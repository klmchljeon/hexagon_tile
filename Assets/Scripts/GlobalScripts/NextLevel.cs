using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : SceneChange
{
    protected override void TransitionToScene()
    {
        StageManager.Instance.currentStageData = StageLoader.LoadStage(++StageManager.Instance.stageIndex);
        if (StageManager.Instance.currentStageData == null)
        {
            Debug.Log("마지막 스테이지");
            return;
        }

        base.TransitionToScene();
    }
}
