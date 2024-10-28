using UnityEngine;

public class StageLoader : MonoBehaviour
{
    public static StageData LoadStage(int chap, int i)
    {
        // Resources/Stage 폴더에서 Stage{i}를 로드
        StageData stageData = Resources.Load<StageData>($"Chapter{chap + 1}/Stage{i + 1}");

        if (stageData != null)
        {
            Debug.Log($"Stage {chap + 1}-{i + 1} loaded successfully.");
            return stageData;
        }
        else
        {
            Debug.Log($"Stage {chap + 1}-{i + 1} not found. Make sure the file exists in the Resources/Chapter{chap + 1} folder.");
            return null;
        }
    }
}
