public class TileInfo
{
    public (int, int) loc;
    public int tileNum;
    public bool cantRotate;
    public bool isRotate;
    public bool color;

    public bool[] buttons = new bool[3];
}

public class PlayerInfo
{
    public (int, int) loc;
    public int color;
}
public class CandyInfo
{
    public (int, int) loc;
    public int color;
}
