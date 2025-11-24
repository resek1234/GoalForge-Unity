using UnityEngine;

public class TrapBombItem : PowerUpItem
{
    [Header("Trap Bomb Settings")]
    [SerializeField] private float stunDuration = 2f;

    protected override void Awake()
    {
        base.Awake();
        itemType = PowerUpType.TrapBomb;
    }

    protected override void ApplyEffect(PlayerController player)
    {
        PlayerController opponent = FindOpponent(player);
        if (opponent != null)
        {
            player.StartCoroutine(StunOpponent(opponent));
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

    private System.Collections.IEnumerator StunOpponent(PlayerController opponent)
    {
        opponent.isStunned = true;

        if (GameUI.Instance != null)
        {
            GameUI.Instance.ShowPowerUpEffect(opponent.GetPlayerNumber(), "⚠️ STUNNED!", stunDuration);
        }

        CreateStunEffect(opponent.transform.position);

        yield return new WaitForSeconds(stunDuration);

        opponent.isStunned = false;
    }

    private void CreateStunEffect(Vector3 position)
    {
        GameObject effectObj = new GameObject("StunEffect");
        effectObj.transform.position = position;

        SpriteRenderer effectSr = effectObj.AddComponent<SpriteRenderer>();
        effectSr.sprite = spriteRenderer.sprite;
        effectSr.color = new Color(1f, 0f, 0f, 0.8f);
        effectSr.sortingOrder = 10;

        StartCoroutine(StunEffectCoroutine(effectObj));
    }

    private System.Collections.IEnumerator StunEffectCoroutine(GameObject effectObj)
    {
        float elapsed = 0f;
        Vector3 startScale = Vector3.one * 0.5f;
        SpriteRenderer sr = effectObj.GetComponent<SpriteRenderer>();

        while (elapsed < stunDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / stunDuration;

            effectObj.transform.localScale = startScale * (1f + progress);
            effectObj.transform.Rotate(0f, 0f, 360f * Time.deltaTime);

            Color color = sr.color;
            color.a = 0.8f * (1f - progress);
            sr.color = color;

            yield return null;
        }

        Destroy(effectObj);
    }
}

