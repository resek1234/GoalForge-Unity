using UnityEngine;
using System.Collections;

public class TrapObstacle : Obstacle
{
    [Header("Trap Settings")]
    [SerializeField] private float stunDuration = 1f;
    [SerializeField] private float cooldown = 2f;
    [SerializeField] private float pulseSpeed = 3f;

    private bool isActive = true;
    private Color originalColor;
    private float pulseTimer = 0f;

    protected override void Awake()
    {
        base.Awake();
        obstacleType = ObstacleType.Trap;

        if (obstacleCollider != null)
        {
            obstacleCollider.isTrigger = true;
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
        if (isActive && spriteRenderer != null)
        {
            pulseTimer += Time.deltaTime * pulseSpeed;
            float pulse = 0.7f + Mathf.Sin(pulseTimer) * 0.3f;
            spriteRenderer.color = originalColor * pulse;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive) return;

        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            StartCoroutine(TrapPlayer(player));
        }
    }

    IEnumerator TrapPlayer(PlayerController player)
    {
        isActive = false;

        player.isStunned = true;
        CreateTrapEffect(player.transform.position);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.gray;

            yield return new WaitForSeconds(stunDuration);

            player.isStunned = false;

            spriteRenderer.color = originalColor;
        }
        else
        {
            yield return new WaitForSeconds(stunDuration);
            player.isStunned = false;
        }

        yield return new WaitForSeconds(cooldown);
        isActive = true;
    }

    void CreateTrapEffect(Vector3 position)
    {
        GameObject effectObj = new GameObject("TrapEffect");
        effectObj.transform.position = position;

        SpriteRenderer effectSr = effectObj.AddComponent<SpriteRenderer>();
        effectSr.sprite = spriteRenderer.sprite;
        effectSr.color = new Color(1f, 0f, 0f, 0.8f);
        effectSr.sortingOrder = 10;

        StartCoroutine(TrapEffectCoroutine(effectObj));
    }

    IEnumerator TrapEffectCoroutine(GameObject effectObj)
    {
        float duration = stunDuration;
        float elapsed = 0f;
        Vector3 startScale = Vector3.one * 0.5f;
        SpriteRenderer sr = effectObj.GetComponent<SpriteRenderer>();

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;

            effectObj.transform.localScale = startScale * (1f + progress);

            Color color = sr.color;
            color.a = 0.8f * (1f - progress);
            sr.color = color;

            yield return null;
        }

        Destroy(effectObj);
    }
}

