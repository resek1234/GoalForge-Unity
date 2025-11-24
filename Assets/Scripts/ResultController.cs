using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // 眠啊!

public class ResultController : MonoBehaviour
{
    public TextMeshProUGUI resultText; // Text ℃ TextMeshProUGUI
    public TextMeshProUGUI scoreText; // Text ℃ TextMeshProUGUI
    public Button replayButton;
    public Button titleButton;

    private void Start()
    {
        ShowResult();

        if (replayButton != null)
            replayButton.onClick.AddListener(OnReplayClicked);

        if (titleButton != null)
            titleButton.onClick.AddListener(OnTitleClicked);
    }

    private void ShowResult()
    {
        if (GameManager.Instance == null) return;

        int p1Score = GameManager.Instance.player1Score;
        int p2Score = GameManager.Instance.player2Score;

        if (scoreText != null)
            scoreText.text = $"{p1Score}  :  {p2Score}";

        if (resultText != null)
        {
            if (p1Score > p2Score)
            {
                resultText.text = $"{GameManager.Instance.player1Team} 铰府!";
            }
            else if (p2Score > p1Score)
            {
                resultText.text = $"{GameManager.Instance.player2Team} 铰府!";
            }
            else
            {
                resultText.text = "公铰何!";
            }
        }
    }

    private void OnReplayClicked()
    {
        GameManager.Instance.ResetGame();
        SceneManager.LoadScene("TeamScene");
    }

    private void OnTitleClicked()
    {
        GameManager.Instance.ResetGame();
        SceneManager.LoadScene("TitleScene");
    }
}