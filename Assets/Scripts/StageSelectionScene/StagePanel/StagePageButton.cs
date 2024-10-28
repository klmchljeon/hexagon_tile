using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePageButton : MonoBehaviour
{
    [SerializeField] public int delta;
    [SerializeField] private GameObject BoundaryPage;

    // Start is called before the first frame update
    void Awake()
    {
        BoundaryPage.GetComponent<UIActiveStateMonitor>().AddListener(Disable);
        Disable(BoundaryPage.activeSelf);
    }

    private void OnDestroy()
    {
        BoundaryPage.GetComponent<UIActiveStateMonitor>().RemoveListener(Disable);
    }

    void Disable(bool isActive)
    {
        gameObject.SetActive(!isActive);
    }
}
