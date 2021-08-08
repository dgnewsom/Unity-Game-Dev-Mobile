using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private TMP_Text highscoreText;

    [SerializeField] private GameObject mainmenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject tutorialPanel;

    /*
    void Start()
    {
        ShowMainMenuPanel();
    }*/

    private void OnEnable()
    {
        ShowMainMenuPanel();
        
    }

    public void ShowMainMenuPanel()
    {
        HideAllPanels();
        mainmenuPanel.SetActive(true);
        highscoreText.text = $"HighScore\n " +
                             $"Level {PlayerPrefs.GetInt(Scorer.LevelHighscoreKey,0)} - {PlayerPrefs.GetInt(Scorer.HighscoreKey,0):0000000000}";
    }

    public void ShowSettingsMenu()
    {
        HideAllPanels();
        settingsPanel.SetActive(true);
    }

    public void ShowTutorialPanel()
    {
        HideAllPanels();
        tutorialPanel.SetActive(true);
    }

    private void HideAllPanels()
    {
        mainmenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        tutorialPanel.SetActive(false);
    }

    public void StartGame()
    {
        if (PlayerPrefs.GetInt("FirstPlay", 1).Equals(1))
        {
            ShowTutorialPanel();
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public void ResetHighscore()
    {
        PlayerPrefs.SetInt(Scorer.LevelHighscoreKey,0);
        PlayerPrefs.SetInt(Scorer.HighscoreKey,0);
    }

    public void QuitGame()
    {
        Application.Quit(0);
    }
}
