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

    public Tile[] adjacentTiles = new Tile[4];
    public int[] costs = new int[4];

    public bool isRotate = false;

    public Vector2 objectPosition;

    public event Action<bool> isRotateChanged;
    
    public void Rotate()
    {
        isRotate ^= true;
        objectPosition = -objectPosition;
        isRotateChanged?.Invoke(isRotate);
        UpdateCost();
    }

    private void Start()
    {
        UpdateCost();
    }

    public void UpdateCost()
    {
        for (int i = 0; i < adjacentTiles.Length; i++)
        {
            if (adjacentTiles[i] == null) continue;

            costs[i] = Calculate(adjacentTiles[i], i < 2);
            adjacentTiles[i].costs[3 - i] = adjacentTiles[i].Calculate(this, 3 - i < 2);
        }
    }

    public int Calculate(Tile adjTile, bool isUpper)
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
