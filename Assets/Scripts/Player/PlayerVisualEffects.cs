using UnityEngine;
using System.Collections;

public class PlayerVisualEffects : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private PlayerController playerController;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
        
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
        if (playerController == null || spriteRenderer == null) return;

        if (playerController.hasShield)
        {
            float pulse = 0.7f + Mathf.Sin(Time.time * 10f) * 0.3f;
            spriteRenderer.color = Color.Lerp(originalColor, Color.cyan, pulse * 0.5f);
        }
        else if (playerController.hasMegaBall)
        {
            float pulse = 0.7f + Mathf.Sin(Time.time * 8f) * 0.3f;
            spriteRenderer.color = Color.Lerp(originalColor, Color.red, pulse * 0.4f);
        }
        else if (playerController.hasReversedControls)
        {
            float pulse = 0.7f + Mathf.Sin(Time.time * 12f) * 0.3f;
            spriteRenderer.color = Color.Lerp(originalColor, Color.green, pulse * 0.5f);
        }
        else if (playerController.isStunned)
        {
            spriteRenderer.color = Color.gray;
        }
        else
        {
            spriteRenderer.color = originalColor;
        }
    }

    public void PlaySpeedBoostEffect()
    {
        StartCoroutine(SpeedBoostEffectCoroutine());
    }

    IEnumerator SpeedBoostEffectCoroutine()
    {
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float flash = Mathf.Sin(elapsed * 20f);
            spriteRenderer.color = Color.Lerp(originalColor, Color.yellow, flash * 0.5f);
            yield return null;
        }
    }

    public void PlayWindCurseEffect()
    {
        StartCoroutine(WindCurseEffectCoroutine());
    }

    IEnumerator WindCurseEffectCoroutine()
    {
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float flash = Mathf.Sin(elapsed * 15f);
            spriteRenderer.color = Color.Lerp(originalColor, Color.green, flash * 0.6f);
            yield return null;
        }
    }
}

