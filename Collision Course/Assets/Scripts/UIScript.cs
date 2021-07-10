using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [Header("Start countdown")]
    [SerializeField] private int secondsToCountdown = 3;
    [SerializeField] private TMP_Text countdownText;

    [Header("Collectible Displays Area")] 
    [SerializeField] private TMP_Text multiplierText;
    [SerializeField] private TMP_Text multiplierTimerText;
    [SerializeField] private Image shieldIcon;
    [SerializeField] private TMP_Text shieldTimerText;
    [SerializeField] private Image laserIcon;
    [SerializeField] private TMP_Text laserTimerText;

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
    private bool multiplierActive = false;
    private bool lasersActive = false;
    private bool shieldActive = false;

    public bool MultiplierActive => multiplierActive;
    public bool LasersActive => lasersActive;
    public bool ShieldActive => shieldActive;

    private float multiplierTimer;
    private float shieldTimer;
    private float laserTimer;


    private void Start()
    {
        RunCountdownTimer(secondsToCountdown);
        scorer = FindObjectOfType<Scorer>();
        ClearCollectibleDisplays();
    }

    private void FixedUpdate()
    {
        if (lasersActive)
        {
            laserTimer -= Time.deltaTime;
            SetLaserTimerText();
            if (laserTimer <= 0)
            {
                StopLasers();
            }
        }
        if (shieldActive)
        {
            shieldTimer -= Time.deltaTime;
            SetShieldTimerText();
            if (shieldTimer <= 0)
            {
                StopShield();
            }
        }
        if (multiplierActive)
        {
            multiplierTimer -= Time.deltaTime;
            SetMultiplierTimerText();
            if (multiplierTimer <= 0)
            {
                StopMultiplier();
            }
        }
    }

    private void StopMultiplier()
    {
        multiplierActive = false;
        FindObjectOfType<Scorer>().ResetMultiplier();
        ClearMultiplierDisplay();
    }

    private void StopShield()
    {
        shieldActive = false;
        FindObjectOfType<PlayerHealth>().SetShieldActive(shieldActive);
        ClearShieldDisplay();
    }

    private void StopLasers()
    {
        lasersActive = false;
        FindObjectOfType<PlayerHealth>().SetLasersActive(lasersActive);
        ClearLasersDisplay();
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
    /// Method to continually flash HighScore amountText until reset
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

    public void ClearCollectibleDisplays()
    {
        ClearMultiplierDisplay();
        ClearShieldDisplay();
        ClearLasersDisplay();
    }

    private void ClearMultiplierDisplay()
    {
        multiplierText.text = "Multiplier x1";
        multiplierTimerText.text = "";
        multiplierActive = false;
    }

    private void ClearShieldDisplay()
    {
        shieldIcon.color = new Color(1,1,1,0.25f);
        shieldTimerText.text = "";
        shieldActive = false;
    }

    private void ClearLasersDisplay()
    {
        laserIcon.color = new Color(1,1,1,0.25f);
        laserTimerText.text = "";
        lasersActive = false;
    }

    /*public void SetCollectibleDisplay(Sprite collectibleIcon, string collectibleName)
    {
        collectibleIconImage.sprite = collectibleIcon;
        collectibleIconImage.enabled = true;
        multiplierText.text = collectibleName;
        collectibleActive = true;
    }*/
    public void StartMultiplier(int multiplierAmount, float effectTime)
    {
        print($"{multiplierAmount}x Multiplier started for {effectTime} seconds.");
        multiplierActive = true;
        multiplierText.text = $"Multiplier x{multiplierAmount}";
        multiplierTimer = effectTime;
        FindObjectOfType<Scorer>().SetScoreMultiplier(multiplierAmount);
        //SetMultiplierTimerText();
    }

    private void SetMultiplierTimerText()
    {
        multiplierTimerText.text = $"{Mathf.FloorToInt(multiplierTimer):00}";
    }

    public void StartLasers(float effectTime)
    {
        print($"Lasers started for {effectTime} seconds.");
        lasersActive = true;
        laserIcon.color = Color.white;
        laserTimer = effectTime;
        FindObjectOfType<PlayerHealth>().SetLasersActive(lasersActive);
        //SetLaserTimerText();
    }

    private void SetLaserTimerText()
    {
        laserTimerText.text = $"{Mathf.FloorToInt(laserTimer):00}";
    }

    public void StartShield(float effectTime)
    {
        print($"Shield started for {effectTime} seconds.");
        shieldActive = true;
        shieldIcon.color = Color.white;
        shieldTimer = effectTime;
        FindObjectOfType<PlayerHealth>().SetShieldActive(shieldActive);
        //SetShieldTimerText();
    }

    private void SetShieldTimerText()
    {
        shieldTimerText.text = $"{Mathf.FloorToInt(shieldTimer):00}";
    }
}
