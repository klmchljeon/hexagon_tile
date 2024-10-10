using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public int color;
    public (int, int) candyIndex;
    public bool isCatch;

    public event Action onCatch;

    private void Awake()
    {
        GameManager.Instance.CheckCandy += CatchCandy;
    }

    private void OnDestroy()
    {
        GameManager.Instance.CheckCandy -= CatchCandy;
    }

    void CatchCandy((int,int) loc)
    {
        if (loc != candyIndex || isCatch)
        {
            return;
        }

        for (int i = 0; i < GameManager.Instance.playerList.Length; i++)
        {
            if (GameManager.Instance.playerList[i] == null) continue;

            Player player = GameManager.Instance.playerList[i].GetComponent<Player>();
            if (player.playerIndex == candyIndex && player.color == color)
            {
                GameManager.Instance.candyCount--;
                StackLogChange();
                onCatch?.Invoke();
                SoundManager.instance.PlaySound(GetComponent<AudioSource>().clip, GetComponent<AudioSource>(), true, 0.15f);
            }
        }
    }
    void StackLogChange()
    {
        PuzzleActionData top = GameManager.Instance.actionStack.Pop();
        top.CandyCatch = true;
        GameManager.Instance.actionStack.Push(top);
    }
}