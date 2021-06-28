using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGroup : MonoBehaviour
{
    private RandomPickupScript[] pickups;

    // Start is called before the first frame update
    void Start()
    {
        pickups = GetComponentsInChildren<RandomPickupScript>();
        Reset(0);
    }

    public void Reset(float delay = 2f)
    {
        StartCoroutine(ResetDelayed(delay));
    }

    IEnumerator ResetDelayed(float delay)
    {
        foreach (RandomPickupScript pickup in pickups)
        {
            pickup.Reset();
        }

        yield return new WaitForSeconds(delay);
        pickups[Random.Range(0, pickups.Length)].gameObject.SetActive(true);
    }
}
