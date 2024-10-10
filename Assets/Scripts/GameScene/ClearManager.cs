using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClearManager : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject layerMask;
    [SerializeField] GameObject startPanel;
    [SerializeField] Sprite star;
    [SerializeField] GameObject nextStageButton;

    void Start()
    {
        GameManager.Instance.GameEnd += Clear;
    }

    void Clear(int ap, Vector3Int starCount)
    {
        GameManager.Instance.GameEnd -= Clear;

        panel.SetActive(true);
        layerMask.SetActive(true);

        
        if (StageLoader.LoadStage(StageManager.Instance.stageIndex + 1) == null)
        {
            NextButtonDisable();
        }

        int cnt;
        if (ap < 0)
        {
            cnt = 0;
            NextButtonDisable();
        }
        else if (ap <= starCount.x)
        {
            cnt = 1;
        }
        else if (ap <= starCount.y)
        {
            cnt = 2;
        }
        else if (ap <= starCount.z)
        {
            cnt = 3;
        }
        else
        {
            Debug.Log("더 최적화된 경로");
            cnt = 3;
        }

        StarManager.Instance.Clear(GameManager.Instance.stageNum, cnt);
        StartCoroutine(StarCountCoroutine(cnt));
    }

    private IEnumerator StarCountCoroutine(int cnt)
    {
        for (int i = 0; i < cnt; i++)
        {
            yield return new WaitForSeconds(0.5f);

            startPanel.transform.GetChild(i).GetComponent<Image>().sprite = star;
        }
    }

    void NextButtonDisable()
    {
        nextStageButton.GetComponent<Button>().interactable = false;
        Color txtColor = nextStageButton.GetComponentInChildren<TMP_Text>().color;
        txtColor.a = 0.5f;
        nextStageButton.GetComponentInChildren<TMP_Text>().color = txtColor;
    }
}
