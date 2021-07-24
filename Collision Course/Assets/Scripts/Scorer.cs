using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorer : MonoBehaviour
{
    [SerializeField] private float scoreMultiplier = 50f;
    [SerializeField] private int continuesAllowed;
 
    private float score = 0;
    private UIScript uiscript;
    private static int continuesRemaining;
    private float originalMultiplier;

    public static string HighscoreKey = "HighScore";

    public float Score => score;

    public static int ContinuesRemaining => continuesRemaining;

    private void Awake()
    {
        originalMultiplier = scoreMultiplier;
    }

    private void Start()
    {
        continuesRemaining = continuesAllowed;
        uiscript = FindObjectOfType<UIScript>();
        uiscript.SetScoreDisplay((int)score);
    }

    private void FixedUpdate()
    {
        if(!UIScript.IsRunning){return;}
        score += Time.deltaTime * scoreMultiplier;
        uiscript.SetScoreDisplay((int)score);
    }

    public bool CheckHighScore()
    {
        if ((int)score > PlayerPrefs.GetInt(HighscoreKey, 0))
        {
            PlayerPrefs.SetInt(HighscoreKey,(int)score);
            return true;
        }

        return false;
    }

    public static void ReduceContinues()
    {
        continuesRemaining--;
    }

    public static void IncreaseContinues()
    {
        continuesRemaining++;
    }

    public void IncreaseScorePercentage(int percentage)
    {
        float amountToIncrease = (score / 100) * percentage;
        score += amountToIncrease;
    }

    public void DecreaseScorePercentage(int percentage)
    {
        float amountToDecrease = (score / 100) * percentage;
        score -= amountToDecrease;
    }

    public void SetScoreMultiplier(int multiplierAmount)
    {
        scoreMultiplier = originalMultiplier * multiplierAmount;
    }

    public void ResetMultiplier()
    {
        scoreMultiplier = originalMultiplier;
    }
}
