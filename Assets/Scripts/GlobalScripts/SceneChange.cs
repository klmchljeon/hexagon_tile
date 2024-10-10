using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    [SerializeField]
    protected string sceneName;

    public Image fadeImage; // 검은 배경이 될 Image (UI 요소)
    public float fadeDuration = 0.5f; // 페이드 지속 시간

    protected virtual void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(TransitionToScene);
    }
    protected virtual void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveListener(TransitionToScene);
    }


    protected virtual void Start()
    {
        // 시작할 때 검은 배경을 비활성화 (투명하게 설정)
        //fadeImage.color = new Color(0, 0, 0, 0);
    }

    // 씬 전환을 실행하는 함수
    protected virtual void TransitionToScene()
    {
        StartCoroutine(FadeOutAndLoadScene());
    }

    // 코루틴: 씬을 페이드 아웃한 후 로드
    protected IEnumerator FadeOutAndLoadScene()
    {
        // 검은 배경 활성화
        fadeImage.gameObject.SetActive(true);

        // 페이드 아웃(화면을 검게 만듦)
        yield return StartCoroutine(Fade(1.0f));

        // 비동기 씬 전환 시작
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // 비동기 로딩이 완료될 때까지 대기
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                // 씬 로딩 완료되면 allowSceneActivation을 true로 설정해 씬을 전환
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    protected IEnumerator FadeInAndEnterScene()
    {
        // 검은 배경 활성화
        fadeImage.color = new Color(0, 0, 0, 1);
        fadeImage.gameObject.SetActive(true);

        // 페이드 아웃(화면을 투명하게 만듦)
        yield return StartCoroutine(Fade(0.0f));

        fadeImage.gameObject.SetActive(false);
    }

    // 페이드 효과를 적용하는 코루틴
    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a; // 현재 알파값
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha); // 검은 배경의 투명도 조정
            yield return null;
        }

        // 최종적으로 알파값을 정확히 맞춰줌
        fadeImage.color = new Color(0, 0, 0, targetAlpha);
    }
}
