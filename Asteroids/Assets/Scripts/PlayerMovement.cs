using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forceMagnitude;
    [SerializeField] private float maxVelocity;
    [SerializeField] private GameObject touchIndicator;
    [SerializeField] private float rotationSpeed;

    private Camera mainCamera;
    private Rigidbody rb;

    private Vector3 movementDirection;
    
    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ProcessInput();
        KeepPlayerOnScreen();
        RotateToFaceVelocity();
    }

    private void FixedUpdate()
    {
        if(movementDirection == Vector3.zero){return;}
        rb.AddForce(movementDirection * forceMagnitude * Time.deltaTime, ForceMode.Force);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }

    /// <summary>
    /// Get input from the player and set movement direction and touch indicator
    /// </summary>
    private void ProcessInput()
    {
        if (Touchscreen.current.primaryTouch.press.IsPressed())
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
            worldPosition.z = 0f;

            SetTouchIndicator(worldPosition);

            movementDirection = worldPosition - transform.position;
            movementDirection.z = 0f;
            movementDirection.Normalize();
        }
        else
        {
            SetTouchIndicator(Vector3.zero);
            movementDirection = Vector3.zero;
        }
    }

    /// <summary>
    /// Set touch indicator position and if world position is 0 hide it.
    /// </summary>
    /// <param name="worldPosition"></param>
    private void SetTouchIndicator(Vector3 worldPosition)
    {
        touchIndicator.transform.position = worldPosition;
        touchIndicator.SetActive(!(worldPosition == Vector3.zero));
    }

    /// <summary>
    /// Check if player is off screen and if so move to opposite side of screen
    /// </summary>
    private void KeepPlayerOnScreen()
    {
        Vector3 newPosition = transform.position;

        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        if (viewportPosition.x > 1)
        {
            newPosition.x = -newPosition.x + 0.1f;
        }

        if (viewportPosition.x < 0)
        {
            newPosition.x = -newPosition.x - 0.1f;
        }

        if (viewportPosition.y > 1)
        {
            newPosition.y = -newPosition.y + 0.1f;
        }

        if (viewportPosition.y < 0)
        {
            newPosition.y = -newPosition.y - 0.1f;
        }
        

        transform.position = newPosition;
    }

    private void RotateToFaceVelocity()
    {
        if(rb.velocity == Vector3.zero){return;}
        Quaternion targetRotation = Quaternion.LookRotation(rb.velocity, Vector3.back);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
