using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // 추가!

public class MainGameController : MonoBehaviour
{
    [Header("UI 요소")]
    public TextMeshProUGUI timerText; // 전부 TextMeshProUGUI로 변경
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;
    public TextMeshProUGUI player1NameText;
    public TextMeshProUGUI player2NameText;
    public TextMeshProUGUI controlGuideText;

    [Header("게임 설정")]
    public float gameDuration = 60f;

    [Header("진영 오브젝트")]
    public GameObject player1TeamParent;
    public GameObject player2TeamParent;

    private float remainingTime;
    private bool gameRunning = false;
    private int player1Score = 0;
    private int player2Score = 0;

    private void Start()
    {
        remainingTime = gameDuration;
        SetupGame();
        UpdateScoreUI();
        UpdateControlGuide();

        Invoke("StartGame", 3f);
    }

    private void SetupGame()
    {
        if (GameManager.Instance == null) return;

        SetupSides();

        if (GameManager.Instance.player1IsLeftSide)
        {
            if (player1NameText != null)
                player1NameText.text = GameManager.Instance.player1Team;

            if (player2NameText != null)
                player2NameText.text = GameManager.Instance.player2Team;
        }
        else
        {
            if (player1NameText != null)
                player1NameText.text = GameManager.Instance.player2Team;

            if (player2NameText != null)
                player2NameText.text = GameManager.Instance.player1Team;
        }

        player1Score = GameManager.Instance.player1Score;
        player2Score = GameManager.Instance.player2Score;
    }

    private void SetupSides()
    {
        if (GameManager.Instance.player1IsLeftSide)
        {
            if (player1TeamParent != null)
                player1TeamParent.transform.position = new Vector3(-5, 0, 0);

            if (player2TeamParent != null)
                player2TeamParent.transform.position = new Vector3(5, 0, 0);
        }
        else
        {
            if (player1TeamParent != null)
                player1TeamParent.transform.position = new Vector3(5, 0, 0);

            if (player2TeamParent != null)
                player2TeamParent.transform.position = new Vector3(-5, 0, 0);
        }
    }

    private void UpdateControlGuide()
    {
        if (controlGuideText != null)
        {
            if (GameManager.Instance != null && GameManager.Instance.player1IsLeftSide)
            {
                controlGuideText.text = "Player1: WASD + Space | Player2: 방향키 + Enter";
            }
            else
            {
                controlGuideText.text = "Player1: 방향키 + Enter | Player2: WASD + Space";
            }
        }
    }

    private void StartGame()
    {
        gameRunning = true;
        if (controlGuideText != null)
            controlGuideText.text = "";
    }

    private void Update()
    {
        if (gameRunning)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                EndGame();
            }

            UpdateTimerUI();
        }

        // 테스트용
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddPlayer1Score();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AddPlayer2Score();
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private void UpdateScoreUI()
    {
        if (GameManager.Instance != null && GameManager.Instance.player1IsLeftSide)
        {
            if (player1ScoreText != null)
                player1ScoreText.text = player1Score.ToString();

            if (player2ScoreText != null)
                player2ScoreText.text = player2Score.ToString();
        }
        else
        {
            if (player1ScoreText != null)
                player1ScoreText.text = player2Score.ToString();

            if (player2ScoreText != null)
                player2ScoreText.text = player1Score.ToString();
        }
    }

    public void AddPlayer1Score()
    {
        player1Score++;
        UpdateScoreUI();
    }

    public void AddPlayer2Score()
    {
        player2Score++;
        UpdateScoreUI();
    }

    private void EndGame()
    {
        gameRunning = false;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.player1Score = player1Score;
            GameManager.Instance.player2Score = player2Score;
        }

        SceneManager.LoadScene("ResultScene");
    }
}