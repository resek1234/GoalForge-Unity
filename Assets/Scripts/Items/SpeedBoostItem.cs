using UnityEngine;

public class SpeedBoostItem : PowerUpItem
{
    [Header("Speed Boost Settings")]
    [SerializeField] private float speedMultiplier = 2f;

    protected override void Awake()
    {
        base.Awake();
        itemType = PowerUpType.SpeedBoost;
    }

    protected override void ApplyEffect(PlayerController player)
    {
        player.StartCoroutine(ApplySpeedBoost(player));
    }

    private System.Collections.IEnumerator ApplySpeedBoost(PlayerController player)
    {
        float originalSpeed = player.moveSpeed;
        player.moveSpeed *= speedMultiplier;

        if (GameUI.Instance != null)
        {
            GameUI.Instance.ShowPowerUpEffect(player.GetPlayerNumber(), "⚡ SPEED BOOST!", duration);
        }

        PlayerVisualEffects visualEffects = player.GetComponent<PlayerVisualEffects>();
        if (visualEffects != null)
        {
            visualEffects.PlaySpeedBoostEffect();
        }

        yield return new WaitForSeconds(duration);

        player.moveSpeed = originalSpeed;
    }
}

