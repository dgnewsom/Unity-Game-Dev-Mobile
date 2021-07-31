using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipModel : MonoBehaviour
{
    [SerializeField] float minForce = 500f;
    [SerializeField] float maxForce = 750f;
    [SerializeField] private float radius = 1f;

    private Transform[] cells;
    private Vector3[] originalCellPositions;
    private Vector3[] originalCellRotations;

    void Awake()
    {
        List<Transform> temp = new List<Transform>();
        foreach (Transform t in transform)
        {
            temp.Add(t);
        }

        cells = temp.ToArray();
        originalCellPositions = new Vector3[cells.Length];
        originalCellRotations = new Vector3[cells.Length];

        for (int i = 0; i < cells.Length; i++)
        {
            originalCellPositions[i] = cells[i].localPosition;
            originalCellRotations[i] = cells[i].localEulerAngles;
            
        }
        Reset();
    }

    // Start is called before the first frame update
    void Start()
    {
        /*Explode();
        Invoke(nameof(Reset),3f);*/
    }

    public void Explode()
    {

        //FindObjectOfType<CollectibleIndicatorSpawner>().ClearAllIndicators();
        foreach (Transform cell in cells)
        {
            Rigidbody rb = cell.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddExplosionForce(Random.Range(minForce, maxForce), transform.position, radius);
                rb.AddRelativeTorque(Random.Range(minForce,maxForce),Random.Range(minForce,maxForce),Random.Range(minForce,maxForce));
            }
        }
    }

    public void Reset()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].localPosition = originalCellPositions[i];
            cells[i].localEulerAngles = originalCellRotations[i];
            Rigidbody rb = cells[i].GetComponent<Rigidbody>();
            if (rb)
            {
                rb.isKinematic = true;
                rb.velocity = new Vector3(0,0,0);
            }
        }
    }
    

}
