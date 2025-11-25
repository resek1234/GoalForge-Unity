using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private int playerNumber = 1;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float dashForce = 12f;
    [SerializeField] private float dashCooldown = 0.5f;

    [Header("Super Mode")]
    [SerializeField] private float superShotPowerMultiplier = 2.5f;
    [SerializeField] private float statChangeSuperModeDuration = 5f; // Duration for Giant/SpeedBoost

    private Rigidbody2D rb;
    private CharacterType characterType;
    private float lastDashTime;
    private float horizontalInput;

    // Super Mode / Item States
    [HideInInspector] public bool isSuperMode = false;
    [HideInInspector] public float superModeSpeedMultiplier = 1f;
    [HideInInspector] public float superModeJumpMultiplier = 1f;

<<<<<<< Updated upstream
=======
    [HideInInspector] public bool hasShield = false;
    [HideInInspector] public bool hasMegaBall = false;
    [HideInInspector] public float megaBallMultiplier = 1f;
    [HideInInspector] public bool isStunned = false;
    [HideInInspector] public bool hasReversedControls = false;
    private bool isSuperShotReady = false;


>>>>>>> Stashed changes
    public int GetPlayerNumber() => playerNumber;
    public void SetPlayerNumber(int number) => playerNumber = number;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        characterType = GetComponent<CharacterType>(); // Get the CharacterType component
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
        if (playerNumber == 1)
        {
            horizontalInput = 0f;
            if (Input.GetKey(KeyCode.A)) horizontalInput = -1f;
            if (Input.GetKey(KeyCode.D)) horizontalInput = 1f;

            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + dashCooldown)
            {
                Dash();
            }
        }
        else if (playerNumber == 2)
        {
            horizontalInput = 0f;
            if (Input.GetKey(KeyCode.LeftArrow)) horizontalInput = -1f;
            if (Input.GetKey(KeyCode.RightArrow)) horizontalInput = 1f;

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

        if (playerNumber == 1)
        {
            if (Input.GetKey(KeyCode.W)) moveY = 1f;
            if (Input.GetKey(KeyCode.S)) moveY = -1f;
        }
        else if (playerNumber == 2)
        {
            if (Input.GetKey(KeyCode.UpArrow)) moveY = 1f;
            if (Input.GetKey(KeyCode.DownArrow)) moveY = -1f;
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

    // --- Super Mode Methods ---

    public void ActivateSuperMode(SuperModeType mode, PlayerController opponent)
    {
        switch (mode)
        {
            case SuperModeType.SuperShot:
                isSuperShotReady = true;
                // Maybe play a sound or show a visual effect to indicate shot is ready
                Debug.Log($"Player {playerNumber}'s Super Shot is ready!");
                break;
            case SuperModeType.Freeze:
                if (opponent != null)
                {
                    opponent.Stun(1f);
                    Debug.Log($"Player {playerNumber} used Freeze on Player {opponent.GetPlayerNumber()}!");
                }
                break;
            case SuperModeType.ControlScramble:
                 if (opponent != null)
                {
                    opponent.ScrambleControls(3f);
                    Debug.Log($"Player {playerNumber} used ControlScramble on Player {opponent.GetPlayerNumber()}!");
                }
                break;
            case SuperModeType.Giant:
            case SuperModeType.SpeedBoost:
                StartCoroutine(StatChangeCoroutine(mode, statChangeSuperModeDuration));
                Debug.Log($"Player {playerNumber} used {mode}!");
                break;
        }
    }

    private IEnumerator StatChangeCoroutine(SuperModeType mode, float duration)
    {
        if (characterType != null && !characterType.IsSuperModeActive())
        {
            characterType.ActivateSuperMode(mode);
            yield return new WaitForSeconds(duration);
            characterType.DeactivateSuperMode();
        }
    }

    public void Stun(float duration)
    {
        if (!isStunned) // Prevent stacking
        {
            StartCoroutine(StunCoroutine(duration));
        }
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        // Optional: Add visual effect for stun
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }

    public void ScrambleControls(float duration)
    {
        if (!hasReversedControls) // Prevent stacking
        {
            StartCoroutine(ScrambleControlsCoroutine(duration));
        }
    }

    private IEnumerator ScrambleControlsCoroutine(float duration)
    {
        hasReversedControls = true;
        yield return new WaitForSeconds(duration);
        hasReversedControls = false;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            if (ball != null)
            {
                Vector2 kickDirection = (collision.transform.position - transform.position).normalized;
<<<<<<< Updated upstream
                float kickPower = 3.5f * (isSuperMode ? 1.2f : 1f);
                ball.Kick(kickDirection * kickPower);
=======
                float kickPower;

                if (isSuperShotReady)
                {
                    kickPower = 3.5f * superShotPowerMultiplier;
                    ball.Kick(kickDirection * kickPower);
                    isSuperShotReady = false;
                    Debug.Log("Super Shot executed!");
                }
                else
                {
                    kickPower = 3.5f * (isSuperMode ? 1.2f : 1f) * megaBallMultiplier;
                    ball.Kick(kickDirection * kickPower);
                }

>>>>>>> Stashed changes

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

