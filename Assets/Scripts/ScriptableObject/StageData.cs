using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewStageData", menuName = "Stage/StageData")]
public class StageData : ScriptableObject
{
    public int stageNumber;
    public int[] tileNumbers = new int[36];
    public bool[] tileRotated = new bool[36];

    public int actionPoint;

    public Vector2 playerPosition;
    public Vector2 goalPosition;

    public Vector3[] itemPosition; //아이템 위치(x,y), 종류(z)
    
}
