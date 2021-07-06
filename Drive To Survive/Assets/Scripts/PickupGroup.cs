using System.Collections;
using UnityEngine;

/// <summary>
/// Class to handle a group of random pickups.
/// </summary>
public class PickupGroup : MonoBehaviour
{
    private RandomPickupScript[] pickups;

    // Start is called before the first frame update
    void Start()
    {
        pickups = GetComponentsInChildren<RandomPickupScript>();
        Reset(0);
    }

    /// <summary>
    /// Reset to new random pickup / location in the group
    /// </summary>
    /// <param name="delay">Delay before reset</param>
    public void Reset(float delay = 10f)
    {
        StartCoroutine(ResetDelayed(delay));
    }

    /// <summary>
    /// Reset all pickups, wait for delay and enable a random one in the group
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator ResetDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (RandomPickupScript pickup in pickups)
        {
            pickup.Reset();
        }

        pickups[Random.Range(0, pickups.Length)].gameObject.SetActive(true);
    }

    
}
