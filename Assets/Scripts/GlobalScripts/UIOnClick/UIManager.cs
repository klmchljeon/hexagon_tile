using System.Threading;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] protected GameObject pause;
    [SerializeField] GameObject layerMask;

    void Update()
    {
        // 안드로이드 뒤로가기 키(Escape)가 눌렸는지 확인
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 활성화된 UIActiveStateMonitor를 가진 오브젝트를 찾는다
            UIActiveStateMonitor[] monitors = FindObjectsOfType<UIActiveStateMonitor>();

            UIActiveStateMonitor p = null;
            int cnt = 0;
            // 활성화된 가장 앞에 있는 오브젝트를 찾기 위해 반복
            foreach (var monitor in monitors)
            {
                if (monitor.gameObject.activeInHierarchy)
                {
                    p = monitor;
                    cnt++;
                }
            }

            if (p == null)
            {
                // 만약 활성화된 UIActiveStateMonitor를 가진 오브젝트가 없다면 다른 동작 수행
                PerformAlternativeAction();
            }
            else
            {
                // 가장 앞에 있는 활성화된 오브젝트를 비활성화
                p.SetUIActive(false);
                if (cnt == 1 && layerMask != null) layerMask.SetActive(false);
            }
        }
    }

    // 활성화된 UI 오브젝트가 없을 때 수행할 동작
    protected virtual void PerformAlternativeAction()
    {
        Debug.Log("활성화된 UI 오브젝트가 없습니다. 다른 동작을 수행합니다.");
        pause.GetComponent<UIActiveStateMonitor>().SetUIActive(true);
        layerMask.SetActive(true);
    }
}
