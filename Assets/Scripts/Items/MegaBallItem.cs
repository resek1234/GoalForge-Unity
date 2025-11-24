using UnityEngine;

public class MegaBallItem : PowerUpItem
{
    [Header("Mega Ball Settings")]
    [SerializeField] private float kickPowerMultiplier = 2f;

    protected override void Awake()
    {
        base.Awake();
        itemType = PowerUpType.MegaBall;
    }

    protected override void ApplyEffect(PlayerController player)
    {
        player.StartCoroutine(ApplyMegaBall(player));
    }

    private System.Collections.IEnumerator ApplyMegaBall(PlayerController player)
    {
        player.hasMegaBall = true;
        player.megaBallMultiplier = kickPowerMultiplier;

        if (GameUI.Instance != null)
        {
            GameUI.Instance.ShowPowerUpEffect(player.GetPlayerNumber(), "⚽ MEGA BALL!", duration);
        }

        yield return new WaitForSeconds(duration);

        player.hasMegaBall = false;
        player.megaBallMultiplier = 1f;
    }
}

