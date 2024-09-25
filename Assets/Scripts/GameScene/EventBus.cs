using System;

public static class EventBus
{
    public static event Action OnMoveStart;
    public static event Action OnMoveComplete;

    public static event Action OnRotateStart;
    public static event Action OnRotateComplete;

    public static void MoveStart()
    {
        OnMoveStart?.Invoke();
    }

    public static void MoveComplete()
    {
        OnMoveComplete?.Invoke();
    }

    public static void RotateStart()
    {
        OnRotateStart?.Invoke();
    }

    public static void RotateComplete()
    {
        OnMoveComplete?.Invoke();
    }
}