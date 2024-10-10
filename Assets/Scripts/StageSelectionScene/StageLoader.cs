using UnityEngine;

public class StageLoader : MonoBehaviour
{
    public static StageData LoadStage(int i)
    {
        // Resources/Stage 폴더에서 Stage{i}를 로드
        StageData stageData = Resources.Load<StageData>($"Stage/Stage{i+1}");

        if (stageData != null)
        {
            Debug.Log($"Stage {i+1} loaded successfully.");
            return stageData;
        }
        else
        {
            Debug.LogError($"Stage {i+1} not found. Make sure the file exists in the Resources/Stage folder.");
            return null;
        }
    }
}
