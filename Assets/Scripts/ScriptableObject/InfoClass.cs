public class TileInfo
{
    //public (int, int) loc;
    public int tileNum = 0;
    public bool cantRotate = false;
    public bool isRotate = false;
    public int color = 0;

    public bool[] buttons = new bool[3];
}

public class PlayerInfo
{
    public (int, int) loc = (-1,-1);
    public int color = 0;
}
public class CandyInfo
{
    public (int, int) loc = (-1,-1);
    public int color = 0;
}
