using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private TMP_Text highscoreText;

    private void OnEnable()
    {
        highscoreText.text = $"HighScore\n " +
                             $"Level {PlayerPrefs.GetInt(Scorer.LevelHighscoreKey,0)} - {PlayerPrefs.GetInt(Scorer.HighscoreKey,0):0000000000}";
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit(0);
    }
}
