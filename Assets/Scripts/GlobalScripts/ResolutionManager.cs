using UnityEngine;

public class CameraResolutionAdjuster : MonoBehaviour
{
    public int targetWidth = 1920;  // 기준 해상도의 가로
    public int targetHeight = 1080; // 기준 해상도의 세로
    public Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        AdjustCameraSize();
    }

    void AdjustCameraSize()
    {
        float targetAspect = (float)targetWidth / targetHeight;
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        // 화면 비율에 맞춰 orthographicSize 조정
        if (mainCamera.orthographic)
        {
            // 카메라의 orthographic size를 스케일 비율에 따라 조정
            if (scaleHeight < 1.0f)
            {
                // 현재 화면이 타겟보다 세로로 긴 경우 (위아래 블랙박스)
                mainCamera.orthographicSize = targetHeight / 200.0f; // 기본 사이즈 설정
            }
            else
            {
                // 현재 화면이 타겟보다 가로로 긴 경우 (좌우 블랙박스)
                mainCamera.orthographicSize = (targetHeight / 200.0f) * scaleHeight;
            }
        }

        // 블랙박스를 처리하기 위해 카메라 뷰포트 조정
        if (scaleHeight < 1.0f)
        {
            // 위아래 블랙박스
            Rect rect = mainCamera.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            mainCamera.rect = rect;
        }
        else
        {
            // 좌우 블랙박스
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = mainCamera.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            mainCamera.rect = rect;
        }
    }

    void OnPreCull()
    {
        // 카메라 영역 밖에 블랙박스를 그려서 빈 공간 처리
        GL.Clear(true, true, Color.black);
    }
}
