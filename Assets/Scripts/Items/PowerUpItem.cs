using UnityEngine;

public enum PowerUpType
{
    SpeedBoost,
    Shield,
    MegaBall,
    TrapBomb,
    WindCurse
}

public abstract class PowerUpItem : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] protected PowerUpType itemType;
    [SerializeField] protected float duration = 5f;
    [SerializeField] protected float rotationSpeed = 100f;
    [SerializeField] protected float bobSpeed = 2f;
    [SerializeField] protected float bobHeight = 0.3f;

    protected SpriteRenderer spriteRenderer;
    protected Collider2D itemCollider;
    protected bool isCollected = false;
    protected Vector3 startPosition;
    protected float bobTimer = 0f;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<Collider2D>();
    }

    protected virtual void Start()
    {
        startPosition = transform.position;
        bobTimer = Random.Range(0f, Mathf.PI * 2f);
    }

    protected virtual void Update()
    {
        if (!isCollected)
        {
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

            bobTimer += Time.deltaTime * bobSpeed;
            float newY = startPosition.y + Mathf.Sin(bobTimer) * bobHeight;
            transform.position = new Vector3(startPosition.x, newY, startPosition.z);

            float pulse = 1f + Mathf.Sin(bobTimer * 2f) * 0.1f;
            transform.localScale = Vector3.one * 0.8f * pulse;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected) return;

        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            CollectItem(player);
        }
    }

    protected virtual void CollectItem(PlayerController player)
    {
        isCollected = true;
        
        // Play item pickup sound
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayItemPickupSound();
        }

        ApplyEffect(player);
        CreateCollectEffect();

        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
        if (itemCollider != null)
            itemCollider.enabled = false;

        Destroy(gameObject, 0.5f);
    }

    protected virtual void CreateCollectEffect()
    {
        GameObject effectObj = new GameObject("CollectEffect");
        effectObj.transform.position = transform.position;

        SpriteRenderer effectSr = effectObj.AddComponent<SpriteRenderer>();
        effectSr.sprite = spriteRenderer.sprite;
        effectSr.color = spriteRenderer.color;
        effectSr.sortingOrder = 10;

        StartCoroutine(CollectEffectCoroutine(effectObj));
    }

    protected System.Collections.IEnumerator CollectEffectCoroutine(GameObject effectObj)
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 startScale = effectObj.transform.localScale;
        SpriteRenderer sr = effectObj.GetComponent<SpriteRenderer>();

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;

            effectObj.transform.localScale = startScale * (1f + progress * 2f);

            Color color = sr.color;
            color.a = 1f - progress;
            sr.color = color;

            yield return null;
        }

        Destroy(effectObj);
    }

    protected abstract void ApplyEffect(PlayerController player);

    public PowerUpType GetItemType() => itemType;
    public float GetDuration() => duration;
}

