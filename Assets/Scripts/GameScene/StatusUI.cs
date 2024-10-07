using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusUI : MonoBehaviour
{

    public TextMeshProUGUI stage;
    public TextMeshProUGUI candyCount;
    public TextMeshProUGUI actionPoint;

    public StageData TestStage;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        GameManager.Instance.UpdateUI += UpdateUI;
    }

    private void OnDisable()
    {
        GameManager.Instance.UpdateUI -= UpdateUI;
    }

    void UpdateUI()
    {
        if (GameManager.Instance.stageNum == 0)
        {
            stage.text = "test";
        }
        else
        {
            stage.text = GameManager.Instance.stageNum.ToString();
        }
        candyCount.text = GameManager.Instance.candyCount.ToString();

        if (GameManager.Instance.actionPoint < 0)
        {
            actionPoint.text = "초과!";
        }
        else
        {
            actionPoint.text = GameManager.Instance.actionPoint.ToString();
        }
    }
}
