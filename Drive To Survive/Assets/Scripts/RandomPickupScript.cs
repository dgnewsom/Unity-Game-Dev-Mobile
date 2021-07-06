using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public enum PickupType{ScoreDown, ScoreUp, SpeedUp, SpeedDown};

/// <summary>
/// Class to handle random pickups
/// </summary>
public class RandomPickupScript : MonoBehaviour
{
    
    [Header("Model Prefabs")]
    [SerializeField] private GameObject scoreUpPrefab;
    [SerializeField] private GameObject scoreDownPrefab;
    [SerializeField] private GameObject speedUpPrefab;
    [SerializeField] private GameObject speedDownPrefab;
    [SerializeField] private TMP_Text percentageText;

    private int[] scorePercentages = {5, 10, 15, 20, 25, 30, 35, 40, 45, 50};
    private int[] speedPercentages = {5, 10, 15, 20};
    private int pickupPercentage;
    private ScoreSystem scoreSystem;
    private PickupType pickupType;
    private GameObject pickupModel;
    private PickupGroup pickupGroup;

    // Start is called before the first frame update
    void Start()
    {
        scoreSystem = FindObjectOfType<ScoreSystem>();
        pickupGroup = GetComponentInParent<PickupGroup>();
        percentageText = GetComponentInChildren<TMP_Text>();
    }
    
    private void OnEnable()
    {
        SetPickupType();
    }

    /// <summary>
    /// Choose random pickup type and percentage and the instantiate the correct model
    /// </summary>
    private void SetPickupType()
    {
        pickupType = (PickupType) Random.Range(0, 4);
        GameObject.Destroy(pickupModel);
        pickupModel = Instantiate(GetPickupPrefab(),transform.position,Quaternion.identity,this.transform);
        SetPickupPercentage();
    }

    /// <summary>
    /// Set a pickup percentage based on current type
    /// </summary>
    private void SetPickupPercentage()
    {
        switch (pickupType)
        {
            case PickupType.ScoreUp:
                SetPercentageAmountFromArray(scorePercentages);
                break;
            case PickupType.ScoreDown:
                SetPercentageAmountFromArray(scorePercentages);
                break;
            case PickupType.SpeedUp:
                SetPercentageAmountFromArray(speedPercentages);
                break;
            case PickupType.SpeedDown:
                SetPercentageAmountFromArray(speedPercentages);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// Calculate and set the pickups percentage from the relevant array.
    /// (higher indexes less likely.)
    /// </summary>
    /// <param name="percentageArray"></param>
    private void SetPercentageAmountFromArray(int[] percentageArray)
    {
        //Add random value between 0 and 1 for each array element 
        float random = 0;
        for (int i = 0; i < percentageArray.Length; i++)
        {
            random += Random.Range(0f, 0.9f);
        }

        //round the result and set value at this index as percentage.
        int index = Mathf.CeilToInt(random);
        index = Mathf.Clamp(index, 0, percentageArray.Length - 1);
        pickupPercentage = percentageArray[index];
        percentageText.text = $"{pickupPercentage}%";
    }

    private GameObject GetPickupPrefab()
    {
        switch (pickupType)
        {
            case PickupType.ScoreUp:
                return scoreUpPrefab;
            case PickupType.ScoreDown:
                return scoreDownPrefab;
            case PickupType.SpeedUp:
                return speedUpPrefab;
            case PickupType.SpeedDown:
                return speedDownPrefab;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// Trigger relevant sound and method on pickup and reset pickup group.
    /// </summary>
    /// <param name="player"></param>
    public void CollectPickup(Car player)
    {
        switch (pickupType)
        {
            case PickupType.ScoreUp:
                SoundManager.Instance.PlayScoreUpPickupSound();
                scoreSystem.ScoreUpPickup(pickupPercentage);
                break;
            case PickupType.ScoreDown:
                SoundManager.Instance.PlayScoreDownPickupSound();
                scoreSystem.ScoreDownPickup(pickupPercentage);
                break;
            case PickupType.SpeedUp:
                SoundManager.Instance.PlaySpeedUpPickupSound();
                player.SpeedUpPickup(pickupPercentage);
                break;
            case PickupType.SpeedDown:
                SoundManager.Instance.PlaySpeedDownPickupSound();
                player.SpeedDownPickup(pickupPercentage);
                break;
            default:
                throw new ArgumentOutOfRangeException();
            
        }

        pickupGroup.Reset();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Reset pickup
    /// </summary>
    public void Reset()
    {
        gameObject.SetActive(false);
    }
}
