using UnityEngine;
using System.Collections;

public class UIShakeEffect : MonoBehaviour
{
    public RectTransform uiElement; // 흔들릴 UI 요소
    public float duration = 0.5f;   // 흔들리는 지속 시간
    public float magnitude = 10f;   // 흔들림의 세기

    public void Shake()
    {
        StartCoroutine(StartShake());
    }

    private IEnumerator StartShake()
    {
        Vector3 originalPosition = uiElement.anchoredPosition;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // 랜덤한 위치 계산
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;
            uiElement.anchoredPosition = originalPosition + new Vector3(offsetX, offsetY, 0);

            // 프레임마다 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // 원래 위치로 복원
        uiElement.anchoredPosition = originalPosition;
    }
}
