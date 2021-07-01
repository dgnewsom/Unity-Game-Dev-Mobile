using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void ResetCone()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        transform.position = InitialPosition;
        transform.rotation = InitialRotation;
        isHit = false;
    }

    public void HitCone()
    {
        if (!isHit)
        {
            print("Hit Cone");
            isHit = true;
            scoreSystem.ScoreDownPickup(ScoreDeductionPercentage);
            Invoke(nameof(ResetCone),ResetDelay);
        }
    }
}
