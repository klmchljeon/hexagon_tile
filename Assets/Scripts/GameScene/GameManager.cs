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

    public int playerCount = 0;
    public int candyCount = 0;

    //event
    public event Action UpdateUI;
    public event Action<(int, int), bool> TileRotate;
    public event Action<(int, int)> TileClick;
    public event Action<(int, int), (int, int), bool> moveStart;
    public event Action<(int, int)> CheckCandy;

    //status
    public bool isMoving = false;
    public bool isRotating = false;

    //touch
    private Vector3 touchDownPosition = Vector3.forward;
    private Vector3 nullPosition = Vector3.forward;

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
        stageNum = tileGen.stageNum;
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
        Debug.Log("이동 시작");
        isMoving = true;

        if (!isUndo)
            actionStack.Push(new PuzzleActionData(ActionType.Move, loc, loc2));

        moveStart?.Invoke(loc, loc2, isUndo);
    }

    void MoveComplete((int, int) loc, int cost, bool isUndo)
    {
        Debug.Log("이동 완료");
        isMoving = false;
        actionPoint -= cost;

        CheckCandy?.Invoke(loc);
        UpdateUI?.Invoke();
        if (!GameEndCheck() && !isUndo)
            TileClick(loc);
    }

    void RotateStart((int, int) loc, bool isUndo)
    {
        Debug.Log("회전 시작");
        isRotating = true;
        TileRotate?.Invoke(loc, isUndo);

        if (!isUndo)
            actionStack.Push(new PuzzleActionData(ActionType.Rotate, loc));
        
    }

    void RotateComplete((int, int) loc, int cost)
    {
        Debug.Log("회전 완료");
        isRotating = false;
        actionPoint -= cost;

        UpdateUI?.Invoke();
        GameEndCheck();
    }

    void Update()
    {

        if (!isMoving && !isRotating)
        {
            SelectTile();
        }
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


    //언젠가 수정하겠지
    public void ResetClick()
    {
        TileClick?.Invoke((-1, -1));
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
                    touchDownPosition = nullPosition;
                    ResetClick();
                    return;
                }

                touchDownPosition = Input.mousePosition;
                return;
            }
            else
            {
                touchDownPosition = nullPosition;
                ResetClick();
            }
        }

        if (Input.GetMouseButtonUp(0) && !layerMask.activeSelf)
        {
            if (touchDownPosition == nullPosition || Vector3.Distance(touchDownPosition, Input.mousePosition) > 100f)
            {
                touchDownPosition = nullPosition;
                ResetClick();
                return;
            }

            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 1f);


            touchDownPosition = nullPosition;

            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;

                if (clickedObject.name[0] != 'T')
                {
                    ResetClick();
                    return;
                }

                int x = Int32.Parse(clickedObject.name[1].ToString());
                int y = Int32.Parse(clickedObject.name[2].ToString());

                (int, int) tileIndex = (x, y);
                TileClick.Invoke(tileIndex);
            }
            else
            {
                ResetClick();
            }
        }
    }
}
