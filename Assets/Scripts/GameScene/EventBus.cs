using System;

public static class EventBus
{
    public static event Action<(int,int),(int,int), bool> OnMoveStart;
    public static event Action<(int, int), int> OnMoveComplete;

    public static event Action<(int, int), bool> OnRotateStart;
    public static event Action<(int, int), int> OnRotateComplete;

    public static event Action<(int, int), bool> OnUndoEvent;

    public static void MoveStart((int,int) loc, (int,int) loc2, bool isUndo)
    {
        OnMoveStart?.Invoke(loc, loc2, isUndo);
    }

    public static void MoveComplete((int, int) loc, int cost)
    {
        OnMoveComplete?.Invoke(loc,cost);
    }

    public static void RotateStart((int, int) loc, bool isUndo)
    {
        OnRotateStart?.Invoke(loc, isUndo);
    }

    public static void RotateComplete((int, int) loc, int cost)
    {
        OnRotateComplete?.Invoke(loc, cost);
    }

    public static void UndoEvent((int, int) loc, bool isCatchCandy)
    {
        OnUndoEvent?.Invoke(loc,isCatchCandy);
    }
}