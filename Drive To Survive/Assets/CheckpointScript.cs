using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    [SerializeField] private bool IsStartLine;

    public void HitCheckpoint()
    {
        if (IsStartLine)
        {
            GetComponentInParent<LapController>().CheckLapComplete();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
