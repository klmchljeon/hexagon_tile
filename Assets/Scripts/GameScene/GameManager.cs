using System;
using System.Collections;
using System.Collections.Generic;
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

    public GameObject player;

    public Button[] buttons;
    private bool firstUILoad = true;

    public event Action<int,int> UpdateUI;

    private void Awake()
    {
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => MovePlayer(btn));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (firstUILoad)
        {
            tileList = tileGen.tileList;
            playerPosition = tileGen.playerIndex;
            actionPoint = tileGen.actionPoint;
            player = FindObjectOfType<PlayerMove>().gameObject;

            UpdateUI?.Invoke((int)playerPosition.x, (int)playerPosition.y);
            firstUILoad = false;
        }

        if (player.GetComponent<PlayerMove>().moveEnd)
        {
            Debug.Log("이동 끝");
            UpdateUI?.Invoke((int)playerPosition.x, (int)playerPosition.y);
            player.GetComponent<PlayerMove>().moveEnd = false;
        }
    }

    void MovePlayer(Button button)
    {
        int idx = -1;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == button)
            {
                idx = i; break;
            }
        }

        if (idx == -1)
        {
            Debug.Log("fail");
            return;
        }

        PlayerMove playerMove = player.GetComponent<PlayerMove>();

        if (playerMove.moveFlag) 
        {
            Debug.Log("이동중");
            return;
        }

        int[,] dx = { { -1, 0 }, { 0, 1 }, { -1, 0 }, { 0, 1 } };
        int[] dy = { 1, 1, -1, -1 };

        int x = (int)playerPosition.x;
        int y = (int)playerPosition.y;

        int nx = x + dx[idx, y%2];
        int ny = y + dy[idx];
        
        playerPosition = new Vector2(nx, ny);

        Tile curTile = tileList[x, y].GetComponent<Tile>();
        Tile nextTile = tileList[nx, ny].GetComponent<Tile>();

        actionPoint -= curTile.costs[idx];

        playerMove.StartPos = player.transform.position;
        playerMove.EndPos = nextTile.transform.position + (Vector3)nextTile.objectPosition;

        playerMove.moveFlag = true;

    }
}
