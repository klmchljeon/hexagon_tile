using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private float _rotationAngle = 180f;
    protected virtual float rotationAngle
    {
        get { return _rotationAngle; }
        set { _rotationAngle = value; }
    }

    [SerializeField]
    private int _tileNum = 0;
    public virtual int tileNum
    {
        get { return _tileNum; }
        set { _tileNum = value; }
    }

    public GameObject MovableEffect;

    public Tile[] adjacentTiles = new Tile[4];
    public int[] costs = new int[4];

    public bool cantRotate = false;
    public bool isRotate = false;
    public event Action<float> isRotateChanged;

    public Vector2 objectPosition;

    public void FirstRotate()
    {
        isRotate = true;
        objectPosition = -objectPosition;
        transform.eulerAngles = new Vector3(0, 0, 360f - rotationAngle);
    }
    
    public void Rotate()
    {
        isRotate ^= true;
        objectPosition = -objectPosition;
        isRotateChanged?.Invoke(isRotate?(360f-rotationAngle):rotationAngle);
        UpdateCost();
    }

    private void Start()
    {
        UpdateCost();
    }

    private void Update()
    {

    }

    public void MovableSelect()
    {
        MovableEffect.SetActive(true);
    }
    public void ResetSelect()
    {
        MovableEffect.SetActive(false);
    }

    public void UpdateCost()
    {
        for (int i = 0; i < adjacentTiles.Length; i++)
        {
            if (adjacentTiles[i] == null) continue;

            costs[i] = Calculate(adjacentTiles[i], i);
            adjacentTiles[i].costs[3 - i] = adjacentTiles[i].Calculate(this, 3 - i);
        }
    }


    public virtual int Calculate(Tile adjTile, int index)
    {
        bool toUp = index < 2;
        //bool toLeft = index % 2 == 0;

        if (adjTile.tileNum == 0)
        {
            if (isRotate == adjTile.isRotate)
            {
                return 2;
            }
            else if (toUp != isRotate)
            {
                return 1;
            }
            else
            {
                return -1;
            }

        }
        else if (adjTile.tileNum == 1)
        {
            if (toUp == isRotate)
            {
                return -1;
            }
            else if (!adjTile.isRotate == (index == 1 || index == 2))
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        else if (adjTile.tileNum == 2)
        {
            if (isRotate != adjTile.isRotate)
            {
                if (toUp == isRotate)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            else if (adjTile.isRotate && index == 2)
            {
                return 1;
            }
            else if (!adjTile.isRotate && index == 1)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }
        else if (adjTile.tileNum == 3)
        {
            if (isRotate != adjTile.isRotate)
            {
                if (toUp == isRotate)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            else if (adjTile.isRotate && index == 3)
            {
                return 1;
            }
            else if (!adjTile.isRotate && index == 0)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        Debug.Log($"{tileNum}, {adjTile.tileNum} 간 정의 필요");
        return -1;
    }
}
