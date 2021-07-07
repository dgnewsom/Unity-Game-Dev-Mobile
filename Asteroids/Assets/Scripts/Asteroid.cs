using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        print(playerHealth);
        if (playerHealth == null){return;}
            
        playerHealth.TakeDamage(damageAmount);
    }

}
