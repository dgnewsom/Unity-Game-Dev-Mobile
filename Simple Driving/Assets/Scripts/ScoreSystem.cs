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

    private void OnDestroy()
    {
        CheckHighScore((int)score);
    }

    internal void CheckHighScore(int score)
    {
        if (PlayerPrefs.GetInt(highScoreKey,0) < score)
        {
            PlayerPrefs.SetInt(highScoreKey,score);
        }
    }
}
