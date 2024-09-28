using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UndoButton : MonoBehaviour
{
    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Undo);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(Undo);
    }

    void Undo()
    {
        if (GameManager.Instance.isMoving || GameManager.Instance.isRotating)
            return;

        if (GameManager.Instance.actionStack.Count == 0)
            return;

        PuzzleActionData action = GameManager.Instance.actionStack.Pop();
        if (action.ActionType == ActionType.Move)
        {
            EventBus.MoveStart(action.EndPos, action.StartPos, true);
            if (action.CandyCatch)
            {
                EventBus.UndoEvent(action.EndPos, true);
            }
        }
        else
        {
            EventBus.RotateStart(action.StartPos, true);
        }
    }

}
