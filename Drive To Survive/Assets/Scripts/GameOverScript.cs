using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScript : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text scoreValuesText;
    [SerializeField] private TMP_Text highScoresText;
    [SerializeField] private float timeBetweenHighScoreFlashes = 1f;

    private ScoreSystem scoreSystem;
    private LapController lapController;
    private Car player;

    public void Start()
    {
        scoreSystem = FindObjectOfType<ScoreSystem>();
        lapController = FindObjectOfType<LapController>();
        player = FindObjectOfType<Car>();
    }

    public void DisplayGameOver()
    {
        //Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        int score = scoreSystem.GetScore();
        int laps = lapController.LapsCompleted;
        int topSpeedBonus = (int) player.topSpeed / 10;
        int totalScore = score * laps * topSpeedBonus;
        bool isHighScore = scoreSystem.CheckHighScore(totalScore);

        scoreValuesText.text = $"{score}\nX\n{laps}\nX\n{topSpeedBonus}\n=\n{totalScore}";
        
        if (isHighScore)
        {
            highScoresText.text = $"New Highscore!\n{PlayerPrefs.GetInt(ScoreSystem.highScoreKey)}";
            StartCoroutine(FlashHighscoreText());
        }
        else
        {
            highScoresText.text = $"Highscore\n{PlayerPrefs.GetInt(ScoreSystem.highScoreKey)}";
        }
    }

    IEnumerator FlashHighscoreText()
    {
        while (true)
        {
            highScoresText.gameObject.SetActive(false);
            yield return new WaitForSeconds(timeBetweenHighScoreFlashes);
            highScoresText.gameObject.SetActive(true);
            yield return new WaitForSeconds(timeBetweenHighScoreFlashes);

        }
    }
}
