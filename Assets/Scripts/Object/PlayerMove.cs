using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public bool moveFlag = false;
    public bool moveEnd = false;

    public float duration = 0.5f;  // 이동하는 데 걸리는 시간

    private float timeElapsed = 0f;

    public Vector3 StartPos;
    public Vector3 EndPos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!moveFlag) return;

        timeElapsed += Time.deltaTime;
        float t = Mathf.Clamp01(timeElapsed / duration);

        // 포물선 경로 계산
        Vector3 newPosition = GetParabolicPosition(StartPos, EndPos, t);
        transform.position = newPosition;

        if (timeElapsed > duration)
        {
            timeElapsed = 0f;
            moveFlag = false;
            moveEnd = true;
        }
    }

    Vector3 GetParabolicPosition(Vector3 start, Vector3 end, float t)
    {
        float height = Mathf.Max(start.y, end.y) - Mathf.Min(start.y, end.y) + 1f;

        // 시작점과 끝점 사이를 선형 보간 (x축과 z축은 선형 이동)
        Vector3 linearPosition = Vector3.Lerp(start, end, t);

        // 포물선 모양을 위한 추가적인 y축 변위 계산
        float parabolicT = t * (1 - t); // 포물선의 기본 곡선 (0 ~ 0.25 범위)

        // 시작점과 끝점 사이의 y값 차이 계산
        float yDifference = end.y - start.y;

        // 포물선 곡선을 이용하여 y축 변위를 계산하고 적용 (시작점, 끝점의 높이를 고려)
        float newY = start.y + yDifference * t + height * parabolicT;

        // 최종 포지션 계산
        return new Vector3(linearPosition.x, newY, linearPosition.z);
    }
}
