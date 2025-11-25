using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Using TextMeshPro for modern UI text

public class CharacterSelectUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI player1ModeText;
    [SerializeField] private TextMeshProUGUI player2ModeText;

    [Header("Scene Settings")]
    [SerializeField] private string mainGameSceneName = "MainScene";

    private GameSettings gameSettings;

    void Awake()
    {
        EnsureGameSettings();
    }

    void Start()
    {
        // Set initial selections and update UI
        if (gameSettings != null)
        {
            // Set defaults in case nothing is selected yet
            gameSettings.player1SuperMode = SuperModeType.SuperShot;
            gameSettings.player2SuperMode = SuperModeType.SuperShot;
            UpdateUI();
        }
    }

    private void EnsureGameSettings()
    {
        // Find GameSettings in the scene
        gameSettings = FindObjectOfType<GameSettings>();

        // If it doesn't exist, create it
        if (gameSettings == null)
        {
            GameObject gameSettingsObject = new GameObject("GameSettings");
            gameSettings = gameSettingsObject.AddComponent<GameSettings>();
            Debug.Log("GameSettings object created automatically.");
        }
    }

    public void SelectP1SuperMode(int modeIndex)
    {
        if (gameSettings == null) return;

        SuperModeType selectedMode = (SuperModeType)modeIndex;
        gameSettings.player1SuperMode = selectedMode;
        Debug.Log($"Player 1 selected: {selectedMode}");
        UpdateUI();
    }

    public void SelectP2SuperMode(int modeIndex)
    {
        if (gameSettings == null) return;

        SuperModeType selectedMode = (SuperModeType)modeIndex;
        gameSettings.player2SuperMode = selectedMode;
        Debug.Log($"Player 2 selected: {selectedMode}");
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (gameSettings == null) return;

        if (player1ModeText != null)
        {
            player1ModeText.text = $"P1 Mode: {gameSettings.player1SuperMode}";
        }

        if (player2ModeText != null)
        {
            player2ModeText.text = $"P2 Mode: {gameSettings.player2SuperMode}";
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(mainGameSceneName);
    }
}
