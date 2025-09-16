using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Settings")]
    public float playDelay = 0.5f; // delay before loading scene

    public void PlayGame()
    {
        StartCoroutine(LoadWithDelay("Intro"));
    }

    private System.Collections.IEnumerator LoadWithDelay(string sceneName)
    {
        // Wait in real time (ignores Time.timeScale)
        yield return new WaitForSecondsRealtime(playDelay);

        // Always reset time before entering a new scene
        Time.timeScale = 1f;

        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Reset time scale to avoid "frozen" UI
        SceneManager.LoadScene("Main Menu"); // replace with your title scene name
    }
}
