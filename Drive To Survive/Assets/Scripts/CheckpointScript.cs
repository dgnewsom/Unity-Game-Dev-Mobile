using UnityEngine;

/// <summary>
/// Basic Checkpoint script used by lap controller
/// </summary>
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
