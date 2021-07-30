using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidModel : MonoBehaviour
{
    [SerializeField] float minForce = 500f;
    [SerializeField] float maxForce = 750f;
    [SerializeField] private float radius = 1f;
    [SerializeField] private Material[] asteroidMaterials;

    private Transform[] cells;
    private Vector3[] originalCellPositions;
    private Vector3[] originalCellRotations;
    private GameObject interior;

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

        interior = cells[cells.Length - 1].gameObject;
        Reset();
    }

// Start is called before the first frame update
    void Start()
    {
        /*Explode();
        Invoke(nameof(Reset),5f);*/
    }

    public void Explode()
    {
        interior.SetActive(false);
        foreach (Transform cell in cells)
        {
            Rigidbody rb = cell.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddExplosionForce(Random.Range(minForce, maxForce), transform.position, radius);
            }
        }
    }

    public void Reset()
    {
        interior.SetActive(true);
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

    public void SetMaterial(AsteroidType asteroidType)
    {
        try
        {
            foreach (Transform cell in cells)
            {
                Renderer renderer = cell.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = asteroidMaterials[(int) asteroidType];
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Cannot set asteroid colour - {e}");
        }
    }
}
