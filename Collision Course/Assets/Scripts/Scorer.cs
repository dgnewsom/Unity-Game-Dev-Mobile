using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorer : MonoBehaviour
{
    [SerializeField] private float scoreMultiplier = 50f;
    [SerializeField] private int continuesAllowed;
    [SerializeField] private float levelUpIncrements;

    private float score = 0;
    private int level = 1;
    private UIScript uiscript;
    private static int continuesRemaining;
    private float originalMultiplier;
    private float nextLevelAmount;
    private ObjectSpawner spawner;

    public static string HighscoreKey = "HighScore";
    public static string LevelHighscoreKey = "LevelHighcore";

    public float Score => score;

    public static int ContinuesRemaining => continuesRemaining;

    private void Awake()
    {
        originalMultiplier = scoreMultiplier;
        nextLevelAmount = levelUpIncrements;
    }

    private void Start()
    {
        continuesRemaining = continuesAllowed;
        uiscript = GetComponentInParent<UIScript>();
        uiscript.SetScoreDisplay((int)score);
        spawner = FindObjectOfType<ObjectSpawner>();
        uiscript.SetLevelText(level,(int)nextLevelAmount);
    }

    private void FixedUpdate()
    {
        if(!UIScript.IsRunning){return;}
        score += Time.deltaTime * scoreMultiplier;
        uiscript.SetScoreDisplay((int)score);
        if (score >= nextLevelAmount)
        {
            LevelUp();
            uiscript.SetLevelText(level,(int)nextLevelAmount);
        }
    }

    private void LevelUp()
    {
        level++;
        print($"Level up - {level}");
        nextLevelAmount += levelUpIncrements;
        spawner.LevelUp();
    }

    public bool CheckHighScore()
    {
        if (level > PlayerPrefs.GetInt(LevelHighscoreKey,0))
        {
            if ((int)score > PlayerPrefs.GetInt(HighscoreKey, 0))
            {
                PlayerPrefs.SetInt(LevelHighscoreKey,level);
                PlayerPrefs.SetInt(HighscoreKey,(int)score);
                return true;
            }
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
        score = Mathf.Clamp(score += amountToIncrease,0,float.MaxValue);
    }

    public void DecreaseScorePercentage(int percentage)
    {
        float amountToDecrease = (score / 100) * percentage;
        score = Mathf.Clamp(score -= amountToDecrease,0,float.MaxValue);
    }

    public void SetScoreMultiplier(int multiplierAmount)
    {
        scoreMultiplier = originalMultiplier * multiplierAmount;
    }

    public void ResetMultiplier()
    {
        scoreMultiplier = originalMultiplier;
    }

    public void AddToScore(float amountToAdd)
    {
        score = Mathf.Clamp(score += amountToAdd,0,float.MaxValue);
    }

    public void RemoveFromScore(float amountToRemove)
    {
        score = Mathf.Clamp(score -= amountToRemove,0,float.MaxValue);
    }
}
