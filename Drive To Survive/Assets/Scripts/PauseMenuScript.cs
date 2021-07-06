using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Basic pause menu script
/// </summary>
public class PauseMenuScript : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void ResumeButton()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

}
