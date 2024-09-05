using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    enum directions
    {
        topLeft, topRight, bottomLeft, bottomRight
    }

    Tile[] adjacentTiles = new Tile[4];
    int[] costs = new int[4];

    bool isRotate = false;

    public event Action<bool> isRotateChanged;

    void Rotate()
    {
        isRotate ^= true;
        isRotateChanged?.Invoke(isRotate);
        Update();

    }

    private void Start()
    {
        Update();
    }

    void Update()
    {
        for (int i = 0; i < adjacentTiles.Length; i++)
        {
            if (adjacentTiles[i] == null) continue;

            costs[i] = Calculate(adjacentTiles[i], i < 2);
        }
    }

    int Calculate(Tile adjTile, bool isUpper)
    {
        if (isRotate == adjTile.isRotate)
            return 2;

        if (isRotate != isUpper)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
}
