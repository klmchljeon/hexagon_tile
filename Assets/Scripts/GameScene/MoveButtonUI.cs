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
    }

    private void OnEnable()
    {
        gameManager.UpdateUI += UpdateUI;
    }

    private void OnDisable()
    {
        gameManager.UpdateUI -= UpdateUI;
    }

    void UpdateUI(int x, int y)
    {
        GameObject curTile = gameManager.tileList[x, y];

        //LeftTop, RightTop, LeftBottom, RightBottom, Left, Right. Center
        int[,] dx = { { -1, 0 }, { 0, 1 }, { -1, 0 }, { 0, 1 }, { -1, -1, }, { 1, 1 }, { 0, 0 } };
        int[] dy = { 1, 1, -1, -1, 0, 0, 0 };

        for (int i = 0; i < 7; i++)
        {
            int nx = x + dx[i, y%2];
            int ny = y + dy[i];

            if (CheckRange(nx, ny))
            {
                buttons[i].SetActive(true);
                GameObject nextTile = gameManager.tileList[nx, ny];
                buttons[i].transform.eulerAngles = new Vector3(0, 0, nextTile.transform.eulerAngles.z);
                buttons[i].GetComponent<Image>().sprite = nextTile.GetComponent<SpriteRenderer>().sprite;

                if (i >= 4) continue;

                int cost = curTile.GetComponent<Tile>().costs[i];

                if (cost == -1 || cost > gameManager.actionPoint)                
                {
                    buttons[i].GetComponent<Button>().interactable = false;
                }
                else
                {
                    buttons[i].GetComponent<Button>().interactable = true;
                }
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
