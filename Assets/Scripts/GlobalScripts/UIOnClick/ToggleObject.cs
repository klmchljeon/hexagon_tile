using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    public GameObject targetObject; // 인스펙터에서 연결할 오브젝트

    public void ToggleActive()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }
}
