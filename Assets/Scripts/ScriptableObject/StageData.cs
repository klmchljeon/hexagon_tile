using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewStageData", menuName = "Stage/StageData")]
public class StageData : ScriptableObject
{
    public int stageNumber;
    public int[] tileNumbers = new int[36];
    public bool[] tileRotated = new bool[36];
}
