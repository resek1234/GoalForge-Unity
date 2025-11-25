using UnityEngine;

public class WindZone : Obstacle
{
    [Header("Wind Settings")]
    [SerializeField] private Vector2 windForce = new Vector2(5f, 0f);
    [SerializeField] private float windStrength = 3f;
    [SerializeField] private float waveSpeed = 2f;
    [SerializeField] private float waveAmount = 0.2f;

    private Color originalColor;
    private float waveTimer = 0f;

    protected override void Awake()
    {
        base.Awake();
        obstacleType = ObstacleType.WindZone;

        if (obstacleCollider != null)
        {
            obstacleCollider.isTrigger = true;
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        windForce = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 5f;
    }

    void Update()
    {
        if (spriteRenderer != null)
        {
            waveTimer += Time.deltaTime * waveSpeed;
            float wave = 0.5f + Mathf.Sin(waveTimer) * waveAmount;
            Color newColor = originalColor;
            newColor.a = wave;
            spriteRenderer.color = newColor;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(windForce.normalized * windStrength, ForceMode2D.Force);
            }
        }

        Ball ball = collision.GetComponent<Ball>();
        if (ball != null)
        {
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(windForce.normalized * windStrength * 0.5f, ForceMode2D.Force);
            }
        }
    }
}

