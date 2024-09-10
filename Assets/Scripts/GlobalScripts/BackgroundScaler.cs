using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    public SpriteRenderer backgroundRenderer;

    void Start()
    {
        AdjustBackgroundSize();
    }

    void AdjustBackgroundSize()
    {
        // 카메라의 높이를 기준으로 배경 크기를 조정
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = backgroundRenderer.bounds.size.x / backgroundRenderer.bounds.size.y;

        if (screenRatio >= targetRatio)
        {
            // 화면이 더 넓은 경우, 세로를 기준으로 맞춤
            float scaleFactor = Screen.height / (backgroundRenderer.bounds.size.y * 100)*4;
            transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
        }
        else
        {
            // 화면이 더 좁은 경우, 가로를 기준으로 맞춤
            float scaleFactor = Screen.width / (backgroundRenderer.bounds.size.x * 100)*4;
            transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
        }
    }
}
