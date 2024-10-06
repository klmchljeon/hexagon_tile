using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRotate : MonoBehaviour
{
    int cost;

    public bool isRotating = false; // 현재 회전 중인지 확인하는 플래그
    private float rotationDuration = 1.0f; // 회전 애니메이션 지속 시간
    
    float startAngle;
    float endAngle;
    float elapsedTime;

    Tile tile;

    // Start is called before the first frame update
    void Awake()
    {
        tile = GetComponent<Tile>();
        tile.isRotateChanged += RotateAnimate;
    }

    private void OnDestroy()
    {
        tile.isRotateChanged -= RotateAnimate;
    }

    private void Update()
    {
        if (isRotating)
        {
            RotateTile();
        }
    }

    // 회전 애니메이션 실행
    public void RotateAnimate(float targetAngle, bool isUndo)
    {
        startAngle = transform.eulerAngles.z; // 현재 Z축의 시작 각도
        endAngle = startAngle + targetAngle*(isUndo?1:-1); // 목표 각도 계산
        Debug.Log((startAngle, endAngle));
        elapsedTime = 0f; // 경과 시간 초기화
        isRotating = true;

        cost = isUndo ? -tile.rotateCost : tile.rotateCost;
        //StartCoroutine(RotateTile(180));
    }

    private void RotateTile()
    {
        // 회전 애니메이션
        elapsedTime += Time.deltaTime; // 프레임당 경과 시간 증가
        float currentAngle = Mathf.Lerp(startAngle, endAngle, elapsedTime / rotationDuration); // 선형 보간을 사용하여 회전 각도 계산
        transform.eulerAngles = new Vector3(0, 0, currentAngle); // Z축 회전 적용

        if (elapsedTime > rotationDuration)
        {
            transform.eulerAngles = new Vector3(0, 0, endAngle%360); // 정확한 목표 각도로 설정
            isRotating = false; // 회전 완료
            EventBus.RotateComplete(tile.loc, cost);
        }
    }
}
