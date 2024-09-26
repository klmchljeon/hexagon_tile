using System;

public static class EventBus
{
    public static event Action<(int,int),(int,int)> OnMoveStart;
    public static event Action<(int, int), int> OnMoveComplete;

    public static event Action<(int, int)> OnRotateStart;
    public static event Action<(int, int), int> OnRotateComplete;

    public static void MoveStart((int,int) loc, (int,int) loc2)
    {
        OnMoveStart?.Invoke(loc,loc2);
    }

    public static void MoveComplete((int, int) loc, int cost)
    {
        OnMoveComplete?.Invoke(loc,cost);
    }

    public static void RotateStart((int, int) loc)
    {
        OnRotateStart?.Invoke(loc);
    }

    public static void RotateComplete((int, int) loc, int cost)
    {
        OnRotateComplete?.Invoke(loc, cost);
    }
}