using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject gameOverScreen;

    [Header("Start countdown")]
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private int secondsToCountdown = 3;

    public static bool IsRunning = false;

    private void Start()
    {
        RunCountdownTimer(secondsToCountdown);
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

    public void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        IsRunning = false;
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
