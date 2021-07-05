using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    private UIControllerScript uiController;
    public const string highScoreKey = "HighScore";
    [SerializeField] private int score;
    private Car playerCar;

    private void Start()
    {
        uiController = FindObjectOfType<UIControllerScript>();
        playerCar = FindObjectOfType<Car>();
    }

    
    void FixedUpdate()
    {
        score += (int) (Time.deltaTime * playerCar.CurrentSpeed);
        uiController.SetScoreText(score);
    }

    public bool CheckHighScore(int score)
    {
        if (PlayerPrefs.GetInt(highScoreKey,0) < score)
        {
            PlayerPrefs.SetInt(highScoreKey,score);
            return true;
        }

        return false;
    }

    public void ScoreUpPickup(float scoreUpPercentage)
    {
        float amountToAdd = score * (scoreUpPercentage / 100);
        score += (int) amountToAdd;
    }

    internal void ScoreDownPickup(float scoreDownPercentage)
    {
        float amountToRemove = score * (scoreDownPercentage / 100);
        score -= (int) amountToRemove;
    }

    public int GetScore()
    {
        return score;
    }
}
