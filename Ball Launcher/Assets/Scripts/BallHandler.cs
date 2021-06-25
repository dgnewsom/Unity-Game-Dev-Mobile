using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private int numberOfBalls = 3;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivotPoint;
    [SerializeField] private float respawnDelay = 1.0f;
    [SerializeField] private float detachDuration = 0.15f;

    
    private Rigidbody2D currentBallRigidbody;
    private SpringJoint2D currentBallSpringJoint;
    private Camera mainCamera;
    private bool IsDragging;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        SpawnBall();
    }

    private void SpawnBall()
    {
        if (numberOfBalls > 0)
        {
            GameObject ballInstance = GameObject.Instantiate(ballPrefab, pivotPoint.position, Quaternion.identity, this.transform);
            currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
            currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();
            currentBallSpringJoint.connectedBody = pivotPoint;
            numberOfBalls--;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBallRigidbody.Equals(null)) {return;}
        if (!Touchscreen.current.primaryTouch.press.IsPressed())
        {
            if (IsDragging)
            {
                LaunchBall();
            }
            IsDragging = false;
            return;
        }

        DragBall();
    }

    private void DragBall()
    {
        IsDragging = true;
        currentBallRigidbody.isKinematic = true;

        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        currentBallRigidbody.position = worldPosition;
    }

    private void LaunchBall()
    {
        currentBallRigidbody.isKinematic = false;
        currentBallRigidbody = null;

        Invoke(nameof(DetachBall),detachDuration);
        
    }

    private void DetachBall()
    {
        Destroy(currentBallSpringJoint.gameObject,5f);
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;
        Invoke(nameof(SpawnBall),respawnDelay);
    }
}
