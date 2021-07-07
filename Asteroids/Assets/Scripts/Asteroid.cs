using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private Transform asteroidModel;
    [SerializeField] private Vector2 spinSpeedRange;

    private float spinSpeed;

    private void Start()
    {
        spinSpeed = Random.Range(spinSpeedRange.x, spinSpeedRange.y);
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

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }



    private void Explode()
    {
        //TODO add explosion effect
        //asteroidModel.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
