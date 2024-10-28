using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StagePageManager : MonoBehaviour
{
    [SerializeField] private GameObject panelExit;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    private int pageIndex;

    // Start is called before the first frame update
    void Start()
    {
        if (StageManager.Instance.stageIndex != -1)
        {
            panelExit.GetComponent<UIActiveStateMonitor>().SetUIActive(true);

            pageIndex = StageManager.Instance.stageIndex / 20;
        }
        else
        {
            pageIndex = 0;
        }
        panelExit.transform.GetChild(pageIndex).GetComponent<UIActiveStateMonitor>().SetUIActive(true);

        leftButton.onClick.AddListener(() => OnButtonClick(leftButton));
        rightButton.onClick.AddListener(() => OnButtonClick(rightButton));
    }

    private void OnDisable()
    {
        // 해제
        leftButton.onClick.RemoveListener(() => OnButtonClick(leftButton));
        rightButton.onClick.RemoveListener(() => OnButtonClick(rightButton));
    }

    public void OnButtonClick(Button clickedButton)
    {
        int delta = clickedButton.GetComponent<StagePageButton>().delta;
        panelExit.transform.GetChild(pageIndex).GetComponent<UIActiveStateMonitor>().SetUIActive(false);

        pageIndex += delta;
        panelExit.transform.GetChild(pageIndex).GetComponent<UIActiveStateMonitor>().SetUIActive(true);
    }
}
