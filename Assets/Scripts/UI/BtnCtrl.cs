using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnCtrl : MonoBehaviour
{
    void Start()
    {
        // Auto-connect buttons by name
        ConnectButton("StartButton", OnClickStartGame);
        ConnectButton("ManualButton", OnClickManual);
        ConnectButton("ExitButton", OnClickExit);
        ConnectButton("TitleButton", OnClickTitle);
    }

    void ConnectButton(string buttonName, UnityEngine.Events.UnityAction action)
    {
        GameObject buttonObj = GameObject.Find(buttonName);
        if (buttonObj != null)
        {
            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(action);
                Debug.Log($"[BtnCtrl] Connected {buttonName}");
            }
        }
    }

    // Start Game: Load MainScene
    public void OnClickStartGame()
    {
        Debug.Log("[BtnCtrl] OnClickStartGame called");
        SceneManager.LoadScene("MainScene");
    }

    // How To Play: Load ManualScene to explain rules
    public void OnClickManual()
    {
        Debug.Log("[BtnCtrl] OnClickManual called");
        SceneManager.LoadScene("ManualScene");
    }

    // Back to Title: Load TitleScene (used in ManualScene)
    public void OnClickTitle()
    {
        Debug.Log("[BtnCtrl] OnClickTitle called");
        SceneManager.LoadScene("TitleScene");
    }

    // Exit Game
    public void OnClickExit()
    {
        Debug.Log("[BtnCtrl] OnClickExit called - Quitting application");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
