using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private Transform asteroidModel;
    [SerializeField] private Vector2 spinSpeedRange;

    private float spinSpeed;
    private Rigidbody rb;

    private void Start()
    {
        spinSpeed = Random.Range(spinSpeedRange.x, spinSpeedRange.y);
        asteroidModel.rotation = Quaternion.Euler(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ApplySpinToAsteroid();
    }

    /// <summary>
    /// Apply spin to asteroid at speed set in start.
    /// </summary>
    private void ApplySpinToAsteroid()
    {
        transform.Rotate(new Vector3(0f, 0f, 1f), spinSpeed);
    }


    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth == null){return;}
            
        playerHealth.TakeDamage(damageAmount);

        Explode();
    }

    /// <summary>
    /// Disable model and collider, slow velocity by 2/3, start explosion effect then destroy after delay.
    /// </summary>
    private void Explode()
    {
        GetComponent<Collider>().enabled = false;
        asteroidModel.gameObject.SetActive(false);
        rb.velocity /= 3;
        GetComponentInChildren<ParticleSystem>().Play();
        Destroy(this.gameObject,5f);
    }
}
