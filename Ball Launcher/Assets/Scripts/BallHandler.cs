using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D currentBallRigidbody;

    [SerializeField]
    private SpringJoint2D currentBallSpringJoint;

    [SerializeField]
    private float detachDuration = 0.2f;

    private Camera mainCamera;

    private bool IsDragging;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
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
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;
    }
}
