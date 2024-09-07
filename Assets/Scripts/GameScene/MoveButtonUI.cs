using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveButtonUI : MonoBehaviour
{ 
    GameManager gameManager;

    public GameObject[] buttons;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        gameManager.UpdateUI += UpdateUI;
    }

    void UpdateUI(int x, int y)
    {
        //LeftTop, RightTop, LeftBottom, RightBottom, Left, Right. Center
        int[,] dx = { { -1, 0 }, { 0, 1 }, { -1, 0 }, { 0, 1 }, { -1, -1, }, { 1, 1 }, { 0, 0 } };
        int[] dy = { 1, 1, -1, -1, 0, 0, 0 };

        for (int i = 0; i < 7; i++)
        {
            int nx = x + dx[i, y%2];
            int ny = y + dy[i];

            if (CheckRange(nx, ny))
            {
                GameObject curTile = gameManager.tileList[nx, ny];
                buttons[i].transform.eulerAngles = new Vector3(0, 0, curTile.transform.eulerAngles.z);
                buttons[i].GetComponent<Image>().sprite = curTile.GetComponent<SpriteRenderer>().sprite;
            }
            else
            {
                buttons[i].SetActive(false);
            }
        }
    }

    bool CheckRange(int x, int y)
    {
        return 0 <= x && x < 6 && 0 <= y && y < 6;
    }
}
