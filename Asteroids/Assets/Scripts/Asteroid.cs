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
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0f,0f,1f),spinSpeed);
    }



    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        print(playerHealth);
        if (playerHealth == null){return;}
            
        playerHealth.TakeDamage(damageAmount);

        Explode();
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        //Explode();
    }*/

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void Explode()
    {
        GetComponent<Collider>().enabled = false;
        asteroidModel.gameObject.SetActive(false);
        rb.velocity /= 3;
        GetComponentInChildren<ParticleSystem>().Play();

        Destroy(this.gameObject,5f);
    }
}
