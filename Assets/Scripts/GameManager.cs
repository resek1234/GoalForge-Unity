using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // 게임 데이터
    public string player1Team; // Player 1 팀
    public string player2Team; // Player 2 팀
    public bool player1IsLeftSide; // true면 Player1이 왼쪽, false면 오른쪽

    // 점수
    public int player1Score = 0;
    public int player2Score = 0;

    // 현재 어느 플레이어가 팀 선택 중인지
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
            currentSelectingPlayer = SelectingPlayer.Player2; // 다음은 Player2 차례
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