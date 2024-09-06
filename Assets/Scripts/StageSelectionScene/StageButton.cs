using System;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    public StageData stageData; // ��ư�� �Ҵ��� StageData
    Button button;       // ����Ƽ�� ��ư ������Ʈ

    private void Start()
    {
        button = GetComponent<Button>();
        // ��ư Ŭ�� �� �̺�Ʈ �Ҵ�
        button.onClick.AddListener(OnStageButtonClick);
    }

    // ��ư�� Ŭ���Ǿ��� �� ����Ǵ� �Լ�
    void OnStageButtonClick()
    {
        // Ŭ���� ��ư�� StageData�� StageManager�� ����
        StageManager.Instance.currentStageData = stageData;

        // ���� ������ �̵�
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}
