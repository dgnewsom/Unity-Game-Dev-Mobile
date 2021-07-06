using UnityEngine;

/// <summary>
/// Class to handle Scoring and HighScores
/// </summary>
public class ScoreSystem : MonoBehaviour
{
    private UIControllerScript uiController;
    public const string HighScoreKey = "HighScore";
    private int score;
    private Car playerCar;

    public int Score => score;

    private void Start()
    {
        uiController = FindObjectOfType<UIControllerScript>();
        playerCar = FindObjectOfType<Car>();
    }
    
    void FixedUpdate()
    {
        //Increase score based upon curent speed and update display
        score += (int) (Time.deltaTime * playerCar.CurrentSpeed * 2);
        uiController.SetScoreText(score);
    }

    /// <summary>
    /// Check if given score is new highscore, save and return true / false
    /// </summary>
    /// <param name="scoreToCheck">Score to check</param>
    /// <returns>true if new high score, false if not</returns>
    public bool CheckHighScore(int scoreToCheck)
    {
        if (PlayerPrefs.GetInt(HighScoreKey,0) < scoreToCheck)
        {
            PlayerPrefs.SetInt(HighScoreKey,scoreToCheck);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Increase score pickup
    /// </summary>
    /// <param name="scoreUpPercentage">Percentage to increase score by</param>
    public void ScoreUpPickup(float scoreUpPercentage)
    {
        float amountToAdd = score * (scoreUpPercentage / 100);
        score += (int) amountToAdd;
    }

    /// <summary>
    /// Decrease score pickup
    /// </summary>
    /// <param name="scoreDownPercentage">Percentage to decrease score by</param>
    internal void ScoreDownPickup(float scoreDownPercentage)
    {
        float amountToRemove = score * (scoreDownPercentage / 100);
        score -= (int) amountToRemove;
    }
}
