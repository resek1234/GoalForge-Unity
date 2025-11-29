using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    // Default values can be set here. These will be overwritten by the selection scene.
    public SuperModeType player1SuperMode = SuperModeType.SuperShot;
    public SuperModeType player2SuperMode = SuperModeType.Freeze;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
