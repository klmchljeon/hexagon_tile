using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StagePanelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject panelExit;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject stage;
    [SerializeField] private int stageCnt;

    private Image fadeImage;

    // Start is called before the first frame update
    void Start()
    {
        fadeImage = FindInactiveUIObject<Image>("FadeImage");
        Generate(stageCnt);

        if (StageManager.Instance.stageIndex != -1)
        {
            panelExit.GetComponent<UIActiveStateMonitor>().SetUIActive(true);
        }
    }

    void Generate(int n)
    {
        int cnt = 0;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (cnt == n) break;
                cnt++;

                GameObject stageUI = Instantiate(stage);
                stageUI.transform.SetParent(panel.transform);

                Vector3 pos = new Vector3(-460f, 240f, 0);
                pos += new Vector3(230f * j, -165 * i, 0);

                stageUI.GetComponent<RectTransform>().anchoredPosition = pos;
                stageUI.name = $"Stage{cnt}";
                stageUI.GetComponentInChildren<TMP_Text>().text = $"{cnt}";

                stageUI.GetComponent<StageButton>().idx = cnt - 1;
                stageUI.GetComponent<SceneChange>().fadeImage = fadeImage;
            }
        }

        GetComponent<FillStar>().starEnable(panel);
    }

    private T FindInactiveUIObject<T>(string name) where T : Component
    {
        // 모든 T 타입의 오브젝트를 찾아옵니다 (비활성화된 것도 포함).
        T[] allObjects = Resources.FindObjectsOfTypeAll<T>();

        foreach (T obj in allObjects)
        {
            if (obj.name == name)
            {
                return obj;
            }
        }

        return null;
    }
}
