using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusUI : MonoBehaviour
{
    GameManager gameManager;

    public TextMeshProUGUI stage;
    public TextMeshProUGUI candyCount;
    public TextMeshProUGUI actionPoint;

    public StageData TestStage;

    private void Awake()
    {
        FirstUpdate();

        gameManager = GetComponent<GameManager>();
    }

    private void OnEnable()
    {
        gameManager.UpdateUI += UpdateUI;
    }

    private void OnDisable()
    {
        gameManager.UpdateUI -= UpdateUI;
    }

    void FirstUpdate()
    {
        StageData stageData;
        if (StageManager.Instance == null)
        {
            stageData = TestStage;
        }
        else
        {
            stageData = StageManager.Instance.currentStageData;
        }

        if (stageData.stageNumber == 0)
        {
            stage.text = "test";
        }
        else
        {
            stage.text = stageData.stageNumber.ToString();
        }
        candyCount.text = "1";
        actionPoint.text = stageData.actionPoint.ToString();
    }

    void UpdateUI(int x, int y)
    {
        candyCount.text = gameManager.candyCount.ToString();
        actionPoint.text = gameManager.actionPoint.ToString();
    }
}
