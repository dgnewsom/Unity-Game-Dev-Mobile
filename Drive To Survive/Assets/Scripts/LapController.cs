using UnityEngine;

/// <summary>
/// Class to handle completion of laps around the circuit
/// </summary>
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
    
    /// <summary>
    /// Method to check if all checkpoints have been passed and increment number of laps.
    /// </summary>
    public void CheckLapComplete()
    {
        //Check if each checkpoint is still enabled
        int numberOfCheckpointsleft = 0;
        foreach (CheckpointScript checkpointScript in checkpoints)
        {
            if (checkpointScript.gameObject.activeInHierarchy)
            {
                numberOfCheckpointsleft++;
            }
        }
        //If only start finish remains then increment laps and reset all checkpoints
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

    /// <summary>
    /// Reset all checkpoints to enabled
    /// </summary>
    private void ResetCheckpoints()
    {
        foreach (CheckpointScript checkpointScript in checkpoints)
        {
            checkpointScript.gameObject.SetActive(true);
        }
    }

}
