using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject lasers;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private GameObject boosters;
    
    private SpaceshipModel spaceshipModel;
    private UIScript uiScript;
    private float currentHealth;
    

    void Start()
    {
        currentHealth = maxHealth;
        uiScript = FindObjectOfType<UIScript>();
        uiScript.SetHealthBarValue(currentHealth / maxHealth);
        spaceshipModel = GetComponentInChildren<SpaceshipModel>();
        SetShieldActive(false);
        SetLasersActive(false);
    }

    /// <summary>
    /// Damage the player ship and check for death
    /// </summary>
    /// <param name="damageAmount">Amount to damage the player ship</param>
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            DeathBehaviour();
        }
    }

    private void UpdateHealthBar()
    {
        uiScript.SetHealthBarValue(currentHealth / maxHealth);
    }

    /// <summary>
    /// Behaviour to perform on death
    /// </summary>
    private void DeathBehaviour()
    {
        //playerModel.SetActive(false);
        GameObject.FindObjectOfType<CollectibleIndicatorSpawner>().ClearAllIndicators();
        spaceshipModel.Explode();
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().velocity /= 3;
        SoundManager.Instance.PlayPlayerExplosionSound();
        explosion.Play();
        boosters.SetActive(false);
        SoundManager.Instance.StopBoostSound();
        uiScript.StopAllPickups();
        UIScript.IsRunning = false;
        Invoke(nameof(ShowGameOverScreen),3f);
    }

    public void ResetPlayerHealth()
    {
        //playerModel.SetActive(true);
        spaceshipModel.Reset();
        GetComponent<Collider>().enabled = true;
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void ShowGameOverScreen()
    {
        uiScript.ShowGameOverScreen();
    }

    public void SetShieldActive(bool active)
    {
        shield.SetActive(active);
    }

    public void SetLasersActive(bool active)
    {
        lasers.SetActive(active);
    }

    public void IncreaseHealthPercentage(int percentage)
    {
        float amountToIncrease = (currentHealth / 100) * percentage;
        currentHealth = Mathf.Clamp((currentHealth += amountToIncrease),0,maxHealth);
        UpdateHealthBar();
    }

    public bool HealthFull()
    {
        return !(currentHealth < maxHealth);
    }
}
