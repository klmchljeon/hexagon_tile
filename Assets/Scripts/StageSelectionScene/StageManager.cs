using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    public int stageIndex;

    public StageData currentStageData = null;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}