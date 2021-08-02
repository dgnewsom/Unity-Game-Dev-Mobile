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

    [Header("Collectible Display Areas")] 
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

    [Header("Level Display")] 
    [SerializeField] private TMP_Text currentLevelText;
    [SerializeField] private TMP_Text nextLevelText;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject pauseButton;


    [SerializeField] private bool TestMode;

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
        if (TestMode) {return;}
        /*
         * Check if collectible timers active or finished
         */
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

    /// <summary>
    /// Run countdown timer to start game
    /// </summary>
    /// <param name="seconds"></param>
    private void RunCountdownTimer(int seconds)
    {
        IsRunning = false;
        pauseButton.SetActive(false);
        StartCoroutine(CountdownTimer(seconds));
    }

    /// <summary>
    /// Coroutine for the start countdown display
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    IEnumerator CountdownTimer(int seconds)
    {

        int countdown = seconds;
        countdownText.text = countdown.ToString();
        while (countdown > 0)
        {
            SoundManager.Instance.PlayCountdownBeepSound();
            yield return new WaitForSecondsRealtime(1);
            countdown--;
            countdownText.text = countdown.ToString();
            
        }

        SoundManager.Instance.PlayStartBeepSound();
        countdownText.text = "GO!";
        pauseButton.SetActive(true);

        IsRunning = true;
        yield return new WaitForSecondsRealtime(1);

        countdownText.text = "";
    }

    /// <summary>
    /// Update the score display
    /// </summary>
    /// <param name="score"></param>
    public void SetScoreDisplay(int score)
    {
        scoreDisplay.text = $"Score\n{score:0000000000}";
    }

    /// <summary>
    /// Set HealthBar to players current health percentage.
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
        int level = scorer.Level;
        bool isHighScore = scorer.CheckHighScore();
        int highScore = PlayerPrefs.GetInt(Scorer.HighscoreKey, 0);
        if (isHighScore)
        {
            highscoreDisplay.text = $"Score - {score:0000000000}\n" +
                                    $"New HighScore!\n " +
                                    $"Level {level} - {highScore:0000000000}";
            StartCoroutine(FlashHighscoreText());
        }
        else
        {
            highscoreDisplay.text = $"Score - {score:0000000000}\n" +
                                    $"HighScore\n " +
                                    $"Level {level} - {highScore:0000000000}";
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

    /// <summary>
    /// Continue game with current score
    /// </summary>
    public void ContinueGame()
    {
        Scorer.ReduceContinues();
        FindObjectOfType<ObjectSpawner>().Continue();
        gameOverScreen.SetActive(false);
        RunCountdownTimer(secondsToCountdown);
        FindObjectOfType<PlayerMovement>().ResetPlayerMovement();
        FindObjectOfType<PlayerHealth>().ResetPlayerHealth();
        ClearCollectibleDisplays();
    }

    /// <summary>
    /// Reload the Scene
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Load Main menu scene
    /// </summary>
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Methods called by buttons - Dont delete!
    /// </summary>
    public void ShowRestartAd()
    {
        Time.timeScale = 1f;
        AdManager.Instance.ShowAd(this,AdManager.restartAdString);
    }

    public void ShowContinueAd()
    {
        Time.timeScale = 1f;
        AdManager.Instance.ShowAd(this,AdManager.continueAdString);
    }

    public void ShowMainMenuAd()
    {
        Time.timeScale = 1f;
        AdManager.Instance.ShowAd(this,AdManager.mainMenuAdString);
    }

    /// <summary>
    /// Clears all collectibles on start / restart
    /// </summary>
    private void ClearCollectibleDisplays()
    {
        StopMultiplier();
        ClearMultiplierDisplay();
        StopLasers();
        ClearLasersDisplay();
        StopShield(false);
        ClearShieldDisplay();
    }

    /*
     * Methods to Deal with collectibles
     */

    /// <summary>
    /// Start multiplier collectible
    /// </summary>
    /// <param name="multiplierAmount">Amount to set score multiplier to</param>
    /// <param name="effectTime">Time to set multiplier for</param>
    public void StartMultiplier(int multiplierAmount, float effectTime)
    {
        //Set score multiplier on scorer script
        FindObjectOfType<Scorer>().SetScoreMultiplier(multiplierAmount);

        //Set timer value and texts
        multiplierText.text = $"Multiplier x{GetComponentInChildren<Scorer>().GetCurrentMultiplier()}";
        multiplierTimer += effectTime;

        //Enable timer
        multiplierActive = true;
    }

    /// <summary>
    /// Start Shield collectible
    /// </summary>
    /// <param name="effectTime">Time to set Shield active for</param>
    public void StartShield(float effectTime)
    {
        //Play shield sound
        SoundManager.Instance.PlayShieldPickupSound();

        //Set timer value and icon visibility
        shieldIcon.color = Color.white;
        shieldTimer += effectTime;
        
        //Enable timer
        shieldActive = true;

        //Activate shield on player
        FindObjectOfType<PlayerHealth>().SetShieldActive(shieldActive);
    }

    /// <summary>
    /// Start Laser collectible
    /// </summary>
    /// <param name="effectTime">Time to set Lasers active for</param>
    public void StartLasers(float effectTime)
    {
        //Play Laser charge sound
        SoundManager.Instance.PlayLaserChargeSound();

        //Set timer value and icon visibility
        laserIcon.color = Color.white;
        laserTimer += effectTime;

        //Enable timer
        lasersActive = true;

        //Activate lasers on player
        FindObjectOfType<PlayerHealth>().SetLasersActive(lasersActive);
    }

    /// <summary>
    /// Resets Multiplier ui components
    /// </summary>
    private void ClearMultiplierDisplay()
    {
        multiplierText.text = "Multiplier x1";
        multiplierTimerText.text = "";
        multiplierActive = false;
    }

    /// <summary>
    /// Resets shield ui components
    /// </summary>
    private void ClearShieldDisplay()
    {
        shieldIcon.color = new Color(1,1,1,0.25f);
        shieldTimerText.text = "";
        shieldActive = false;
    }

    /// <summary>
    /// Resets Laser ui components
    /// </summary>
    private void ClearLasersDisplay()
    {
        laserIcon.color = new Color(1,1,1,0.25f);
        laserTimerText.text = "";
        lasersActive = false;
    }

    /// <summary>
    /// Update the multiplier timer text
    /// </summary>
    private void SetMultiplierTimerText()
    {
        multiplierTimerText.text = $"{Mathf.FloorToInt(multiplierTimer):00}";
    }

    /// <summary>
    /// Update the Laser timer text
    /// </summary>
    private void SetLaserTimerText()
    {
        laserTimerText.text = $"{Mathf.FloorToInt(laserTimer):00}";
    }

    /// <summary>
    /// Update the Laser timer text
    /// </summary>
    private void SetShieldTimerText()
    {
        shieldTimerText.text = $"{Mathf.FloorToInt(shieldTimer):00}";
    }

    /// <summary>
    /// Reset Score Multiplier
    /// </summary>
    private void StopMultiplier()
    {
        multiplierActive = false;
        FindObjectOfType<Scorer>().ResetMultiplier();
        ClearMultiplierDisplay();
    }

    /// <summary>
    /// Disable shield collectible
    /// </summary>
    private void StopShield(bool playSound = true)
    {
        if (playSound)
        {
            SoundManager.Instance.PlayShieldOffSound();
        }
        shieldActive = false;
        FindObjectOfType<PlayerHealth>().SetShieldActive(shieldActive);
        ClearShieldDisplay();
    }

    /// <summary>
    /// Disable laser collectible
    /// </summary>
    private void StopLasers()
    {
        lasersActive = false;
        FindObjectOfType<PlayerHealth>().SetLasersActive(lasersActive);
        SoundManager.Instance.StopLasersSound();
        ClearLasersDisplay();
    }

    public void SetLevelText(int currentLevel, int nextLevel)
    {
        currentLevelText.text = $"Level {currentLevel}";
        nextLevelText.text = $"Next level - {nextLevel:#,###0}";
    }

    public void StopAllPickups()
    {
        StopLasers();
        StopShield(false);
        StopMultiplier();
    }

    public void Pause()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }
}
