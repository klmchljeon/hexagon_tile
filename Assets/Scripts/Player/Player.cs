using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int color;
    public (int, int) playerIndex;

    public event Action<Vector3, Vector3, int> playerMove;

    private void Awake()
    {
        GameManager.Instance.moveStart += MoveStart;
    }
    private void OnDestroy()
    {
        GameManager.Instance.moveStart -= MoveStart;
    }

    void MoveStart((int,int) start, (int,int) end)
    {
        if (start != playerIndex)
            return;

        playerIndex = end;

        Tile curTile = GameManager.Instance.tileList[start.Item1, start.Item2].GetComponent<Tile>();
        Tile nextTile = GameManager.Instance.tileList[end.Item1, end.Item2].GetComponent<Tile>();

        int idx = -1;
        for (int i = 0; i < curTile.adjacentIdx.Length; i++)
        {
            if (end == curTile.adjacentIdx[i])
            {
                idx = i;
                break;
            }
        }
        if (idx == -1) return;


        Vector3 StartPos = transform.position;
        Vector3 EndPos = nextTile.transform.position + (Vector3)nextTile.objectPosition;
        int cost = curTile.costs[idx];

        playerMove?.Invoke(StartPos, EndPos, cost);
    }
}
