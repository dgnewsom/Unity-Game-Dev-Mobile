using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float scoreMultiplier = 10f;
    public const string highScoreKey = "HighScore";
    private float score;

    // Update is called once per frame
    void Update()
    {
        score += Time.deltaTime * scoreMultiplier;

        scoreText.text = ((int)score).ToString();
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
