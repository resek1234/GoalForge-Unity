using System.Collections;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoinTossController : MonoBehaviour
{
    [Header("3D Coin Object")]
    public GameObject coin3D;
    public Camera coinRenderCamera;
    public RawImage coinRawImage;

    [Header("UI")]
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI sideInfoText;
    public Button playButton;

    [Header("Canvas Position")]
    public Vector2 startPosition = new Vector2(500, 300);
    public Vector2 endPosition = new Vector2(0, 0);
    public float tossDuration = 2f;
    public float spinSpeed = 720f;
    public float sizeStart = 100f;
    public float sizeEnd = 400f;

    private bool player1IsLeft;
    private RectTransform imageRect;
    private RenderTexture renderTexture;

    private void Start()
    {
        if (coinRawImage != null)
            imageRect = coinRawImage.GetComponent<RectTransform>();

        SetupRenderTexture();

        if (playButton != null)
        {
            playButton.gameObject.SetActive(false);
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        if (resultText != null)
            resultText.text = "Coin Tossing...";

        if (sideInfoText != null)
            sideInfoText.text = "";

        StartCoroutine(Auto3DCoinToss());
    }

    private void SetupRenderTexture()
    {
        renderTexture = new RenderTexture(512, 512, 24);
        renderTexture.antiAliasing = 4;

        if (coinRenderCamera != null)
        {
            coinRenderCamera.targetTexture = renderTexture;

            coinRenderCamera.clearFlags = CameraClearFlags.SolidColor;
            coinRenderCamera.backgroundColor = new UnityEngine.Color(0, 0, 0, 0);
        }


        if (coinRawImage != null)
            coinRawImage.texture = renderTexture;
    }

    private IEnumerator Auto3DCoinToss()
    {
        yield return new WaitForSeconds(0.5f);

        player1IsLeft = Random.Range(0, 2) == 0;

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

            Vector2 currentPos = Vector2.Lerp(startPosition, endPosition, t);
            imageRect.anchoredPosition = currentPos;

            float currentSize = Mathf.Lerp(sizeStart, sizeEnd, t);
            imageRect.sizeDelta = new Vector2(currentSize, currentSize);

            if (coin3D != null)
            {
                float rotation = spinSpeed * elapsedTime;
                coin3D.transform.rotation = Quaternion.Euler(0, rotation, 0);
            }

            yield return null;
        }

        float finalRotation = player1IsLeft ? 0f : 180f;
        if (coin3D != null)
            coin3D.transform.rotation = Quaternion.Euler(0, finalRotation, 0);

        imageRect.anchoredPosition = endPosition;
        imageRect.sizeDelta = new Vector2(sizeEnd, sizeEnd);

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SetCoinTossResult(player1IsLeft);
        }

        ShowResult();
    }

    private void ShowResult()
    {
        if (resultText != null)
            resultText.text = player1IsLeft ? "Heads!" : "Tails!";

        if (sideInfoText != null)
        {
            string p1Team = PlayerManager.Instance != null ? PlayerManager.Instance.player1Team : "Player 1";
            string p2Team = PlayerManager.Instance != null ? PlayerManager.Instance.player2Team : "Player 2";

            if (player1IsLeft)
            {
                sideInfoText.text = $"First: {p1Team}\nSecond: {p2Team}";
            }
            else
            {
                sideInfoText.text = $"First: {p2Team}\nSecond: {p1Team}";
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
        if (renderTexture != null)
            renderTexture.Release();
    }
}