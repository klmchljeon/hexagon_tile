using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearManager : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject layerMask;
    [SerializeField] GameObject startPanel;
    [SerializeField] Sprite star;

    void Start()
    {
        GameManager.Instance.GameEnd += Clear;
    }

    void Clear(int ap, Vector3Int starCount)
    {
        GameManager.Instance.GameEnd -= Clear;

        panel.SetActive(true);
        layerMask.SetActive(true);

        int cnt;
        if (ap < 0)
        {
            cnt = 0;
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
}
