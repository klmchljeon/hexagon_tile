using UnityEngine;

[System.Serializable]
public class TileInfo
{
    //public (int, int) loc;
    public int tileNum = 10;
    public bool cantRotate = true;
    public bool isRotate = false;
    public int color = 0;

    public bool[] buttons = new bool[3];
}

[System.Serializable]
public class PlayerInfo
{
    public Vector2Int loc = new Vector2Int(-1,-1);
    public int color = 0;
}

[System.Serializable]
public class CandyInfo
{
    public Vector2Int loc = new Vector2Int(-1, -1);
    public int color = 0;
}
