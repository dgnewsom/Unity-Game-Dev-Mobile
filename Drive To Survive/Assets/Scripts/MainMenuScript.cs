using System;
using System.Collections.Generic;
using TMPro;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class to handle main menu interaction
/// </summary>
public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private TMP_Text highscoreText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text energyReadyText;
    [SerializeField] private int maxEnergy;
    [SerializeField] private int energyRechargeTimeInMinutes = 15;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject rechargeButton;
    

    private AndroidNotificationHandler notificationHandler;
    private const string EnergyKey = "Energy";
    private const string EnergyRechargeKey = "Recharge Energy 1";
    private const string EnergyFullKey = "Energy Full";
    private const string FirstLoadKey = "FirstLoad";
    private List<DateTime> rechargeTimes;
    private int energy;

    public void Start()
    {
        notificationHandler = GetComponent<AndroidNotificationHandler>();
        
        //Set energy if previously saved.
        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);

        //If game just loaded then update energy level from stored recharge times
        if (PlayerPrefs.GetInt(FirstLoadKey) == 1)
        {
            UpdateRechargedEnergy();
        }

        //Re-save updated energy level
        PlayerPrefs.SetInt(EnergyKey,energy);

        //Set text display values
        SetHighscoreText();
        SetEnergyText();
    }

    private void FixedUpdate()
    {
        //Reset Energy recharge status
        energy = PlayerPrefs.GetInt(EnergyKey);
        startButton.interactable = (energy > 0);
        SetRechargeStatus();

        //Recharge if next recharge time reached
        string energyReadyString = PlayerPrefs.GetString(EnergyRechargeKey, string.Empty);
        if (!energyReadyString.Equals(String.Empty))
        {
            DateTime energyReady = DateTime.Parse(energyReadyString);

            if (DateTime.Now > energyReady)
            {
                if (PlayerPrefs.GetInt(FirstLoadKey,1) == 1) {return;}
                RechargeEnergy(true);
            }
        }
    }

    /// <summary>
    /// Set highscore display text
    /// </summary>
    private void SetHighscoreText()
    {
        int highScore = PlayerPrefs.GetInt(ScoreSystem.HighScoreKey, 0);
        highscoreText.text = $"HighScore - {highScore}";
    }

    /// <summary>
    /// Set energy counter text
    /// </summary>
    private void SetEnergyText()
    {
        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);
        energyText.text = $"{energy}";
    }

    /// <summary>
    /// Set recharge button visibility and recharge time text.
    /// </summary>
    private void SetRechargeStatus()
    {
        if (PlayerPrefs.GetString(EnergyRechargeKey, String.Empty).Equals(String.Empty))
        {
            rechargeButton.SetActive(false);
            energyReadyText.text = "";
        }
        else
        {
            rechargeButton.SetActive(true);
            string energyReadyString = PlayerPrefs.GetString(EnergyRechargeKey, string.Empty);

            DateTime energyReady = DateTime.Parse(energyReadyString);
            TimeSpan timeUntilRecharge = energyReady - DateTime.Now;
            energyReadyText.text = $"Recharge in\n" +
                                   $"{timeUntilRecharge.Minutes} Minutes\n" +
                                   $"{timeUntilRecharge.Seconds} Seconds";
        }
    }

    /// <summary>
    /// Decrement energy, reset recharge times and load main game.
    /// </summary>
    public void StartGame()
    {
        energy--;
        PlayerPrefs.SetInt(EnergyKey,energy);
        if (PlayerPrefs.GetString(EnergyRechargeKey, String.Empty).Equals(String.Empty))
        {
            ResetRechargeTime();
        }
        SetEnergyText();
        SetRechargeTimes();
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Reset next recharge time.
    /// </summary>
    private void ResetRechargeTime()
    {
        DateTime rechargeTime = DateTime.Now.AddMinutes(energyRechargeTimeInMinutes);
        PlayerPrefs.SetString(EnergyRechargeKey, rechargeTime.ToString());
    }

    /// <summary>
    /// Clear recharge time and clear notification
    /// </summary>
    private void ClearRechargeTime()
    {
        PlayerPrefs.SetString(EnergyRechargeKey, String.Empty);
        PlayerPrefs.SetString(EnergyFullKey, String.Empty);
        AndroidNotificationCenter.CancelAllNotifications();
    }

    /// <summary>
    /// Clear and reset notification
    /// </summary>
    /// <param name="rechargeTime"> The time to display notification</param>
    private void ResetNotification(DateTime rechargeTime)
    {
#if UNITY_ANDROID
        AndroidNotificationCenter.CancelAllNotifications();
        notificationHandler.ScheduleNotification(rechargeTime);
#endif
    }
    
    /// <summary>
    /// Recharge energy by 1 if below max energy then recalculate recharge times.
    /// </summary>
    public void RechargeEnergy(bool reset = true)
    {
        
        if(energy < maxEnergy)
        {
            energy++;
            PlayerPrefs.SetInt(EnergyKey,energy);
        }

        if (energy >= maxEnergy)
        {
            ClearRechargeTime();
        }
        else
        {
            if (reset)
            {
                ResetRechargeTime();
            }
        }
        SetEnergyText();
        SetRechargeTimes(false);
    }

    /// <summary>
    /// Used by recharge button to display and advert.
    /// </summary>
    public void ShowAd()
    {
        FindObjectOfType<AdManagerScript>().ShowAd(this);
    }
    
    /// <summary>
    /// Quit game cleanly
    /// </summary>
    public void QuitGame()
    {
        Application.Quit(0);
    }
    
    /// <summary>
    /// Set recharge times on exit
    /// </summary>
    public void OnApplicationQuit()
    {
        SetRechargeTimes();
    }

    /// <summary>
    /// Clears the high score from player prefs.
    /// </summary>
    public void ClearHighScore()
    {
        int highscore = 0;
        PlayerPrefs.SetInt(ScoreSystem.HighScoreKey,highscore);
        highscoreText.text = $"HighScore - {highscore}";
    }

    /// <summary>
    /// Recharge energy for each recharge time passed whilst application was closed.
    /// </summary>
    private void UpdateRechargedEnergy()
    {
        /*
         * If energy full time passed reset energy to max and return
         * if not then recharge for each time passed and reset next
         * recharge time to next time in the future.
         */
        string energyFullTime = PlayerPrefs.GetString(EnergyFullKey,String.Empty);
        if (!energyFullTime.Equals(String.Empty) && DateTime.Now > DateTime.Parse(energyFullTime))
        {
            energy = maxEnergy;
            PlayerPrefs.SetInt(EnergyKey, maxEnergy);
            ClearRechargeTime();
            return;
        }
        else
        {
            rechargeTimes = new List<DateTime>();
            print("Updated energy on load");
            for (int i = 1; i <= maxEnergy; i++)
            {
                string rechargeTimeString = PlayerPrefs.GetString($"Recharge Energy {i}",String.Empty);
                if (!rechargeTimeString.Equals(string.Empty))
                {
                    DateTime rechargeTime = DateTime.Parse(rechargeTimeString);
                    rechargeTimes.Add(rechargeTime);
                    if (DateTime.Now > rechargeTime)
                    {
                        PlayerPrefs.SetString($"Recharge Energy {i}",String.Empty);
                    }

                }
            }

            List<DateTime> remainingTimes = new List<DateTime>();
        
            foreach (DateTime rechargeTime in rechargeTimes)
            {
                if (DateTime.Now < rechargeTime)
                {
                    /*print(rechargeTime);*/
                    remainingTimes.Add(rechargeTime);
                }
                else
                {
                    RechargeEnergy(false);
                }
            
            }

            remainingTimes.Sort();
            PlayerPrefs.SetString(EnergyRechargeKey,String.Empty);
            foreach (DateTime rechargeTime in remainingTimes)
            {
                if (rechargeTime > DateTime.Now)
                {
                    PlayerPrefs.SetString(EnergyRechargeKey,rechargeTime.ToString());
                    break;
                }
            }
        }

        //Clear reload trigger
        PlayerPrefs.SetInt(FirstLoadKey,0);
        
    }

    /// <summary>
    /// Recalculates energy recharge times and energy full time and stores in PlayerPrefs.
    /// Sets android notification for lives full time.
    /// </summary>
    private void SetRechargeTimes(bool onExit = true)
    {
        //Set all recharge times to empty string
        for (int i = 2; i <= maxEnergy; i++)
        {
            PlayerPrefs.SetString($"Recharge Energy {i}",String.Empty);
        }

        //If energy is full then cancel notifications and clear energy full time
        int numberOfMissingEnergy = maxEnergy - energy;
        if (numberOfMissingEnergy == 0)
        {
            PlayerPrefs.SetString(EnergyFullKey, string.Empty);
            AndroidNotificationCenter.CancelAllNotifications();
        }

        /*
        / If energy missing, set recharge times from next recharge time and at intervals
        / of energy recharge time for each missing energy.
        / Set energy full time and notification to the latest recharge time.
        */

        if (numberOfMissingEnergy >= 1)
        {
            if (!PlayerPrefs.GetString(EnergyRechargeKey).Equals(String.Empty))
            {
                for (int i = 2; i <= numberOfMissingEnergy; i++)
                {
                    DateTime previousRecharge = DateTime.Parse(PlayerPrefs.GetString($"Recharge Energy {i-1}"));
                    string rechargeKey = $"Recharge Energy {i}";
                    DateTime rechargeTime = previousRecharge.AddMinutes(energyRechargeTimeInMinutes);
                    PlayerPrefs.SetString(rechargeKey, rechargeTime.ToString());
                    if (i == numberOfMissingEnergy)
                    {
                        PlayerPrefs.SetString(EnergyFullKey, rechargeTime.ToString());
                        ResetNotification(rechargeTime);
                    }
                }
            }
        }

        // If application quitting set trigger to recalculate on next load.
        if (onExit)
        {
            PlayerPrefs.SetInt(FirstLoadKey,1);
        }
    }
}
