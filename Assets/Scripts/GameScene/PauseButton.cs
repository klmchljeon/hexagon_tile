using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private UIActiveStateMonitor pauseUI;
    [SerializeField] private Sprite pause;
    [SerializeField] private Sprite play;

    private Image child;

    // Start is called before the first frame update
    void Start()
    {
        pauseUI.AddListener(ToggleImage);
        child = transform.GetChild(0).GetComponentInChildren<Image>();
    }

    private void OnDestroy()
    {
        pauseUI.RemoveListener(ToggleImage);
    }

    void ToggleImage(bool isActive)
    {
        if (isActive)
        {
            child.sprite = play;
        }
        else
        {
            child.sprite = pause;
        }
    }
}
