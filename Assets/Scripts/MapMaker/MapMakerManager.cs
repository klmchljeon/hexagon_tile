using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMakerManager : MonoBehaviour
{
    [SerializeField] private GameObject ObjectSelect;
    [SerializeField] private GameObject Map;

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
        string stagePath = "Assets/Stage/MapMaker1.asset";

        // StageData가 없으면 생성, 있으면 불러옴
        stageData = ScriptableObjectUtility.CreateOrLoadScriptableObject<StageData>(stagePath);
    }

    void Select(Button button)
    {
        // 버튼이 부모에서 몇 번째 자식인지 확인
        selectIndex = button.transform.GetSiblingIndex();
        Debug.Log($"버튼 {button.name}은 부모의 {selectIndex} 번째 자식입니다.");
    }

    void Apply(Button button)
    {
        int buttonIndx = button.transform.GetSiblingIndex();
        switch (selectIndex)
        {
            //타일
            case 0:
            case 1:
            case 2:
            case 3:
                TileApply(buttonIndx, selectIndex);
                break;

            case 4:
                break;

            case 5:
                break;

            case 6:
                TileRotate(buttonIndx);
                break;

            case 7:
                TileCantRotate(buttonIndx);
                break;

            //지우개(10번타일,회전불가능,회전x)
            case 8:
                Erase(buttonIndx);
                break;

            default:
                Debug.Log("할당안됨");
                break;
        }
    }

    void TileApply(int tileIdx, int type)
    {
        if (stageData.tileInfos[tileIdx] == null)
        {
            stageData.tileInfos[tileIdx] = new TileInfo();
        }

        stageData.tileInfos[tileIdx].tileNum = type;
        mapButtons[tileIdx].GetComponent<Image>().sprite = objectButtons[type].GetComponent<Image>().sprite;

        if (stageData.tileInfos[tileIdx].isRotate)
        {
            mapButtons[tileIdx].transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        if (stageData.tileInfos[tileIdx].cantRotate)
        {
            mapButtons[tileIdx].GetComponent<Image>().color = new Color(190f / 255f, 190f / 255f, 190f / 255f, 1f);
        }
    }

    void TileRotate(int tileIdx)
    {
        if (stageData.tileInfos[tileIdx] == null) return;
        if (stageData.tileInfos[tileIdx].tileNum == 10) return;

        if (stageData.tileInfos[tileIdx].isRotate)
        {
            stageData.tileInfos[tileIdx].isRotate = false;
            mapButtons[tileIdx].transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            stageData.tileInfos[tileIdx].isRotate = true;
            mapButtons[tileIdx].transform.localEulerAngles = new Vector3(0, 0, (stageData.tileInfos[tileIdx].tileNum != 1) ? 180f : 60f);
        }
    }

    void SetPlayer(int tileIdx)
    {
        if (stageData.tileInfos[tileIdx] == null) return;
        if (stageData.tileInfos[tileIdx].tileNum == 10) return;

        (int,int) loc = (tileIdx % 6, tileIdx / 6);

        //PlayerInfo player = new PlayerInfo();
        //player.loc = (tileIdx % 6, tileIdx / 6);

        for (int i = 0; i < stageData.playerInfos.Length; i++)
        {
            if (stageData.playerInfos[i] != null && stageData.playerInfos[i].loc == loc) return;
            if (stageData.candyInfos[i] != null && stageData.candyInfos[i].loc == loc) return;
        } 

        for (int i = 0; i < stageData.playerInfos.Length; i++)
        {
            if (stageData.playerInfos[i] != null)
            {
                stageData.playerInfos[i] = new PlayerInfo();
                stageData.playerInfos[i].loc = loc;
                
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
        if (stageData.tileInfos[tileIdx] == null)
        {
            stageData.tileInfos[tileIdx] = new TileInfo();
        }

        stageData.tileInfos[tileIdx].tileNum = 10;
        mapButtons[tileIdx].GetComponent<Image>().sprite = objectButtons[8].GetComponent<Image>().sprite;

        stageData.tileInfos[tileIdx].isRotate = false;
        stageData.tileInfos[tileIdx].cantRotate = true;
        stageData.tileInfos[tileIdx].color = 0;
    }
}
