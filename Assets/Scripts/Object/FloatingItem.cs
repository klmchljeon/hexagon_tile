using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    public float floatAmplitude = 0.5f; // 아이템이 떠다니는 높이
    public float floatSpeed = 1f;       // 떠다니는 속도
    public float startY;               // 초기 y 좌표값

    void Start()
    {
        // 초기 y값 저장
        startY = transform.position.y;
    }

    void Update()
    {
        // y 좌표를 Sin 함수를 이용해 주기적으로 변동
        float newY = startY + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
