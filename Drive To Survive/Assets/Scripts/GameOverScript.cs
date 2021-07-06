using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Game over script to calculate, display and check Score / HighScore
/// </summary>
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

    /// <summary>
    /// Calculate and display score parameters and high score, 
    /// </summary>
    public void DisplayGameOver()
    {
        gameOverPanel.SetActive(true);
        int[] Scores = CalculateScore();
        
        bool isHighScore = scoreSystem.CheckHighScore(Scores[3]);

        scoreValuesText.text = $"{Scores[0]}\nX\n{Scores[1]}\nX\n{Scores[2]}\n=\n{Scores[3]}";
        
        //If new HighScore, flash HighScore text
        if (isHighScore)
        {
            highScoresText.text = $"New Highscore!\n{PlayerPrefs.GetInt(ScoreSystem.HighScoreKey)}";
            StartCoroutine(FlashHighscoreText());
        }
        else
        {
            highScoresText.text = $"Highscore\n{PlayerPrefs.GetInt(ScoreSystem.HighScoreKey)}";
        }
    }

    /// <summary>
    /// Calculates and returns score as in array
    /// </summary>
    /// <returns>int array containing (initial score, laps completed, topSpeedBonus, total score)</returns>
    private int[] CalculateScore()
    {
        int[] ScoresOut = new int[4];
        ScoresOut[0] = scoreSystem.Score;
        ScoresOut[1] = lapController.LapsCompleted;
        ScoresOut[2] = (int) player.TopSpeed / 10;
        ScoresOut[3] = ScoresOut[0] * ScoresOut[1] * ScoresOut[2];
        return ScoresOut;
    }

    /// <summary>
    /// Method to continually flash HighScore text
    /// </summary>
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
