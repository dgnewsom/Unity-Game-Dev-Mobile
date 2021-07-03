using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private float scoreMultiplier = 10f;

    private UIControllerScript uiController;
    public const string highScoreKey = "HighScore";
    private float score;

    private void Start()
    {
        uiController = FindObjectOfType<UIControllerScript>();
    }

    
    void FixedUpdate()
    {
        score += Time.deltaTime * scoreMultiplier;
        uiController.SetScoreText((int)score);
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
        score += amountToAdd;
    }

    internal void ScoreDownPickup(float scoreDownPercentage)
    {
        float amountToRemove = score * (scoreDownPercentage / 100);
        score -= amountToRemove;
    }

    public int GetScore()
    {
        return (int)score;
    }
}
