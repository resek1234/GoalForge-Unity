using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnCtrl : MonoBehaviour
{
    // Start Game: Load MainScene
    public void OnClickStartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    // How To Play: Load ManualScene to explain rules
    public void OnClickManual()
    {
        SceneManager.LoadScene("ManualScene");
    }

    // Back to Title: Load TitleScene (used in ManualScene)
    public void OnClickTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    // Exit Game
    public void OnClickExit()
    {
        Debug.Log("Exit Game");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
