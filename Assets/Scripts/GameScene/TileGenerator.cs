using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField]
    private StageData TestStage;

    public GameObject[] tilePrefabs;
    
    public GameObject candy;
    public Vector2 goalIndex;

    public GameObject player;
    public Vector2 playerIndex;

    public int actionPoint;

    public GameObject tileParent;

    float tileWidth = 0f;
    float tileHeight = 0f;

    public GameObject[,] tileList = new GameObject[6,6];

    private void Awake()
    {
        
    }

    // Start is called before the first frame update 
    void Start()
    {
        CalculateTileBounds();
        SetupStage();
    }

    // Update is called once per frame
    void Update()
    {
        
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
            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    Vector3 position = CalculatePosition(x, y); // 타일의 위치 설정
                    int index = x + y * 6;
                    //Debug.Log($"{x} {y} {stageData.tileRotated[index]}");
                    GameObject tileObject = Instantiate(tilePrefabs[stageData.tileNumbers[index]], position, Quaternion.identity);

                    if (stageData.tileRotated[index])
                    {
                        tileObject.GetComponent<Tile>().Rotate();
                    }

                    if (stageData.cantRotate[index])
                    {
                        tileObject.GetComponent<Tile>().cantRotate = true;
                        tileObject.GetComponent<SpriteRenderer>().color = new Color(190f / 255f, 190f / 255f, 190f / 255f, 1f);
                    }

                    tileObject.transform.SetParent(tileParent.transform);
                    tileObject.name = $"T{x}{y}";

                    tileList[x,y] = tileObject;
                }
            }

            goalIndex = new Vector2(stageData.goalPosition.x, stageData.goalPosition.y);

            GameObject goalTile = tileList[(int)goalIndex.x, (int)goalIndex.y];
            GameObject goalObject = Instantiate(candy);

            Vector3 goalPosition = (Vector3)(goalTile.GetComponent<Tile>().objectPosition);
            
            goalObject.transform.position = goalTile.transform.position + goalPosition;
            goalObject.GetComponent<FloatingItem>().startY = goalPosition.y;
            //

            playerIndex = new Vector2(stageData.playerPosition.x, stageData.playerPosition.y);

            GameObject playerTile = tileList[(int)playerIndex.x, (int)playerIndex.y];
            GameObject playerObject = Instantiate(player);

            Vector3 playerPosition = (Vector3)(playerTile.GetComponent<Tile>().objectPosition);

            playerObject.transform.position = playerTile.transform.position + playerPosition;

            int[,] dx = { { -1, 0 }, { 0, 1 }, { -1, 0 }, { 0, 1 }};
            int[] dy = { 1, 1, -1, -1 };

            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    Tile curTile = tileList[x, y].GetComponent<Tile>();

                    for (int i = 0; i < 4; i++)
                    {
                        int nx = x + dx[i, y % 2];
                        int ny = y + dy[i];

                        if (CheckRange(nx,ny))
                        {
                            curTile.adjacentTiles[i] = tileList[nx,ny].GetComponent<Tile>();
                        }
                    }

                    curTile.UpdateCost();
                }
            }

            actionPoint = stageData.actionPoint;
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

    Vector3 CalculatePosition(int x, int y)
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

        return res;
    }
}
