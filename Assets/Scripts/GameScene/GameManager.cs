using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum ActionType
{
    Move,
    Rotate
}

public struct PuzzleActionData
{
    public ActionType ActionType;
    public (int, int) StartPos;
    public (int, int) EndPos;
    public bool CandyCatch;

    // 이동에 대한 생성자
    public PuzzleActionData(ActionType actionType, (int,int) startPos, (int,int) endPos)
    {
        ActionType = actionType;
        StartPos = startPos;
        EndPos = endPos;
        CandyCatch = false;
    }

    // 회전에 대한 생성자
    public PuzzleActionData(ActionType actionType, (int, int) idx)
    {
        ActionType = actionType;
        StartPos = idx;
        EndPos = (-1, -1);
        CandyCatch = false;
    }
}

public class GameManager : MonoBehaviour
{
    public Stack<PuzzleActionData> actionStack = new Stack<PuzzleActionData>();

    [SerializeField]
    private TileGenerator tileGen;

    //game info
    public GameObject[,] tileList = new GameObject[6, 6];
    public GameObject[] playerList = new GameObject[5];
    public GameObject[] candyList = new GameObject[5];
    public int actionPoint;
    public int stageNum;

    public int playerCount;
    public int candyCount;

    //event
    public event Action UpdateUI;
    public event Action<(int, int), bool> TileRotate;
    public event Action<(int, int)> TileClick;
    public event Action<(int, int), (int, int), bool> moveStart;

    //status
    public bool isMoving = false;
    public bool isRotating = false;

    public GameObject panel;
    public GameObject layerMask;

    Camera mainCamera;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        mainCamera = Camera.main;

        tileGen.isLoaded += FirstInfoUpdate;
        EventBus.OnMoveStart += MoveStart;
        EventBus.OnMoveComplete += MoveComplete;
        EventBus.OnRotateStart += RotateStart;
        EventBus.OnRotateComplete += RotateComplete;
    }

    private void OnDestroy()
    {
        tileGen.isLoaded -= FirstInfoUpdate;
        EventBus.OnMoveStart -= MoveStart;
        EventBus.OnMoveComplete -= MoveComplete;
        EventBus.OnRotateStart -= RotateStart;
        EventBus.OnRotateComplete -= RotateComplete;
    }

    void FirstInfoUpdate()
    {
        stageNum = tileGen.stageNum;
        tileList = tileGen.tileList;
        playerList = tileGen.playerList;
        candyList = tileGen.candyList;
        actionPoint = tileGen.actionPoint;
        for (int i = 0; i < 5; i++)
        {
            if (playerList[i] != null) playerCount++;
            if (candyList[i] != null) candyCount++;
        }

        UpdateUI?.Invoke();
        tileGen.isLoaded -= FirstInfoUpdate;
    }

    void MoveStart((int,int) loc, (int,int) loc2, bool isUndo)
    {
        isMoving = true;

        if (!isUndo)
            actionStack.Push(new PuzzleActionData(ActionType.Move, loc, loc2));

        moveStart?.Invoke(loc, loc2, isUndo);
    }

    void MoveComplete((int, int) loc, int cost)
    {
        isMoving = false;
        actionPoint -= cost;

        UpdateUI?.Invoke();
        GameEndCheck();
    }

    void RotateStart((int, int) loc, bool isUndo)
    {
        isRotating = true;
        TileRotate?.Invoke(loc, isUndo);

        if (!isUndo)
            actionStack.Push(new PuzzleActionData(ActionType.Rotate, loc));
        
    }

    void RotateComplete((int, int) loc, int cost)
    {
        isRotating = false;
        actionPoint -= cost;

        UpdateUI?.Invoke();
        if (!GameEndCheck())
            TileClick(loc);
    }

    void Update()
    {

        if (!isMoving && !isRotating)
        {
            SelectTile();
        }

        //if (player.GetComponent<PlayerMove>().moveEnd)
        /*{
            if (goalPosition == playerPosition)
            {
                candyCount--;
            }

            UpdateUI?.Invoke();
            player.GetComponent<PlayerMove>().moveEnd = false;

            SoundManager.instance.PlaySound(GetComponent<AudioSource>().clip, GetComponent<AudioSource>(), true);

            ReSelect(playerPosition);
            GameEndCheck();
            Debug.Log($"이동 끝 포인트: {actionPoint}");
        }*/
    }

    bool GameEndCheck()
    {
        bool flag = false;
        if (candyCount == 0)
        {
            panel.SetActive(true);
            layerMask.SetActive(true);

            GameObject clear = panel.transform.Find("Clear").gameObject;

            Clear(clear);
            flag = true;
        }
        else if (actionPoint == 0)
        {
            panel.SetActive(true);
            layerMask.SetActive(true);

            GameObject fail = panel.transform.Find("Fail").gameObject;

            GameOver(fail);
            flag = true;
        }

        return flag;
    }

    void Clear(GameObject clear)
    {
        //사탕 획득 모션
        //goal.GetComponent<FloatingItem>().isCollected = true;

        clear.SetActive(true);
    }

    void GameOver(GameObject fail)
    {
        fail.SetActive(true);
    }

    void SelectTile()
    {
        if (Input.GetMouseButtonDown(0) && !layerMask.activeSelf)
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 1f);

            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;

                if (clickedObject.name[0] != 'T') 
                {
                    TileClick?.Invoke((-1, -1));
                    return;
                }

                int x = Int32.Parse(clickedObject.name[1].ToString());
                int y = Int32.Parse(clickedObject.name[2].ToString());

                (int, int) tileIndex = (x, y);
                TileClick.Invoke(tileIndex);
            }
            else
            {
                TileClick?.Invoke((-1, -1));
            }
        }
    }
}
