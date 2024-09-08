using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UphillTile : Tile
{
    [SerializeField]
    private float _childRotationAngle = 300f;
    protected override float rotationAngle
    {
        get { return _childRotationAngle; }
        set { _childRotationAngle = value; }
    }

    [SerializeField]
    private int _childTileNum = 1;
    public override int tileNum
    {
        get { return _childTileNum; }
        set { _childTileNum = value; }
    }


    public override int Calculate(Tile adjTile, int index)
    {
        bool toUp = index < 2;
        bool toLeft = index % 2 == 0;

        if (adjTile.tileNum == 0)
        {
            if (toUp != adjTile.isRotate)
            {
                return -1;
            }
            else if (!isRotate == (index == 1 || index == 2))
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
            if (isRotate != adjTile.isRotate)
            {
                return -1;
            }
            else if (!isRotate == (index == 1 || index == 2))
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        Debug.Log($"{tileNum}, {adjTile.tileNum} 간 정의 필요");
        return -1;
    }
}