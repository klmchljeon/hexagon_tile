using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewStageResult", menuName = "Stage/StageResult")]
public class StageResult : ScriptableObject
{
    public int stageNumber;
    public bool[] star;
}
