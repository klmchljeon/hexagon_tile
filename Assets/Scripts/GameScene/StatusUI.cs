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
        FirstUpdate();
    }

    private void OnEnable()
    {
        GameManager.Instance.UpdateUI += UpdateUI;
    }

    private void OnDisable()
    {
        GameManager.Instance.UpdateUI -= UpdateUI;
    }

    void FirstUpdate()
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
        actionPoint.text = GameManager.Instance.actionPoint.ToString();
    }

    void UpdateUI()
    {
        candyCount.text = GameManager.Instance.candyCount.ToString();
        actionPoint.text = GameManager.Instance.actionPoint.ToString();
    }
}
