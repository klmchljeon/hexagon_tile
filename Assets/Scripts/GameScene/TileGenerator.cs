using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private StageData TestStage;

    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private GameObject[] playerPrefabs;
    [SerializeField] private GameObject[] candyPrefabs;

    [SerializeField] private GameObject tileParent;
    [SerializeField] private GameObject playerParent;
    [SerializeField] private GameObject candyParent;

    [SerializeField] private Vector3 offset;

    private float tileWidth;
    private float tileHeight;

    public GameObject[,] tileList = new GameObject[6, 6];
    public GameObject[] playerList = new GameObject[5];
    public GameObject[] candyList = new GameObject[5];
    public Vector3Int starCount;
    public int actionPoint;
    public int stageNum;

    public event Action isLoaded;

    void Start()
    {
        CalculateTileBounds();
        SetupStage();
        isLoaded?.Invoke();
    }

    void SetupStage()
    {
        StageData stageData;
        if (StageManager.Instance == null)
        {
            stageData = TestStage;
        }
        else
        {
            stageData = StageManager.Instance.currentStageData;
        }

        if (stageData != null)
        {
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 6; x++)
                {
                    Vector3 position = CalculatePosition(x, y, offset); // 타일의 위치 설정
                    int index = x + y * 6;
                    TileInfo tileInfo = stageData.tileInfos[index];

                    GameObject tileObject = Instantiate(tilePrefabs[tileInfo.tileNum], position, Quaternion.identity);
                    tileObject.GetComponent<Tile>().loc = (x, y);

                    if (tileInfo.isRotate)
                    {
                        tileObject.GetComponent<Tile>().FirstRotate();
                    }

                    if (tileInfo.cantRotate)
                    {
                        tileObject.GetComponent<Tile>().cantRotate = true;
                        tileObject.GetComponent<SpriteRenderer>().color = new Color(190f / 255f, 190f / 255f, 190f / 255f, 1f);
                    }

                    tileObject.transform.SetParent(tileParent.transform);
                    tileObject.name = $"T{x}{y}";

                    tileList[x,y] = tileObject;
                }
            }

            for (int i = 0; i < 5; i++)
            {
                PlayerInfo playerInfo = stageData.playerInfos[i];
                if (playerInfo.loc == new Vector2Int(-1,-1)) continue;

                Vector2Int loc = playerInfo.loc;
                GameObject playerTile = tileList[loc.x, loc.y];
                GameObject playerObject = Instantiate(playerPrefabs[playerInfo.color]);

                Vector3 playerPosition = (Vector3)(playerTile.GetComponent<Tile>().objectPosition);
                playerObject.GetComponent<Player>().playerIndex = (loc.x, loc.y);
                playerObject.GetComponent<Player>().color = playerInfo.color;

                tileList[loc.x, loc.y].GetComponent<Tile>().onTileObject = i;

                playerObject.transform.position = playerTile.transform.position + playerPosition;
                playerObject.transform.SetParent(playerParent.transform, true);
                playerObject.name = $"P{i}";

                playerList[i] = playerObject;

            }

            for (int i = 0; i < 5; i++)
            {
                CandyInfo candyInfo = stageData.candyInfos[i];
                if (candyInfo.loc == new Vector2Int(-1,-1)) continue;

                Vector2Int loc = candyInfo.loc;
                GameObject candyTile = tileList[loc.x, loc.y];
                GameObject candyObject = Instantiate(candyPrefabs[candyInfo.color]);

                tileList[loc.x, loc.y].GetComponent<Tile>().onTileCandy = i;

                Vector3 candyPosition = (Vector3)(candyTile.GetComponent<Tile>().objectPosition);
                candyObject.GetComponent<Candy>().candyIndex = (loc.x,loc.y);
                candyObject.GetComponent<Candy>().color = candyInfo.color;
                candyObject.GetComponent<Candy>().isCatch = false;

                candyObject.transform.position = candyTile.transform.position + candyPosition;
                candyObject.transform.SetParent(candyParent.transform, true);
                candyObject.name = $"C{i}";

                candyList[i] = candyObject;
            }

            int[,] dx = { { -1, 0 }, { 0, 1 }, { -1, 0 }, { 0, 1 }};
            int[] dy = { 1, 1, -1, -1 };

            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    if (tileList[x,y] == null) continue;
                    Tile curTile = tileList[x, y].GetComponent<Tile>();

                    for (int i = 0; i < 4; i++)
                    {
                        int nx = x + dx[i, y % 2];
                        int ny = y + dy[i];

                        if (CheckRange(nx,ny))
                        {
                            if (tileList[nx, ny] == null)
                            {
                                Debug.Log((x, y,-1));
                                continue;
                            }
                            
                            curTile.adjacentTiles[i] = tileList[nx,ny].GetComponent<Tile>();
                            curTile.adjacentIdx[i] = (nx, ny);
                        }
                    }
                    curTile.UpdateCost();
                }
            }
            starCount = stageData.starCount;
            actionPoint = stageData.actionPoint;
            stageNum = stageData.stageNumber;
        }
    }

    bool CheckRange(int x, int y)
    {
        return 0 <= x && x < 6 && 0 <= y && y < 6;
    }

    void CalculateTileBounds()
    {
        SpriteRenderer spriteRenderer = tilePrefabs[0].GetComponent<SpriteRenderer>();
        Vector2 spriteSize = spriteRenderer.bounds.size;

        tileWidth = spriteSize.x - 0.01f;
        tileHeight = spriteSize.y;
    }

    Vector3 CalculatePosition(int x, int y, Vector3 offset)
    {
        Vector3 res = new Vector3(-3 * tileWidth, -2.5f * tileHeight * 0.74f, 0);

        if (y%2 == 0)
        {
            res += new Vector3(x * tileWidth, y * tileHeight * 0.74f, 0);
        }
        else
        {
            res += new Vector3((x+0.5f) * tileWidth, y * tileHeight * 0.74f, 0);
        }

        return res + offset;
    }
}
