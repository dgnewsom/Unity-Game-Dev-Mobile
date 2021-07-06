using UnityEngine;

/// <summary>
/// Class to handle Cone collisions
/// </summary>
public class ConeScript : MonoBehaviour
{
    [SerializeField] private float ScoreDeductionPercentage = 50f;
    [SerializeField] private float ResetDelay = 10f;
    private Vector3 InitialPosition;
    private Quaternion InitialRotation;
    private bool isHit;
    private ScoreSystem scoreSystem;

    // Start is called before the first frame update
    void Start()
    {
        scoreSystem = FindObjectOfType<ScoreSystem>();
        InitialPosition = transform.position;
        InitialRotation = transform.rotation;
        ResetCone();
    }

    /// <summary>
    /// Stop cones movement then reset to original position / rotation
    /// </summary>
    private void ResetCone()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        transform.position = InitialPosition;
        transform.rotation = InitialRotation;
        isHit = false;
    }

    /// <summary>
    /// Reduce score by percentage then reset after given delay.
    /// </summary>
    public void HitCone()
    {
        if (!isHit)
        {
            isHit = true;
            SoundManager.Instance.PlayScoreDownPickupSound();
            scoreSystem.ScoreDownPickup(ScoreDeductionPercentage);
            Invoke(nameof(ResetCone),ResetDelay);
        }
    }
}
