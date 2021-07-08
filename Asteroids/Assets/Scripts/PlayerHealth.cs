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

    public void TakeDamage(int damageAmount)
    {
        print($"{damageAmount} damage taken!");
        currentHealth -= damageAmount;
        uiScript.SetHealthBarValue(currentHealth / maxHealth);
        if (currentHealth <= 0)
        {
            Crash();
        }
    }

    private void Crash()
    {
        isDead = true;
        playerModel.SetActive(false);
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().velocity /= 3;
        GetComponentInChildren<ParticleSystem>().Play();
    }
}
