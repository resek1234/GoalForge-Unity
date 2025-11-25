using UnityEngine;

public class WindCurseItem : PowerUpItem
{
    [Header("Wind Curse Settings")]
    [SerializeField] private float curseDuration = 5f;

    protected override void Awake()
    {
        base.Awake();
        itemType = PowerUpType.WindCurse;
        duration = curseDuration;
    }

    protected override void ApplyEffect(PlayerController player)
    {
        PlayerController opponent = FindOpponent(player);
        if (opponent != null)
        {
            player.StartCoroutine(CurseOpponent(opponent));
        }
    }

    private PlayerController FindOpponent(PlayerController currentPlayer)
    {
        PlayerController[] allPlayers = Object.FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        
        foreach (PlayerController p in allPlayers)
        {
            if (p != currentPlayer)
            {
                return p;
            }
        }
        
        return null;
    }

    private System.Collections.IEnumerator CurseOpponent(PlayerController opponent)
    {
        opponent.hasReversedControls = true;

        if (GameUI.Instance != null)
        {
            GameUI.Instance.ShowPowerUpEffect(opponent.GetPlayerNumber(), "💨 REVERSED!", curseDuration);
        }

        PlayerVisualEffects visualEffects = opponent.GetComponent<PlayerVisualEffects>();
        if (visualEffects != null)
        {
            visualEffects.PlayWindCurseEffect();
        }

        yield return new WaitForSeconds(curseDuration);

        opponent.hasReversedControls = false;
    }
}

