using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public TileGenerator tileGen;

    public GameObject[,] tileList;
    public Vector2 playerPosition;
    public Vector2 goalPosition;
    public int actionPoint;
    public int candyCount = 1;

    public GameObject player;
    public GameObject goal;

    public Button[] buttons;
    private bool firstUILoad = true;

    public event Action<int,int> UpdateUI;

    private Camera mainCamera; 
    private GameObject selectedObject = null; // 현재 선택된 오브젝트를 추적
    private GameObject[] adjactTileList = new GameObject[4];

    private string selectedObjectType = null;

    private Vector3 originalScale; // 오브젝트의 원래 크기 저장
    private TileRotate rotatingTile;

    public GameObject panel;
    public GameObject layerMask;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (firstUILoad)
        {
            tileList = tileGen.tileList;
            playerPosition = tileGen.playerIndex;
            goalPosition = tileGen.goalIndex;

            actionPoint = tileGen.actionPoint;
            player = FindObjectOfType<PlayerMove>().gameObject;
            goal = FindObjectOfType<Goal>().gameObject;

            UpdateUI?.Invoke((int)playerPosition.x, (int)playerPosition.y);
            firstUILoad = false;
        }

        if (player.GetComponent<PlayerMove>().moveEnd)
        {
            if (goalPosition == playerPosition)
            {
                candyCount--;
            }

            UpdateUI?.Invoke((int)playerPosition.x, (int)playerPosition.y);
            player.GetComponent<PlayerMove>().moveEnd = false;

            SoundManager.instance.PlaySound(GetComponent<AudioSource>().clip, GetComponent<AudioSource>(), true);

            GameEndCheck();
            Debug.Log($"이동 끝 포인트: {actionPoint}");
        }

        if (rotatingTile != null && rotatingTile.RotateEnd)
        {
            //Debug.Log("회전 끝");
            UpdateUI?.Invoke((int)playerPosition.x, (int)playerPosition.y);
            rotatingTile.RotateEnd = false;
            rotatingTile = null;

            GameEndCheck();
            Debug.Log($"회전 끝 포인트: {actionPoint}");
        }
        
        if (!player.GetComponent<PlayerMove>().moveFlag && rotatingTile == null)
        {
            SelectTile();
        }
    }

    void GameEndCheck()
    {

        if (playerPosition == goalPosition)
        {
            panel.SetActive(true);
            layerMask.SetActive(true);

            GameObject clear = panel.transform.Find("Clear").gameObject;

            goalPosition = new Vector2(-1,-1);
            Clear(clear);
        }
        else if (actionPoint == 0)
        {
            panel.SetActive(true);
            layerMask.SetActive(true);

            GameObject fail = panel.transform.Find("Fail").gameObject;

            GameOver(fail);
        }
    }

    void Clear(GameObject clear)
    {
        //사탕 획득 모션
        goal.GetComponent<FloatingItem>().isCollected = true;

        clear.SetActive(true);
    }

    void GameOver(GameObject fail)
    {
        fail.SetActive(true);
    }

    void MovePlayer(GameObject moveTile, Vector2 nextPosition)
    {
        int idx = -1;
        for (int i = 0; i < adjactTileList.Length; i++)
        {
            if (adjactTileList[i] == moveTile)
            {
                idx = i;
                break;
            }
        }

        if (idx == -1)
        {
            return;
        }

        PlayerMove playerMove = player.GetComponent<PlayerMove>();

        if (playerMove.moveFlag) 
        {
            //Debug.Log("이동중");
            return;
        }

        int x = (int)playerPosition.x;
        int y = (int)playerPosition.y;

        int nx = (int)nextPosition.x;
        int ny = (int)nextPosition.y;
        
        playerPosition = new Vector2(nx, ny);

        Tile curTile = tileList[x, y].GetComponent<Tile>();
        Tile nextTile = tileList[nx, ny].GetComponent<Tile>();

        actionPoint -= curTile.costs[idx];

        playerMove.StartPos = player.transform.position;
        playerMove.EndPos = nextTile.transform.position + (Vector3)nextTile.objectPosition;

        playerMove.moveFlag = true;

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
                    ResetObjectSelection();
                    return;
                }

                int x = Int32.Parse(clickedObject.name[1].ToString());
                int y = Int32.Parse(clickedObject.name[2].ToString());

                Vector2 tileIndex = new Vector2(x, y);
                
                // 인접한 타일을 누른 경우
                if (adjactTileList.Contains(clickedObject))
                {
                    MovePlayer(clickedObject, tileIndex);
                    ResetObjectSelection();
                    return;
                }
                
                if (tileIndex == goalPosition)
                {
                    ResetObjectSelection();
                    return;
                }


                // 이미 선택된 오브젝트를 다시 선택한 경우
                if (selectedObject == clickedObject)
                {
                    if (selectedObjectType == "Tile")
                    {
                        PerformSecondaryActionTile(clickedObject); // 다시 선택 시 동작 수행
                    }
                }
                else
                {
                    // 새로운 오브젝트 선택 시
                    if (selectedObjectType != null)
                    {
                        ResetObjectSelection(); // 이전에 선택된 오브젝트의 크기를 원래대로 복구
                    }

                    SelectObject(clickedObject, tileIndex); // 새로운 오브젝트 선택
                }
            }
            else
            {
                ResetObjectSelection();
            }
        }
    }

    void SelectObject(GameObject obj, Vector2 tileIndex)
    {
        if (tileIndex == playerPosition)
        {
            selectedObject = player;
            selectedObjectType = "Player";

            Tile tile = obj.GetComponent<Tile>();
            for (int i = 0; i < adjactTileList.Length; i++)
            {
                if (tile.costs[i] == -1) continue;

                adjactTileList[i] = tile.adjacentTiles[i].gameObject;
                tile.adjacentTiles[i].MovableSelect();
                Debug.Log("선택");
            }
        }
        else
        {
            if (obj.GetComponent<Tile>().cantRotate)
            {
                Debug.Log("회전 불가능");
                return;
            }

            selectedObject = obj;
            selectedObjectType = "Tile";

            originalScale = obj.transform.localScale; // 원래 크기 저장
            obj.transform.localScale = originalScale * 1.1f; // 크기 1.5배로 확대
            obj.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }

    void PerformSecondaryActionTile(GameObject obj)
    {
        //Debug.Log("Object " + obj.name + " was selected again!");
        // 여기에 오브젝트를 다시 클릭했을 때 수행할 동작 추가
        obj.GetComponent<Tile>().Rotate();
        rotatingTile = obj.GetComponent<TileRotate>();
        rotatingTile.RotateEnd = false;
        actionPoint -= 1;

        ResetObjectSelection();
    }

    // 오브젝트의 크기를 원래대로 복구
    void ResetObjectSelection()
    {
        if (selectedObject != null)
        {
            if (selectedObjectType == "Tile")
            {
                selectedObject.transform.localScale = originalScale;
                selectedObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
            else if (selectedObjectType == "Player")
            {
                for (int i = 0; i < adjactTileList.Length; i++)
                {
                    if (adjactTileList[i] != null)
                        adjactTileList[i].GetComponent<Tile>().ResetSelect();
                    adjactTileList[i] = null;
                }
            }

            selectedObject = null;
            selectedObjectType = null;
        }
    }
}
