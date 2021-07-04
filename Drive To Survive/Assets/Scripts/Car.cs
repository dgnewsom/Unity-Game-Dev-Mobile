using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
    [Header("Car Parameters")]
    [SerializeField] private float startSpeed = 50f;
    [SerializeField] private float maxSpeed = 200f;
    [SerializeField] private float speedGainPerSecond = 0.5f;
    [SerializeField] private float turnSpeed = 150f;

    private int steerValue;
    private UIControllerScript uiController;
    private bool gameOver = false;
    private float currentSpeed = 0f;
    private float topSpeed = 0f;

    public float CurrentSpeed => currentSpeed;
    public float TopSpeed => topSpeed;

    // Start is called before the first frame update
    void Start()
    {
        uiController = FindObjectOfType<UIControllerScript>();
        currentSpeed = startSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(gameOver){return;}
        currentSpeed += speedGainPerSecond * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
        uiController.SetSpeedometerText((int) currentSpeed);

        if (topSpeed < currentSpeed)
        {
            topSpeed = currentSpeed;
            uiController.SetTopSpeedText((int) topSpeed);
        }

        transform.Rotate(0f,steerValue * turnSpeed * Time.deltaTime,0f) ;
        transform.Translate(Vector3.forward * currentSpeed / 5f * Time.deltaTime);
    }
    
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Obstacle"))
        {
            gameOver = true;
            currentSpeed = 0;
            FindObjectOfType<GameOverScript>().DisplayGameOver();
        }

        if (other.collider.CompareTag("Cone"))
        {
            other.gameObject.GetComponent<ConeScript>().HitCone();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            other.GetComponent<RandomPickupScript>().CollectPickup(this);
        }

        if (other.CompareTag("Checkpoint"))
        {
            other.GetComponent<CheckpointScript>().HitCheckpoint();
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
