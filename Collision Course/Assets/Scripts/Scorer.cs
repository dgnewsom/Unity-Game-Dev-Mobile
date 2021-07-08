using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorer : MonoBehaviour
{
    [SerializeField] private float scoreMultiplier;
    [SerializeField] private int continuesAllowed;
 
    private float score = 0;
    private UIScript uiscript;
    private static int continuesRemaining;

    public static string HighscoreKey = "HighScore";

    public float Score => score;

    public static int ContinuesRemaining => continuesRemaining;

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
}
