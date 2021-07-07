using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int startHealth = 1;

    private float currentHealth;

    void Start()
    {
        currentHealth = startHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        print($"{damageAmount} damage taken!");
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Crash();
        }
    }

    private void Crash()
    {
        print("Crashed");
        gameObject.SetActive(false);
    }
}
