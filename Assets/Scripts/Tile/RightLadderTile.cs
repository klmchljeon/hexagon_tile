using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightLadderTile : Tile
{
    [SerializeField]
    private int _childTileNum = 3;
    public override int tileNum
    {
        get { return _childTileNum; }
        set { _childTileNum = value; }
    }

    public override int Calculate(Tile adjTile, int index)
    {
        bool toUp = index < 2;
        //bool toLeft = index % 2 == 0;

        if (adjTile.tileNum == 0)
        {
            return adjTile.Calculate(this, 3 - index);
            {
                if (isRotate && index == 3)
                {
                    if (!adjTile.isRotate)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if (!isRotate && index == 0)
                {
                    if (adjTile.isRotate)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
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
            }
        }
        else if (adjTile.tileNum == 1)
        {
            return adjTile.Calculate(this, 3 - index);
            {
                if (isRotate && index == 3)
                {
                    if (!adjTile.isRotate)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if (!isRotate && index == 0)
                {
                    if (!adjTile.isRotate)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    if (toUp == isRotate)
                    {
                        return -1;
                    }
                    else if (!adjTile.isRotate == (index == 0 || index == 3))
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }
        else if (adjTile.tileNum == 2)
        {
            return adjTile.Calculate(this, 3 - index);
            {
                if (adjTile.isRotate && index == 2)
                {
                    return 1;
                }
                else if (!adjTile.isRotate && index == 1)
                {
                    return 1;
                }
                else if (isRotate == adjTile.isRotate)
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
        }
        else if (adjTile.tileNum == 3)
        {
            if (index == 0 || index == 3)
            {
                return 1;
            }
            else
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
        }

        Debug.Log($"{tileNum}, {adjTile.tileNum} 간 정의 필요");
        return -1;
    }
}
