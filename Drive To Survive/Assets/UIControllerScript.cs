using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIControllerScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text lapText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text topSpeedText;
    [SerializeField] private TMP_Text speedometerText;

    public void SetLapText(int NumberLaps)
    {
        lapText.text = $"{NumberLaps}";
    }

    public void SetScoreText(int score)
    {
        scoreText.text = $"{score}";
    }

    public void SetTopSpeedText(int topSpeed)
    {
        topSpeedText.text = $"Top Speed\n{topSpeed} Mph";
    }

    public void SetSpeedometerText(int speed)
    {
        speedometerText.text = $"{speed}";
    }
}
