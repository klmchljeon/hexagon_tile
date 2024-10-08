using UnityEngine;
using UnityEngine.Events;

public class UIActiveStateMonitor : MonoBehaviour
{
    [SerializeField] private UnityEvent<bool> onActiveStateChanged;

    public void SetUIActive(bool isActive)
    {
        if (gameObject.activeSelf != isActive)
        {
            gameObject.SetActive(isActive);
            onActiveStateChanged?.Invoke(isActive);
        }
    }

    public void AddListener(UnityAction<bool> listener)
    {
        onActiveStateChanged.AddListener(listener);
    }

    public void RemoveListener(UnityAction<bool> listener)
    {
        onActiveStateChanged.RemoveListener(listener);
    }
}
