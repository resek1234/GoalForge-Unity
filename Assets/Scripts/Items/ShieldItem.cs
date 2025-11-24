using UnityEngine;

public class ShieldItem : PowerUpItem
{
    protected override void Awake()
    {
        base.Awake();
        itemType = PowerUpType.Shield;
    }

    protected override void ApplyEffect(PlayerController player)
    {
        player.StartCoroutine(ApplyShield(player));
    }

    private System.Collections.IEnumerator ApplyShield(PlayerController player)
    {
        player.hasShield = true;

        if (GameUI.Instance != null)
        {
            GameUI.Instance.ShowPowerUpEffect(player.GetPlayerNumber(), "🛡️ SHIELD!", duration);
        }

        yield return new WaitForSeconds(duration);

        player.hasShield = false;
    }
}

