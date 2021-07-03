using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LapController : MonoBehaviour
{
    private UIControllerScript uiController;
    private CheckpointScript[] checkpoints;
    private int lapsCompleted;

    public int LapsCompleted => lapsCompleted;

    private void Start()
    {
        uiController = FindObjectOfType<UIControllerScript>();
        checkpoints = GetComponentsInChildren<CheckpointScript>();
        lapsCompleted = 1;
        uiController.SetLapText(lapsCompleted);
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
            lapsCompleted++;
            uiController.SetLapText(lapsCompleted);
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

}
