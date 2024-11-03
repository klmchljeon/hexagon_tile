using TMPro;
using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    public float floatAmplitude = 0.5f; // 아이템이 떠다니는 높이
    public float floatSpeed = 1f;       // 떠다니는 속도
    public float startY;               // 초기 y 좌표값

    public bool isFloated = true;

    public float moveUpDistance = 1.0f; // 아이템이 위로 이동하는 거리
    public float duration = 1.0f; // 애니메이션 지속 시간
    public float fadeSpeed = 1.0f; // 페이드 아웃 속도

    private SpriteRenderer spriteRenderer; // 아이템의 스프라이트 렌더러 참조
    private Vector3 initialPosition; // 초기 위치 저장

    Color initialColor;

    // 목표 위치는 현재 위치에서 moveUpDistance만큼 위로
    Vector3 targetPosition;


    float elapsedTime = 0f;

    private void Awake()
    {
        GetComponent<Candy>().onCatch += CatchCandy;

        //수정 필요?
        EventBus.OnUndoEvent += UndoCandy;
    }

    private void OnDestroy()
    {
        GetComponent<Candy>().onCatch -= CatchCandy;

        //수정 필요?
        EventBus.OnUndoEvent -= UndoCandy;
    }

    void Start()
    {
        // 초기 y값 저장
        startY = transform.position.y;

        spriteRenderer = GetComponent<SpriteRenderer>();
        initialPosition = transform.position;

        initialColor = spriteRenderer.color;

        // 목표 위치는 현재 위치에서 moveUpDistance만큼 위로
        targetPosition = initialPosition + new Vector3(0, moveUpDistance, 0);

    }

    void Update()
    {
        if (isFloated)
        {
            // y 좌표를 Sin 함수를 이용해 주기적으로 변동
            float newY = startY + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
        else
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;

            // 아이템을 위로 이동
            transform.position = Vector3.Lerp(initialPosition, targetPosition, progress);

            // 아이템을 점차 투명하게 (알파 값 감소)
            Color newColor = initialColor;
            newColor.a = Mathf.Lerp(1, 0, progress); // 알파값을 1에서 0으로
            spriteRenderer.color = newColor;

            if (elapsedTime > duration)
            {
                newColor.a = 0;
                spriteRenderer.color = newColor;

                GetComponent<Candy>().isCatch = true;
                isFloated = true;
                elapsedTime = 0f;
                GameManager.Instance.isCatching = false;
            }
        }
    }

    void CatchCandy()
    {
        isFloated = false;
        GameManager.Instance.isCatching = true;
    }

    void UndoCandy((int,int) loc, bool isCatchCandy)
    {
        if (GetComponent<Candy>().candyIndex != loc || !isCatchCandy) 
        {
            return;
        }

        GetComponent<Candy>().isCatch = false;
        GameManager.Instance.candyCount++;
        spriteRenderer.color = initialColor;
    }
}
