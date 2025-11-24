using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro 사용

public class TeamSelectController : MonoBehaviour
{
    [Header("팀 버튼들")]
    public Button[] teamButtons;

    [Header("플레이어 안내 UI")]
    public TextMeshProUGUI playerTurnText; // Text → TextMeshProUGUI로 변경

    [Header("선택 확인 UI")]
    public GameObject confirmPanel;
    public TextMeshProUGUI selectedTeamText; // Text → TextMeshProUGUI로 변경
    public Button confirmButton;
    public Button cancelButton;

    private string currentSelectedTeam;

    private void Start()
    {
        if (confirmPanel != null)
            confirmPanel.SetActive(false);

        UpdatePlayerTurnText();

        for (int i = 0; i < teamButtons.Length; i++)
        {
            int index = i;
            teamButtons[i].onClick.AddListener(() => OnTeamButtonClicked(index));
        }

        if (confirmButton != null)
            confirmButton.onClick.AddListener(OnConfirmSelection);

        if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancelSelection);
    }

    private void UpdatePlayerTurnText()
    {
        if (playerTurnText != null && GameManager.Instance != null)
        {
            if (GameManager.Instance.currentSelectingPlayer == GameManager.SelectingPlayer.Player1)
            {
                playerTurnText.text = "Player 1: Select Your Team";
            }
            else
            {
                playerTurnText.text = "Player 2: Select Your Team";
            }
        }
    }

    private void OnTeamButtonClicked(int teamIndex)
    {
        // TMP 버전으로 수정
        TextMeshProUGUI teamText = teamButtons[teamIndex].GetComponentInChildren<TextMeshProUGUI>();
        if (teamText != null)
        {
            currentSelectedTeam = teamText.text;
        }
        else
        {
            currentSelectedTeam = teamButtons[teamIndex].name;
        }

        ShowConfirmPanel();
    }

    private void ShowConfirmPanel()
    {
        if (confirmPanel != null)
        {
            confirmPanel.SetActive(true);

            string playerNum = GameManager.Instance.currentSelectingPlayer == GameManager.SelectingPlayer.Player1
                ? "Player 1" : "Player 2";

            selectedTeamText.text = $"{playerNum} Select Team \n{currentSelectedTeam}";
        }
    }

    private void OnConfirmSelection()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SelectTeam(currentSelectedTeam);

            if (GameManager.Instance.currentSelectingPlayer == GameManager.SelectingPlayer.Player2
                && !string.IsNullOrEmpty(GameManager.Instance.player2Team))
            {
                SceneManager.LoadScene("CoinScene");
            }
            else
            {
                confirmPanel.SetActive(false);
                UpdatePlayerTurnText();
            }
        }
    }

    private void OnCancelSelection()
    {
        if (confirmPanel != null)
            confirmPanel.SetActive(false);
    }
}