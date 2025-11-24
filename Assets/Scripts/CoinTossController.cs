using System.Collections;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoinTossController : MonoBehaviour
{
    [Header("3D 동전 렌더링")]
    public GameObject coin3D;              // 3D 동전 오브젝트
    public Camera coinRenderCamera;        // 동전 전용 카메라
    public RawImage coinRawImage;          // UI에 표시할 RawImage

    [Header("UI")]
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI sideInfoText;
    public Button playButton;

    [Header("애니메이션 - Canvas 좌표")]
    public Vector2 startPosition = new Vector2(500, 300);
    public Vector2 endPosition = new Vector2(0, 0);
    public float tossDuration = 2f;
    public float spinSpeed = 720f;
    public float sizeStart = 100f;         // 시작 크기
    public float sizeEnd = 400f;           // 끝 크기

    private bool player1IsLeft;
    private RectTransform imageRect;
    private RenderTexture renderTexture;

    private void Start()
    {
        // RawImage의 RectTransform
        if (coinRawImage != null)
            imageRect = coinRawImage.GetComponent<RectTransform>();

        // RenderTexture 설정
        SetupRenderTexture();

        if (playButton != null)
        {
            playButton.gameObject.SetActive(false);
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        if (resultText != null)
            resultText.text = "동전을 던지는 중...";

        if (sideInfoText != null)
            sideInfoText.text = "";

        StartCoroutine(Auto3DCoinToss());
    }

    private void SetupRenderTexture()
    {
        // RenderTexture 생성
        renderTexture = new RenderTexture(512, 512, 24);
        renderTexture.antiAliasing = 4;

        // 카메라에 연결
        if (coinRenderCamera != null)
        {
            coinRenderCamera.targetTexture = renderTexture;

            coinRenderCamera.clearFlags = CameraClearFlags.SolidColor;
            coinRenderCamera.backgroundColor = new UnityEngine.Color(0, 0, 0, 0);
        }


        // RawImage에 연결
        if (coinRawImage != null)
            coinRawImage.texture = renderTexture;
    }

    private IEnumerator Auto3DCoinToss()
    {
        yield return new WaitForSeconds(0.5f);

        player1IsLeft = Random.Range(0, 2) == 0;

        // RawImage 시작 위치와 크기
        if (imageRect != null)
        {
            imageRect.anchoredPosition = startPosition;
            imageRect.sizeDelta = new Vector2(sizeStart, sizeStart);
        }

        float elapsedTime = 0f;

        while (elapsedTime < tossDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / tossDuration;

            // UI 이미지 이동
            Vector2 currentPos = Vector2.Lerp(startPosition, endPosition, t);
            imageRect.anchoredPosition = currentPos;

            // UI 이미지 크기 변화
            float currentSize = Mathf.Lerp(sizeStart, sizeEnd, t);
            imageRect.sizeDelta = new Vector2(currentSize, currentSize);

            // 3D 동전 회전
            if (coin3D != null)
            {
                float rotation = spinSpeed * elapsedTime;
                coin3D.transform.rotation = Quaternion.Euler(0, rotation, 0);
            }

            yield return null;
        }

        // 최종 회전
        float finalRotation = player1IsLeft ? 0f : 180f;
        if (coin3D != null)
            coin3D.transform.rotation = Quaternion.Euler(0, finalRotation, 0);

        imageRect.anchoredPosition = endPosition;
        imageRect.sizeDelta = new Vector2(sizeEnd, sizeEnd);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetCoinTossResult(player1IsLeft);
        }

        ShowResult();
    }

    private void ShowResult()
    {
        if (resultText != null)
            resultText.text = player1IsLeft ? "앞면!" : "뒷면!";

        if (sideInfoText != null)
        {
            string p1Team = GameManager.Instance != null ? GameManager.Instance.player1Team : "Player 1";
            string p2Team = GameManager.Instance != null ? GameManager.Instance.player2Team : "Player 2";

            if (player1IsLeft)
            {
                sideInfoText.text = $"왼쪽: {p1Team}\n오른쪽: {p2Team}";
            }
            else
            {
                sideInfoText.text = $"왼쪽: {p2Team}\n오른쪽: {p1Team}";
            }
        }

        if (playButton != null)
            playButton.gameObject.SetActive(true);
    }

    private void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void OnDestroy()
    {
        // RenderTexture 정리
        if (renderTexture != null)
            renderTexture.Release();
    }
}