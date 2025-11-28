using UnityEngine;

public class GoalPost : MonoBehaviour
{
    [Header("Goal Settings")]
    [SerializeField] private int goalOwner = 1; // 1 = Player1의 골대 (Player2가 득점), 2 = Player2의 골대 (Player1이 득점)
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            OnGoal();
        }
    }
    
    void OnGoal()
    {
        int scoringPlayer = (goalOwner == 1) ? 2 : 1;
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGoalScored(scoringPlayer);
        }
        
        // Use the centralized SoundManager to play the goal sound
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayGoalSound();
        }
        
        Debug.Log($"GOAL! Player {scoringPlayer} scored!");
    }
}

