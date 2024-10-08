using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneUIManager : UIManager
{
    protected override void PerformAlternativeAction()
    {
        Debug.Log("활성화된 UI 오브젝트가 없습니다. 다른 동작을 수행합니다.");
        pause.GetComponent<UIActiveStateMonitor>().SetUIActive(true);
    }
}
