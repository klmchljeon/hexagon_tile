using System.Collections;
using UnityEngine;

public class ShakeEffect : MonoBehaviour
{
    public float shakeDuration = 0.2f;  // 진동 시간
    public float shakeMagnitude = 0.05f; // 진동 세기

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    public void Shake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            Vector3 randomPoint = initialPosition + (Random.insideUnitSphere * shakeMagnitude);
            transform.localPosition = new Vector3(randomPoint.x, randomPoint.y, initialPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = initialPosition; // 원래 위치로 복귀
    }

    //void OnMouseDown()
    //{
        // 클릭했을 때 진동 효과를 주는 트리거
    //    Shake();
    //}
}
