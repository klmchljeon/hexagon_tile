using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMoving : MonoBehaviour
{
    [SerializeField] private GameObject BG;
    [SerializeField] private GameObject layer;

    // 상수 : 이동 관련
    private const float DirectionForceReduceRate = 0.935f; // 감속비율
    public float DirectionForceMin; // 설정치 이하일 경우 움직임을 멈춤

    // 변수 : 이동 관련
    private bool _userMoveInput; // 현재 조작을 하고있는지 확인을 위한 변수
    private Vector3 _startPosition;  // 입력 시작 위치를 기억
    private Vector3 _directionForce; // 조작을 멈췄을때 서서히 감속하면서 이동 시키기 위한 변수

    private Vector2 bound = new Vector2(4.6f, 4.0f);

    // 컴포넌트
    private Camera _camera;

    // Start is called before the first frame update
    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (layer.activeSelf) return;

        // 카메라 포지션 이동
        ControlCameraPosition();

        // 조작을 멈췄을때 감속
        ReduceDirectionForce();

        // 카메라 위치 업데이트
        UpdateCameraPosition();
    }

    private void ControlCameraPosition()
    {
        var mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            CameraPositionMoveStart(mouseWorldPosition);
        }
        else if (Input.GetMouseButton(0))
        {
            CameraPositionMoveProgress(mouseWorldPosition);
        }
        else
        {
            CameraPositionMoveEnd();
        }
    }

    private void CameraPositionMoveStart(Vector3 startPosition)
    {
        _userMoveInput = true;
        _startPosition = startPosition;
        _directionForce = Vector2.zero;
    }

    private void CameraPositionMoveProgress(Vector3 targetPosition)
    {
        if (!_userMoveInput)
        {
            CameraPositionMoveStart(targetPosition);
            return;
        }

        _directionForce = _startPosition - targetPosition;
    }

    private void CameraPositionMoveEnd()
    {
        _userMoveInput = false;
    }

    private void ReduceDirectionForce()
    {
        // 조작 중일때는 아무것도 안함
        if (_userMoveInput)
        {
            return;
        }

        // 감속 수치 적용
        _directionForce *= DirectionForceReduceRate;

        // 작은 수치가 되면 강제로 멈춤
        if (_directionForce.magnitude < DirectionForceMin)
        {
            _directionForce = Vector3.zero;
        }
    }

    private void UpdateCameraPosition()
    {
        // 이동 수치가 없으면 아무것도 안함
        if (_directionForce == Vector3.zero)
        {
            return;
        }

        var currentPosition = transform.position;
        var targetPosition = currentPosition + _directionForce;
        targetPosition = new Vector3
            (
                Mathf.Max(Mathf.Min(targetPosition.x, bound.x), -bound.x),
                Mathf.Max(Mathf.Min(targetPosition.y, bound.y), -bound.y),
                targetPosition.z
            ) ;

        transform.position = Vector3.Lerp(currentPosition, targetPosition, 0.5f);
        BG.transform.position = Vector3.Lerp(currentPosition, targetPosition, 0.5f) / 2;
    }
}
