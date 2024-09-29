using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMakerManager : MonoBehaviour
{
    [SerializeField] private GameObject ObjectSelect;
    [SerializeField] private GameObject Map;

    [SerializeField] private string fileName;

    Button[] objectButtons;
    Button[] mapButtons;

    int selectIndex = -1;

    StageData stageData;

    private void Awake()
    {
        objectButtons = ObjectSelect.GetComponentsInChildren<Button>();
        foreach (Button button in objectButtons)
        {
            button.onClick.AddListener(() => Select(button));
        }

        mapButtons = Map.GetComponentsInChildren<Button>();
        foreach (Button button in mapButtons)
        {
            button.onClick.AddListener(() => Apply(button));
        }
    }

    private void OnDestroy()
    {
        foreach (Button button in objectButtons)
        {
            button.onClick.RemoveAllListeners();
        }
        foreach (Button button in mapButtons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        string stagePath = $"Assets/Stage/{fileName}.asset";

        // StageData가 없으면 생성, 있으면 불러옴
        stageData = ScriptableObjectUtility.CreateOrLoadScriptableObject<StageData>(stagePath);
        for (int i = 0; i < stageData.tileInfos.Length; i++)
        {
            Init(i);
        }
    }

    void Select(Button button)
    {
        // 버튼이 부모에서 몇 번째 자식인지 확인
        selectIndex = button.transform.GetSiblingIndex();
        Debug.Log($"버튼 {button.name}은 부모의 {selectIndex} 번째 자식입니다.");
    }

    void Apply(Button button)
    {
        int buttonIndex = button.transform.GetSiblingIndex();
        switch (selectIndex)
        {
            //타일
            case 0:
            case 1:
            case 2:
            case 3:
                TileApply(buttonIndex, selectIndex);
                break;

            case 4:
                SetPlayer(buttonIndex);
                break;

            case 5:
                SetCandy(buttonIndex);
                break;

            case 6:
                TileRotate(buttonIndex);
                break;

            case 7:
                TileCantRotate(buttonIndex);
                break;

            //지우개(10번타일,회전불가능,회전x)
            case 8:
                Erase(buttonIndex);
                break;

            default:
                Debug.Log("할당안됨");
                break;
        }

        GetComponent<SaveScriptableObjectAsJson>().SaveToJsonFile(stageData);
    }

    void Init(int tileIdx)
    {
        if (stageData.tileInfos[tileIdx] == null)
        {
            stageData.tileInfos[tileIdx] = new TileInfo();
        }

        if (stageData.tileInfos[tileIdx].tileNum == 10)
        {
            Erase(tileIdx);
            return;
        }

        TileApply(tileIdx, stageData.tileInfos[tileIdx].tileNum);

        Vector2Int loc = new Vector2Int(tileIdx % 6, tileIdx / 6);
        for (int i = 0; i < stageData.playerInfos.Length; i++)
        {
            if (stageData.playerInfos[i].loc == loc)
            {
                GameObject child = new GameObject("player");
                child.transform.SetParent(mapButtons[tileIdx].transform);
                child.AddComponent<Image>().sprite = objectButtons[4].GetComponent<Image>().sprite;
                child.transform.localPosition = Vector3.zero;
                break;
            }
        }

        for (int i = 0; i < stageData.candyInfos.Length; i++)
        {
            if (stageData.candyInfos[i].loc == loc)
            {
                GameObject child = new GameObject("candy");
                child.transform.SetParent(mapButtons[tileIdx].transform);
                child.AddComponent<Image>().sprite = objectButtons[5].GetComponent<Image>().sprite;
                child.transform.localPosition = Vector3.zero;
                break;
            }
        }
    }

    void TileApply(int tileIdx, int type)
    {
        mapButtons[tileIdx].GetComponent<Image>().sprite = objectButtons[type].GetComponent<Image>().sprite;

        if (stageData.tileInfos[tileIdx].tileNum == 10)
        {
            mapButtons[tileIdx].transform.localEulerAngles = new Vector3(0, 0, 0);
            stageData.tileInfos[tileIdx].isRotate = false;

            mapButtons[tileIdx].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            stageData.tileInfos[tileIdx].cantRotate = false;
        }
        else
        {
            if (stageData.tileInfos[tileIdx].isRotate)
            {
                mapButtons[tileIdx].transform.localEulerAngles = new Vector3(0, 0, (type != 1) ? 180f : 60f);
                if (mapButtons[tileIdx].transform.childCount != 0)
                {
                    GameObject child = mapButtons[tileIdx].transform.GetChild(0).gameObject;
                    child.transform.localEulerAngles = new Vector3(0, 0, -((type != 1) ? 180f : 60f));
                }
            }
            if (stageData.tileInfos[tileIdx].cantRotate)
            {
                mapButtons[tileIdx].GetComponent<Image>().color = new Color(190f / 255f, 190f / 255f, 190f / 255f, 1f);
            }
        }

        stageData.tileInfos[tileIdx].tileNum = type;
    }

    void TileRotate(int tileIdx)
    {
        if (stageData.tileInfos[tileIdx].tileNum == 10) return;

        if (stageData.tileInfos[tileIdx].isRotate)
        {
            stageData.tileInfos[tileIdx].isRotate = false;
            mapButtons[tileIdx].transform.localEulerAngles = new Vector3(0, 0, 0);
            if (mapButtons[tileIdx].transform.childCount != 0)
            {
                GameObject child = mapButtons[tileIdx].transform.GetChild(0).gameObject;
                child.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
        else
        {
            stageData.tileInfos[tileIdx].isRotate = true;
            mapButtons[tileIdx].transform.localEulerAngles = new Vector3(0, 0, (stageData.tileInfos[tileIdx].tileNum != 1) ? 180f : 60f);
            if (mapButtons[tileIdx].transform.childCount != 0)
            {
                GameObject child = mapButtons[tileIdx].transform.GetChild(0).gameObject;
                child.transform.localEulerAngles = new Vector3(0, 0, -((stageData.tileInfos[tileIdx].tileNum != 1) ? 180f : 60f));
            }
        }
    }

    void SetPlayer(int tileIdx)
    {
        if (stageData.tileInfos[tileIdx].tileNum == 10) return;

        Vector2Int loc = new Vector2Int(tileIdx % 6, tileIdx / 6);

        for (int i = 0; i < stageData.playerInfos.Length; i++)
        {
            if (stageData.playerInfos[i].loc == loc) return;
            if (stageData.candyInfos[i].loc == loc) return;
        }

        for (int i = 0; i < stageData.playerInfos.Length; i++)
        {
            if (stageData.playerInfos[i].loc == new Vector2Int(-1,-1))
            {
                //stageData.playerInfos[i] = new PlayerInfo();
                stageData.playerInfos[i].loc = loc;
                GameObject child = new GameObject("player");
                child.transform.SetParent(mapButtons[tileIdx].transform);
                child.AddComponent<Image>().sprite = objectButtons[4].GetComponent<Image>().sprite;
                child.transform.localPosition = Vector3.zero;
                break;
            }
        }
    }

    void SetCandy(int tileIdx)
    {
        if (stageData.tileInfos[tileIdx].tileNum == 10) return;

        Vector2Int loc = new Vector2Int(tileIdx % 6, tileIdx / 6);

        for (int i = 0; i < stageData.candyInfos.Length; i++)
        {
            if (stageData.playerInfos[i].loc == loc) return;
            if (stageData.candyInfos[i].loc == loc) return;
        }

        for (int i = 0; i < stageData.candyInfos.Length; i++)
        {
            if (stageData.candyInfos[i].loc == new Vector2Int(-1,-1))
            {
                //stageData.candyInfos[i] = new CandyInfo();
                stageData.candyInfos[i].loc = loc;
                GameObject child = new GameObject("candy");
                child.transform.SetParent(mapButtons[tileIdx].transform);
                child.AddComponent<Image>().sprite = objectButtons[5].GetComponent<Image>().sprite;
                child.transform.localPosition = Vector3.zero;
                break;
            }
        }
    }

    void TileCantRotate(int tileIdx)
    {
        if (stageData.tileInfos[tileIdx] == null) return;
        if (stageData.tileInfos[tileIdx].tileNum == 10) return;

        if (stageData.tileInfos[tileIdx].cantRotate)
        {
            stageData.tileInfos[tileIdx].cantRotate = false;
            mapButtons[tileIdx].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            stageData.tileInfos[tileIdx].cantRotate = true;
            mapButtons[tileIdx].GetComponent<Image>().color = new Color(190f / 255f, 190f / 255f, 190f / 255f, 1f);
        }
    }

    void Erase(int tileIdx)
    {
        stageData.tileInfos[tileIdx].tileNum = 10;
        mapButtons[tileIdx].GetComponent<Image>().sprite = objectButtons[8].GetComponent<Image>().sprite;

        if (mapButtons[tileIdx].transform.childCount != 0)
        {
            GameObject child = mapButtons[tileIdx].transform.GetChild(0).gameObject;
            Vector2Int loc = new Vector2Int(tileIdx % 6, tileIdx / 6);

            if (child.name == "player")
            {
                for (int i = 0; i < stageData.playerInfos.Length; i++)
                {
                    if (stageData.playerInfos[i].loc == loc)
                    {
                        stageData.playerInfos[i].loc = new Vector2Int(-1,-1);
                        break;
                    }
                }
            }
            else if (child.name == "candy")
            {
                for (int i = 0; i < stageData.candyInfos.Length; i++)
                {
                    if (stageData.candyInfos[i].loc == loc)
                    {
                        stageData.candyInfos[i].loc = new Vector2Int(-1,-1);
                        break;
                    }
                }
            }

            Destroy(child);
        }

        stageData.tileInfos[tileIdx].isRotate = false;
        stageData.tileInfos[tileIdx].cantRotate = true;
        stageData.tileInfos[tileIdx].color = 0;
    }
}
