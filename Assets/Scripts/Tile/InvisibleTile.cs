using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleTile : Tile
{
    [SerializeField]
    private int _childTileNum = 10;
    public override int tileNum
    {
        get { return _childTileNum; }
        set { _childTileNum = value; }
    }

    public override int Calculate(Tile adjTile, int index)
    {
        return -1;
    }
}
