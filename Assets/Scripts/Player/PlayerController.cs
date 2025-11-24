using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private int playerNumber = 1;

    [Header("Movement")]
    [SerializeField] public float moveSpeed = 4f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float dashForce = 12f;
    [SerializeField] private float dashCooldown = 0.5f;

    private Rigidbody2D rb;
    private float lastDashTime;
    private float horizontalInput;

    [HideInInspector] public bool isSuperMode = false;
    [HideInInspector] public float superModeSpeedMultiplier = 1f;
    [HideInInspector] public float superModeJumpMultiplier = 1f;

    [HideInInspector] public bool hasShield = false;
    [HideInInspector] public bool hasMegaBall = false;
    [HideInInspector] public float megaBallMultiplier = 1f;
    [HideInInspector] public bool isStunned = false;
    [HideInInspector] public bool hasReversedControls = false;

    public int GetPlayerNumber() => playerNumber;
    public void SetPlayerNumber(int number) => playerNumber = number;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.linearDamping = 2f;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = 1;
        }
    }

    void Update()
    {
        GetInput();
    }

    void FixedUpdate()
    {
        Move();
    }

    void GetInput()
    {
        if (isStunned)
        {
            horizontalInput = 0f;
            return;
        }

        float inputMultiplier = hasReversedControls ? -1f : 1f;

        if (playerNumber == 1)
        {
            horizontalInput = 0f;
            if (Input.GetKey(KeyCode.A)) horizontalInput = -1f * inputMultiplier;
            if (Input.GetKey(KeyCode.D)) horizontalInput = 1f * inputMultiplier;

            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + dashCooldown)
            {
                Dash();
            }
        }
        else if (playerNumber == 2)
        {
            horizontalInput = 0f;
            if (Input.GetKey(KeyCode.LeftArrow)) horizontalInput = -1f * inputMultiplier;
            if (Input.GetKey(KeyCode.RightArrow)) horizontalInput = 1f * inputMultiplier;

            if (Input.GetKeyDown(KeyCode.RightShift) && Time.time >= lastDashTime + dashCooldown)
            {
                Dash();
            }
        }
    }

    void Move()
    {
        float currentSpeed = moveSpeed * superModeSpeedMultiplier;
        float moveX = horizontalInput * currentSpeed;
        float moveY = 0f;

        float inputMultiplier = hasReversedControls ? -1f : 1f;

        if (playerNumber == 1)
        {
            if (Input.GetKey(KeyCode.W)) moveY = 1f * inputMultiplier;
            if (Input.GetKey(KeyCode.S)) moveY = -1f * inputMultiplier;
        }
        else if (playerNumber == 2)
        {
            if (Input.GetKey(KeyCode.UpArrow)) moveY = 1f * inputMultiplier;
            if (Input.GetKey(KeyCode.DownArrow)) moveY = -1f * inputMultiplier;
        }
        moveY *= currentSpeed;
        rb.linearVelocity = new Vector2(moveX, moveY);
    }

    void Jump()
    {
    }

    void Dash()
    {
        float dashDirection = horizontalInput != 0 ? horizontalInput : (transform.localScale.x > 0 ? 1 : -1);
        rb.AddForce(Vector2.right * dashDirection * dashForce, ForceMode2D.Impulse);
        lastDashTime = Time.time;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayDashSound();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            if (ball != null)
            {
                Vector2 kickDirection = (collision.transform.position - transform.position).normalized;
                float kickPower = 3.5f * (isSuperMode ? 1.2f : 1f) * megaBallMultiplier;
                ball.Kick(kickDirection * kickPower);

                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlayKickSound();
                }
                SuperModeManager superModeManager = Object.FindAnyObjectByType<SuperModeManager>();
                if (superModeManager != null)
                {
                    Debug.Log("[PlayerController] Notifying SuperModeManager.OnBallTouch for " + name);
                    superModeManager.OnBallTouch(this);
                }
                else
                {
                    Debug.LogWarning("[PlayerController] SuperModeManager not found in scene");
                }
            }
        }
    }
}

