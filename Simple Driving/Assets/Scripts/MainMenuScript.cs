using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private TMP_Text highscoreText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text energyReadyText;
    [SerializeField] private int maxEnergy;
    [SerializeField] private int energyRechargeTime;
    [SerializeField] private Button goButton;
    [SerializeField] private GameObject rechargeButton;

    private const string EnergyKey = "Energy";
    private const string EnergyReadyKey = "EnergyReady";
    private int energy;

    public void Start()
    {
        int highScore = PlayerPrefs.GetInt(ScoreSystem.highScoreKey,0);
        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);
        highscoreText.text = $"HighScore - {highScore}";
        energyText.text = $"{energy}";
        rechargeButton.SetActive(false);
    }

    private void FixedUpdate()
    {
        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);

        if (energy == 0)
        {
            goButton.interactable = false;
            rechargeButton.SetActive(true);
            string energyReadyString = PlayerPrefs.GetString(EnergyReadyKey, string.Empty);
            if (energyReadyString.Equals(String.Empty)){return;}

            DateTime energyReady = DateTime.Parse(energyReadyString);
            if (DateTime.Now > energyReady)
            {
                RechargeEnergy();
            }

            SetEnergyReadyText();
        }
    }

    private void SetEnergyReadyText()
    {
        if (energy == 0)
        {
            string energyReadyString = PlayerPrefs.GetString(EnergyReadyKey, string.Empty);
            if (energyReadyString.Equals(String.Empty)){return;}

            DateTime energyReady = DateTime.Parse(energyReadyString);
            TimeSpan timeUntilRecharge = energyReady - DateTime.Now;
            energyReadyText.text = $"Recharge in\n" +
                                   $"{timeUntilRecharge.Minutes} Minutes\n" +
                                   $"{timeUntilRecharge.Seconds} Seconds";
            return;
        }
        energyReadyText.text = String.Empty;

    }

    public void RechargeEnergy()
    {
        energy = maxEnergy;
        PlayerPrefs.SetInt(EnergyKey, maxEnergy);
        goButton.interactable = true;
        SetEnergyReadyText();
        energyText.text = $"{energy}";
        rechargeButton.SetActive(false);
    }

    public void StartGame()
    {
        if (energy > 0)
        {
            energy--;
            PlayerPrefs.SetInt(EnergyKey,energy);
            if (energy == 0)
            {
                DateTime rechargeTime = DateTime.Now.AddMinutes(energyRechargeTime);
                PlayerPrefs.SetString(EnergyReadyKey,rechargeTime.ToString());
            }
            SceneManager.LoadScene(1);
        }
    }
    
    public void QuitGame()
    {
        Application.Quit(0);
    }
}
