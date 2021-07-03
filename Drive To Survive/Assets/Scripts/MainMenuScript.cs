using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);
        if (PlayerPrefs.GetInt(FirstLoadKey) == 1)
        {
            UpdateRechargedEnergy();
        }

        PlayerPrefs.SetInt(EnergyKey,energy);
        notificationHandler = GetComponent<AndroidNotificationHandler>();
        SetHighscoreText();
        SetEnergyText();
    }

    private void FixedUpdate()
    {
        energy = PlayerPrefs.GetInt(EnergyKey);
        startButton.interactable = (energy > 0);
        SetRechargeStatus();

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

    private void UpdateRechargedEnergy()
    {
        string energyFullTime = PlayerPrefs.GetString(EnergyFullKey,String.Empty);
        
        if (!energyFullTime.Equals(String.Empty) && DateTime.Now > DateTime.Parse(energyFullTime))
        {
            energy = maxEnergy;
            PlayerPrefs.SetInt(EnergyKey, maxEnergy);
            ClearRechargeTime();
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

        PlayerPrefs.SetInt(FirstLoadKey,0);
        /*string energyFullTimeString = PlayerPrefs.GetString(EnergyRechargeKey,String.Empty);
        if (!energyFullTimeString.Equals(string.Empty) && DateTime.Now > DateTime.Parse(energyFullTimeString))
        {
            energy = maxEnergy;
            PlayerPrefs.SetInt(EnergyKey,maxEnergy);
            ClearRechargeTime();
        }
        for (int i = 1; i <= maxEnergy; i++)
        {
            string rechargeTimeString = PlayerPrefs.GetString($"Recharge Energy {i}",string.Empty);
            if (rechargeTimeString.Equals(string.Empty))
            {
                if (i > 1)
                {
                    PlayerPrefs.SetString(EnergyRechargeKey,PlayerPrefs.GetString($"Recharge Energy{i-1}"));
                }
                return;
            }
            DateTime rechargeTime = DateTime.Parse(rechargeTimeString);
            if (DateTime.Now > rechargeTime)
            {
                RechargeEnergy(false);
            }
        }*/
    }

    private void SetHighscoreText()
    {
        int highScore = PlayerPrefs.GetInt(ScoreSystem.highScoreKey, 0);
        highscoreText.text = $"HighScore - {highScore}";
    }

    private void SetEnergyText()
    {
        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);
        energyText.text = $"{energy}";
    }

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

    public void StartGame()
    {
        energy--;
        PlayerPrefs.SetInt(EnergyKey,energy);
        if (PlayerPrefs.GetString(EnergyRechargeKey, String.Empty).Equals(String.Empty))
        {
            ResetRechargeTime();
        }
        SetEnergyText();
        SceneManager.LoadScene(1);
    }

    private void ResetRechargeTime()
    {
        DateTime rechargeTime = DateTime.Now.AddMinutes(energyRechargeTimeInMinutes);
        PlayerPrefs.SetString(EnergyRechargeKey, rechargeTime.ToString());
    }

    private void ClearRechargeTime()
    {
        PlayerPrefs.SetString(EnergyRechargeKey, String.Empty);
        PlayerPrefs.SetString(EnergyFullKey, String.Empty);
        AndroidNotificationCenter.CancelAllNotifications();
    }

    private void ResetNotification(DateTime rechargeTime)
    {
#if UNITY_ANDROID
        AndroidNotificationCenter.CancelAllNotifications();
        notificationHandler.ScheduleNotification(rechargeTime);
#endif
    }

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

    public void ShowAd()
    {
        FindObjectOfType<AdManagerScript>().ShowAd(this);
    }

    public void OptionsButton()
    {
        print("Options Menu");
    }

    public void QuitGame()
    {
        /*SetRechargeTimes();*/
        Application.Quit(0);
    }

    public void OnApplicationQuit()
    {
        SetRechargeTimes();
    }

    private void SetRechargeTimes(bool onExit = true)
    {
        for (int i = 2; i <= maxEnergy; i++)
        {
            PlayerPrefs.SetString($"Recharge Energy {i}",String.Empty);
        }

        int numberOfMissingEnergy = maxEnergy - energy;
        if (numberOfMissingEnergy == 0)
        {
            PlayerPrefs.SetString(EnergyFullKey, string.Empty);
            AndroidNotificationCenter.CancelAllNotifications();
        }

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

        if (onExit)
        {
            PlayerPrefs.SetInt(FirstLoadKey,1);
        }
    }

    public void ClearHighScore()
    {
        int highscore = 0;
        PlayerPrefs.SetInt(ScoreSystem.highScoreKey,highscore);
        highscoreText.text = $"HighScore - {highscore}";
    }
}
