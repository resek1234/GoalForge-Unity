using UnityEngine;
using System.Collections;

public class SuperModeManager : MonoBehaviour
{
    [Header("Gauge Settings")]
    [SerializeField] private float maxGauge = 100f;
    [SerializeField] private float autoChargeRate = 1f;
    [SerializeField, Range(0f, 1f)] private float ballTouchBonusPercent = 0.1f; // 공에 닿을 때 최대 게이지의 비율 (0.1 = 10%)

    private const float AutoChargeSpeedMultiplier = 5f;

    [Header("Super Mode Settings")]
    [SerializeField] private float superModeCooldown = 8f;
    [SerializeField] private SuperModeType player1SelectedMode = SuperModeType.SuperShot;
    [SerializeField] private SuperModeType player2SelectedMode = SuperModeType.Freeze;


    [Header("Player References")]
    [SerializeField] private PlayerController player1;
    [SerializeField] private PlayerController player2;

    [Header("Audio")]
    [SerializeField] private AudioClip superModeActivateSound;

    private float player1Gauge = 0f;
    private float player2Gauge = 0f;

    private float player1LastSuperModeTime = -100f;
    private float player2LastSuperModeTime = -100f;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

        void Start()
        {
            if (player1 == null)
            {
                GameObject p1 = GameObject.FindGameObjectWithTag("Player1");
                if (p1 != null) player1 = p1.GetComponent<PlayerController>();
            }
    
            if (player2 == null)
            {
                GameObject p2 = GameObject.FindGameObjectWithTag("Player2");
                if (p2 != null) player2 = p2.GetComponent<PlayerController>();
            }
    
            // Load selected super modes from GameSettings if it exists
            if (GameSettings.Instance != null)
            {
                player1SelectedMode = GameSettings.Instance.player1SuperMode;
                player2SelectedMode = GameSettings.Instance.player2SuperMode;
                Debug.Log($"Loaded P1 Mode: {player1SelectedMode}, P2 Mode: {player2SelectedMode} from GameSettings.");
            }
            else
            {
                // For testing without the selection scene, we use the values set in the Inspector
                Debug.LogWarning("GameSettings.Instance not found. Using default values set in SuperModeManager inspector.");
            }
        }
    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsGameActive())
        {
            return;
        }

        AutoChargeGauge();
        CheckForManualActivation();
    }

    void AutoChargeGauge()
    {
        float rate = autoChargeRate * AutoChargeSpeedMultiplier;

        if (player1Gauge < maxGauge)
        {
            player1Gauge += rate * Time.deltaTime;
            player1Gauge = Mathf.Min(player1Gauge, maxGauge);
        }

        if (player2Gauge < maxGauge)
        {
            player2Gauge += rate * Time.deltaTime;
            player2Gauge = Mathf.Min(player2Gauge, maxGauge);
        }
    }

    void CheckForManualActivation()
    {
        // Player 1 Activation
        if (player1Gauge >= maxGauge && Time.time >= player1LastSuperModeTime + superModeCooldown)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ActivateSuperMode(1);
            }
        }

        // Player 2 Activation
        if (player2Gauge >= maxGauge && Time.time >= player2LastSuperModeTime + superModeCooldown)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            {
                ActivateSuperMode(2);
            }
        }
    }

    public void OnBallTouch(PlayerController player)
    {
        if (player == null)
        {
            Debug.LogWarning("[SuperModeManager] OnBallTouch called with null player");
            return;
        }

        int playerNumber = player.GetPlayerNumber();

        if (playerNumber == 1)
        {
            if (player1 == null) player1 = player;
            float bonus = maxGauge * ballTouchBonusPercent;
            player1Gauge += bonus;
            player1Gauge = Mathf.Min(player1Gauge, maxGauge);
        }
        else if (playerNumber == 2)
        {
            if (player2 == null) player2 = player;
            float bonus = maxGauge * ballTouchBonusPercent;
            player2Gauge += bonus;
            player2Gauge = Mathf.Min(player2Gauge, maxGauge);
        }
    }

    void ActivateSuperMode(int playerNumber)
    {
        if (playerNumber == 1 && player1 != null)
        {
            // Call the new method on PlayerController
            player1.ActivateSuperMode(player1SelectedMode, player2);

            player1Gauge = 0f;
            player1LastSuperModeTime = Time.time;
            PlaySuperModeSound();
            Debug.Log($"Player 1 Activated Super Mode: {player1SelectedMode}");
        }
        else if (playerNumber == 2 && player2 != null)
        {
            // Call the new method on PlayerController
            player2.ActivateSuperMode(player2SelectedMode, player1);

            player2Gauge = 0f;
            player2LastSuperModeTime = Time.time;
            PlaySuperModeSound();
            Debug.Log($"Player 2 Activated Super Mode: {player2SelectedMode}");
        }

        // Force the UI to update immediately
        if (GameUI.Instance != null)
        {
            GameUI.Instance.UpdateGaugeUI();
        }
    }

    void PlaySuperModeSound()
    {
        if (superModeActivateSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(superModeActivateSound);
        }
    }

    public float GetPlayer1Gauge() => player1Gauge;
    public float GetPlayer2Gauge() => player2Gauge;
    public float GetMaxGauge() => maxGauge;
    
    // These might still be useful for UI, but they no longer control logic here
    public bool IsPlayer1InSuperMode() => player1 != null && player1.isSuperMode;
    public bool IsPlayer2InSuperMode() => player2 != null && player2.isSuperMode;
}

