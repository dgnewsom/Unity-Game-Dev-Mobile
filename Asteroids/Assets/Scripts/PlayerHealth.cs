using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private GameObject playerModel;

    private UIScript uiScript;
    private float currentHealth;
    private bool isDead;

    public bool IsDead => isDead;

    void Start()
    {
        currentHealth = maxHealth;
        uiScript = FindObjectOfType<UIScript>();
        uiScript.SetHealthBarValue(currentHealth / maxHealth);
    }

    /// <summary>
    /// Damage the player ship and check for death
    /// </summary>
    /// <param name="damageAmount">Amount to damage the player ship</param>
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        uiScript.SetHealthBarValue(currentHealth / maxHealth);
        if (currentHealth <= 0)
        {
            DeathBehaviour();
        }
    }

    /// <summary>
    /// Behaviour to perform on death
    /// </summary>
    private void DeathBehaviour()
    {
        isDead = true;
        playerModel.SetActive(false);
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().velocity /= 3;
        GetComponentInChildren<ParticleSystem>().Play();
        uiScript.ShowGameOverScreen();
    }
}
