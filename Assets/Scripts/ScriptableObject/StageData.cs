using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewStageData", menuName = "Stage/StageData")]
public class StageData : ScriptableObject
{
    public int stageNumber;

    public int height;
    public int width;

    public int actionPoint;

    public TileInfo[] tileInfos = new TileInfo[36];

    public PlayerInfo[] playerInfos = new PlayerInfo[5];
    public CandyInfo[] candyInfos = new CandyInfo[5];
}
