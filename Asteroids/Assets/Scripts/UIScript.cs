using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [Header("Start countdown")]
    [SerializeField] private int secondsToCountdown = 3;
    [SerializeField] private TMP_Text countdownText;

    [Header("Overlay components")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text scoreDisplay;

    [Header("Game over components")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TMP_Text highscoreDisplay;

    public static bool IsRunning = false;
    private Scorer scorer;

    private void Start()
    {
        RunCountdownTimer(secondsToCountdown);
        scorer = FindObjectOfType<Scorer>();
        //SetScoreDisplay(scorer.Score);
    }

    /// <summary>
    /// Run countdown timer to start game
    /// </summary>
    /// <param name="seconds"></param>
    private void RunCountdownTimer(int seconds)
    {
        IsRunning = false;
        StartCoroutine(CountdownTimer(seconds));
    }

    IEnumerator CountdownTimer(int seconds)
    {
        int countdown = seconds;
        countdownText.text = countdown.ToString();
        while (countdown > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            countdown--;
            countdownText.text = countdown.ToString();
        }

        countdownText.text = "GO!";

        IsRunning = true;
        yield return new WaitForSecondsRealtime(1);

        countdownText.text = "";
    }

    /// <summary>
    /// Set healthbar to players current health percentage.
    /// </summary>
    /// <param name="currentHealthPercentage">current health percentage (0-1)</param>
    public void SetHealthBarValue(float currentHealthPercentage)
    {
        healthBar.value = currentHealthPercentage;
    }

    /// <summary>
    /// Stop player controls and spawning then show game over screen.
    /// </summary>
    public void ShowGameOverScreen()
    {
        IsRunning = false;
        gameOverScreen.SetActive(true);
        int score = (int)scorer.Score;
        bool isHighScore = scorer.CheckHighScore();
        int highScore = PlayerPrefs.GetInt(Scorer.HighscoreKey, 0);
        if (isHighScore)
        {
            highscoreDisplay.text = $"Score - {score:0000000000}\n" +
                                    $"New HighScore! - {highScore:0000000000}";
            StartCoroutine(FlashHighscoreText());
        }
        else
        {
            highscoreDisplay.text = $"Score - {score:0000000000}\n" +
                                    $"HighScore - {highScore:0000000000}";
        }
    }

    /// <summary>
    /// Method to continually flash HighScore text until reset
    /// </summary>
    IEnumerator FlashHighscoreText()
    {
        while (!IsRunning)
        {
            highscoreDisplay.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            highscoreDisplay.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
        }
        highscoreDisplay.gameObject.SetActive(true);
    }

    public void SetScoreDisplay(int score)
    {
        scoreDisplay.text = $"Score\n{score:0000000000}";
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Continue()
    {
        print("Show Advert");
    }
}
