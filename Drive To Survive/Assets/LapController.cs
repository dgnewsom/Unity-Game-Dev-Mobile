using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LapController : MonoBehaviour
{
    [SerializeField] private TMP_Text lapText;
    private CheckpointScript[] checkpoints;
    private int laps;
    private void Start()
    {
        laps = 1;
        lapText.text = $"{laps}";
        checkpoints = GetComponentsInChildren<CheckpointScript>();
    }

    public void CheckLapComplete()
    {
        int numberOfCheckpointsleft = 0;
        foreach (CheckpointScript checkpointScript in checkpoints)
        {
            if (checkpointScript.gameObject.activeInHierarchy)
            {
                numberOfCheckpointsleft++;
            }
        }

        if (numberOfCheckpointsleft <= 1)
        {
            laps++;
            lapText.text = $"{laps}";
            ResetCheckpoints();
        }
        else
        {
            Debug.LogWarning("Checkpoint Missed");
        }
    }

    private void ResetCheckpoints()
    {
        foreach (CheckpointScript checkpointScript in checkpoints)
        {
            checkpointScript.gameObject.SetActive(true);
        }
    }

    public int GetLaps()
    {
        return laps;
    }
}
