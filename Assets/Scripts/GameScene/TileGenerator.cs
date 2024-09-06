using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    public GameObject[] tilePrefabs;

    public GameObject tileParent;

    float tileWidth = 0f;
    float tileHeight = 0f;

    GameObject[,] tileList = new GameObject[6,6];

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer spriteRenderer = tilePrefabs[0].GetComponent<SpriteRenderer>();
        Vector2 spriteSize = spriteRenderer.bounds.size;

        tileWidth = spriteSize.x - 0.01f;
        tileHeight = spriteSize.y;

        Debug.Log(tileWidth);

        SetupStage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetupStage()
    {
        StageData stageData = StageManager.Instance.currentStageData;

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

                    tileObject.transform.SetParent(tileParent.transform);


                    tileList[x,y] = tileObject;
                }
            }
        }
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
