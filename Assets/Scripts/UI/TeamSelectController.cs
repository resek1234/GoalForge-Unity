using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TeamSelectController : MonoBehaviour
{
    [Header("Team Buttons")]
    public Button[] teamButtons;

    [Header("Player Info UI")]
    public TextMeshProUGUI playerTurnText;

    [Header("Confirm UI")]
    public GameObject confirmPanel;
    public TextMeshProUGUI selectedTeamText;
    public Button confirmButton;
    public Button cancelButton;

    private string currentSelectedTeam;

    private void Start()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.ResetGame();
        }

        if (confirmPanel != null)
            confirmPanel.SetActive(false);

        UpdatePlayerTurnText();

        for (int i = 0; i < teamButtons.Length; i++)
        {
            int index = i;
            if (teamButtons[i] != null)
            {
                teamButtons[i].onClick.AddListener(() => OnTeamButtonClicked(index));
            }
        }

        if (confirmButton != null)
            confirmButton.onClick.AddListener(OnConfirmSelection);

        if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancelSelection);
    }

    [ContextMenu("Auto Arrange Buttons")]
    private void ArrangeButtons()
    {
        // 3x3 Grid Layout for Team Buttons
        float startX = -250f;
        float startY = 150f;
        float gapX = 250f;
        float gapY = 150f;

        for (int i = 0; i < teamButtons.Length; i++)
        {
            if (teamButtons[i] == null) continue;

            RectTransform rt = teamButtons[i].GetComponent<RectTransform>();
            if (rt != null)
            {
                int row = i / 3;
                int col = i % 3;

                float x = startX + (col * gapX);
                float y = startY - (row * gapY);

                rt.anchoredPosition = new Vector2(x, y);
            }
        }

        // Center the Confirm Panel
        if (confirmPanel != null)
        {
            RectTransform rt = confirmPanel.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchoredPosition = Vector2.zero;
                // Ensure it has a size
                if (rt.sizeDelta.x < 100) rt.sizeDelta = new Vector2(600, 400);
            }
            
            // Ensure buttons are visible within the panel
            if (confirmButton != null)
            {
                RectTransform btnRt = confirmButton.GetComponent<RectTransform>();
                if (btnRt != null)
                {
                    // Position Confirm button to the right bottom
                    btnRt.anchoredPosition = new Vector2(150, -120);
                }
            }

            if (cancelButton != null)
            {
                RectTransform btnRt = cancelButton.GetComponent<RectTransform>();
                if (btnRt != null)
                {
                    // Position Cancel button to the left bottom
                    btnRt.anchoredPosition = new Vector2(-150, -120);
                }
            }
            
            if (selectedTeamText != null)
            {
                 RectTransform textRt = selectedTeamText.GetComponent<RectTransform>();
                 if (textRt != null)
                 {
                     textRt.anchoredPosition = new Vector2(0, 50);
                 }
            }
        }
    }

    private void UpdatePlayerTurnText()
    {
        if (playerTurnText != null && PlayerManager.Instance != null)
        {
            if (PlayerManager.Instance.currentSelectingPlayer == PlayerManager.SelectingPlayer.Player1)
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
        // Get team name from text or object name
        if (teamButtons[teamIndex] == null) return;

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

            string playerNum = PlayerManager.Instance.currentSelectingPlayer == PlayerManager.SelectingPlayer.Player1
                ? "Player 1" : "Player 2";

            if (selectedTeamText != null)
            {
                selectedTeamText.text = $"{playerNum} Select Team \n{currentSelectedTeam}";
            }
        }
    }

    private void OnConfirmSelection()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SelectTeam(currentSelectedTeam);

            if (PlayerManager.Instance.currentSelectingPlayer == PlayerManager.SelectingPlayer.Player2
                && !string.IsNullOrEmpty(PlayerManager.Instance.player2Team))
            {
                // Both players selected, go to CoinScene
                SceneManager.LoadScene("CoinScene");
            }
            else
            {
                // Player 1 selected, now Player 2's turn
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