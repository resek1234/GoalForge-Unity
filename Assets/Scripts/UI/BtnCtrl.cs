using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnCtrl : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button manualButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button titleButton;

    void Start()
    {
        // Connect buttons if assigned
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnClickStartGame);
            Debug.Log("[BtnCtrl] StartButton connected");
        }
        
        if (manualButton != null)
        {
            manualButton.onClick.AddListener(OnClickManual);
            Debug.Log("[BtnCtrl] ManualButton connected");
        }
        
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnClickExit);
            Debug.Log("[BtnCtrl] ExitButton connected");
        }
        
        if (titleButton != null)
        {
            titleButton.onClick.AddListener(OnClickTitle);
            Debug.Log("[BtnCtrl] TitleButton connected");
        }
    }

	    // Start Game: Load MainScene
	    public void OnClickStartGame()
	    {
	        if (SoundManager.Instance != null)
	        {
	            // WebGL autoplay 정책을 우회하기 위해
	            // 사용자의 첫 클릭 시점에 BGM을 시작하도록 한다.
	            SoundManager.Instance.PlayButtonClickSound();
	            SoundManager.Instance.PlayBGM();
	        }
	        Debug.Log("[BtnCtrl] OnClickStartGame called");
	        SceneManager.LoadScene("MainScene");
	    }

	    // How To Play: Load ManualScene to explain rules
	    public void OnClickManual()
	    {
	        if (SoundManager.Instance != null)
	        {
	            SoundManager.Instance.PlayButtonClickSound();
	            SoundManager.Instance.PlayBGM();
	        }
	        Debug.Log("[BtnCtrl] OnClickManual called");
	        SceneManager.LoadScene("ManualScene");
	    }

	    // Back to Title: Load TitleScene (used in ManualScene)
	    public void OnClickTitle()
	    {
	        if (SoundManager.Instance != null)
	        {
	            SoundManager.Instance.PlayButtonClickSound();
	            SoundManager.Instance.PlayBGM();
	        }
	        Debug.Log("[BtnCtrl] OnClickTitle called");
	        SceneManager.LoadScene("TitleScene");
	    }

    // Exit Game
    public void OnClickExit()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayButtonClickSound();
        }
        Debug.Log("[BtnCtrl] OnClickExit called - Quitting application");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
