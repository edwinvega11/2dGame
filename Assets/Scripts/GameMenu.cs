using UnityEngine;
using UnityEngine.SceneManagement;

// move between the title screen and the game
public class GameMenu : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Wave Clear");
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
