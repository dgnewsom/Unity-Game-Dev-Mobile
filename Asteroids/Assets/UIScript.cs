using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [SerializeField] private Slider healthBar;

    /// <summary>
    /// Set healthbar to players current health percentage.
    /// </summary>
    /// <param name="currentHealthPercentage">current health percentage (0-1)</param>
    public void SetHealthBarValue(float currentHealthPercentage)
    {
        healthBar.value = currentHealthPercentage;
    }
}
