using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TileGenerator tileGen;

    public GameObject[,] tileList;
    public Vector2 playerPosition;
    //public Vector2 goalPosition;

    private bool firstUILoad = true;

    public event Action<int,int> UpdateUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (firstUILoad)
        {
            tileList = tileGen.tileList;
            playerPosition = tileGen.playerIndex;

            UpdateUI?.Invoke((int)playerPosition.x, (int)playerPosition.y);
            Debug.Log(playerPosition);
            firstUILoad = false;
        }
    }
}
