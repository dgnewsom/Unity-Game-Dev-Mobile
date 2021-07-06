using UnityEngine;

/// <summary>
/// Car class used to control the players car / collisions
/// </summary>
public class Car : MonoBehaviour
{
    [Header("Car Parameters")]
    [SerializeField] private float startSpeed = 50f;
    [SerializeField] private float maxSpeed = 200f;
    [SerializeField] private float speedGainPerSecond = 0.5f;
    [SerializeField] private float baseTurnSpeed = 115f;

    private float turnSpeed;
    private int steerValue;
    private UIControllerScript uiController;
    private bool gameOver = false;
    private float currentSpeed = 0f;
    private float topSpeed = 0f;

    /// <summary>
    /// Getters for current and top speed
    /// </summary>
    public float CurrentSpeed => currentSpeed;
    public float TopSpeed => topSpeed;

    // Start is called before the first frame update
    void Start()
    {
        uiController = FindObjectOfType<UIControllerScript>();
        currentSpeed = startSpeed;
        SetTurnSpeedFromCurrentSpeed();
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        //If game not ended, increase current speed, display on UI and calculate new turn speed
        if(gameOver){return;}
        currentSpeed += speedGainPerSecond * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
        SetTurnSpeedFromCurrentSpeed();
        uiController.SetSpeedometerText((int) currentSpeed);

        //Check if new top speed and display
        if (topSpeed < currentSpeed)
        {
            topSpeed = currentSpeed;
            uiController.SetTopSpeedText((int) topSpeed);
        }

        //Apply movement and rotation
        transform.Rotate(0f,steerValue * baseTurnSpeed * Time.deltaTime,0f) ;
        transform.Translate(Vector3.forward * currentSpeed / 5f * Time.deltaTime);
    }

    /// <summary>
    /// Calculate turn speed based upon current speed
    /// </summary>
    private void SetTurnSpeedFromCurrentSpeed()
    {
        turnSpeed = baseTurnSpeed + (currentSpeed / 10f);
    }


    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Obstacle"))
        {
            GameOverBehaviour();
        }
        
        if (other.collider.CompareTag("Cone"))
        {
            other.gameObject.GetComponent<ConeScript>().HitCone();
        }
    }

    /// <summary>
    /// Action to take on game over
    /// </summary>
    private void GameOverBehaviour()
    {
        gameOver = true;
        currentSpeed = 0;
        FindObjectOfType<GameOverScript>().DisplayGameOver();
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

        if (other.CompareTag("Pickup Group"))
        {
            other.GetComponent<PickupGroup>().Reset(15f);
        }
    }

    /// <summary>
    /// Steering direction called by control UI
    /// </summary>
    /// <param name="value">left = -1, right = 1</param>
    public void Steer(int value)
    {
        steerValue = value;
    }

    /// <summary>
    /// Increase speed pickup
    /// </summary>
    /// <param name="speedUpPercent">Amount to increase speed by</param>
    internal void SpeedUpPickup(float speedUpPercent)
    {
        float speedUpAmount = Mathf.Clamp(currentSpeed * (speedUpPercent / 100),currentSpeed, maxSpeed);
        currentSpeed += speedUpAmount;
    }

    /// <summary>
    /// Speed down pickup
    /// </summary>
    /// <param name="speedDownPercent">Amount to decrease speed by</param>
    internal void SpeedDownPickup(float speedDownPercent)
    {
        float speedDownAmount = currentSpeed * (speedDownPercent / 100);
        Mathf.Clamp(currentSpeed -= speedDownAmount,0,currentSpeed) ;
    }
}
