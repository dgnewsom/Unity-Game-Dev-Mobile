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

    [Header("Collectible Active Area")] 
    [SerializeField] private Image collectibleIconImage;
    [SerializeField] private TMP_Text collectibleNameText;
    [SerializeField] private TMP_Text collectibleTimerText;

    [Header("Overlay components")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text scoreDisplay;

    [Header("Game over components")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TMP_Text highscoreDisplay;
    [SerializeField] private Button continueButton;
    [SerializeField] private TMP_Text continueButtonText;

    public static bool IsRunning = false;
    private Scorer scorer;
    private static bool collectibleActive = false;

    public static bool CollectibleActive => collectibleActive;

    private void Start()
    {
        RunCountdownTimer(secondsToCountdown);
        scorer = FindObjectOfType<Scorer>();
        ClearCollectibleDisplay();
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

        continueButtonText.text = $"Continue ( {Scorer.ContinuesRemaining} )";
        if (Scorer.ContinuesRemaining <= 0)
        {
            continueButton.interactable = false;
            continueButtonText.color = Color.grey;
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

    public void ShowRestartAd()
    {
        AdManager.Instance.ShowAd(this,AdManager.restartAdString);
    }

    public void ShowContinueAd()
    {
        AdManager.Instance.ShowAd(this,AdManager.continueAdString);
    }
    public void ShowMainMenuAd()
    {
        AdManager.Instance.ShowAd(this,AdManager.mainMenuAdString);
    }

    public void ContinueGame()
    {
        Scorer.ReduceContinues();
        gameOverScreen.SetActive(false);
        RunCountdownTimer(secondsToCountdown);
        FindObjectOfType<PlayerMovement>().ResetPlayerMovement();
        FindObjectOfType<PlayerHealth>().ResetPlayerHealth();
    }

    public void SetCollectibleTimerDisplay(float timer)
    {
        collectibleTimerText.text = $"{timer:00.0}";
    }

    public void ClearCollectibleDisplay()
    {
        collectibleIconImage.enabled = false;
        collectibleIconImage.sprite = null;
        collectibleNameText.text = "";
        collectibleTimerText.text = "";
        collectibleActive = false;
    }

    public void SetCollectibleDisplay(Sprite collectibleIcon, string collectibleName)
    {
        collectibleIconImage.sprite = collectibleIcon;
        collectibleIconImage.enabled = true;
        collectibleNameText.text = collectibleName;
        collectibleActive = true;
    }
}
