using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillStar : MonoBehaviour
{
    [SerializeField]
    private Sprite star;

    public bool flag = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void starEnable(GameObject stagePanel)
    {
        string prefix = "Stage";

        for (int i = 0; i < stagePanel.transform.childCount; i++)
        {
            GameObject child = stagePanel.transform.GetChild(i).gameObject;
            if (!child.name.StartsWith(prefix)) continue;

            if (flag)
            {
                child.GetComponent<Button>().interactable = false;
                continue;
            }

            int stageNum = int.Parse(child.name.Substring(prefix.Length));

            if (StarManager.Instance.starList.Count <= stageNum)
            {
                flag = true;
                continue;
            }

            if (StarManager.Instance.starList[stageNum] == 0)
            {
                flag = true;
            }

            for (int j = 1; j <= 3; j++)
            {
                child.transform.GetChild(j).gameObject.SetActive(true);
                if (j <= StarManager.Instance.starList[stageNum])
                    child.transform.GetChild(j).GetComponent<Image>().sprite = star;
            }
        }
    }
}