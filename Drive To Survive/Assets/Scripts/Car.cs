using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
    [SerializeField] private float startSpeed = 1f;
    [SerializeField] private float speedGainPerSecond = 0.1f;

    [SerializeField] private float currentSpeed = 0f;

    [SerializeField] private float turnSpeed = 200f;

    [SerializeField]private int steerValue;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = startSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        currentSpeed += speedGainPerSecond * Time.deltaTime;

        transform.Rotate(0f,steerValue * turnSpeed * Time.deltaTime,0f) ;
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Obstacle"))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            other.GetComponent<RandomPickupScript>().CollectPickup(this);
        }
    }

    public void Steer(int value)
    {
        steerValue = value;
    }

    internal void SpeedUpPickup(float speedUpPercent)
    {
        float speedUpAmount = currentSpeed * (speedUpPercent / 100);
        currentSpeed += speedUpAmount;
    }

    internal void SpeedDownPickup(float speedDownPercent)
    {
        float speedDownAmount = currentSpeed * (speedDownPercent / 100);
        Mathf.Clamp(currentSpeed -= speedDownAmount,0,currentSpeed) ;
    }
}
