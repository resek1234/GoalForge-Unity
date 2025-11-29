using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    // ���� ������
    public string player1Team; // Player 1 ��
    public string player2Team; // Player 2 ��
    public bool player1IsLeftSide; // true�� Player1�� ����, false�� ������

    // ����
    public int player1Score = 0;
    public int player2Score = 0;

    // ���� ��� �÷��̾ �� ���� ������
    public enum SelectingPlayer { Player1, Player2 }
    public SelectingPlayer currentSelectingPlayer = SelectingPlayer.Player1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectTeam(string teamName)
    {
        if (currentSelectingPlayer == SelectingPlayer.Player1)
        {
            player1Team = teamName;
            currentSelectingPlayer = SelectingPlayer.Player2; // ������ Player2 ����
        }
        else
        {
            player2Team = teamName;
        }
    }

    public void SetCoinTossResult(bool player1IsLeft)
    {
        player1IsLeftSide = player1IsLeft;
    }

    public void ResetGame()
    {
        player1Team = "";
        player2Team = "";
        player1Score = 0;
        player2Score = 0;
        currentSelectingPlayer = SelectingPlayer.Player1;
    }
}