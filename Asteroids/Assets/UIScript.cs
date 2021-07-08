using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [SerializeField] private Slider healthBar;

    public void SetHealthBarValue(float currentHealthPercentage)
    {
        healthBar.value = currentHealthPercentage;
    }
}
