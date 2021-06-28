using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;

public enum PickupType{ScoreUp, ScoreDown, SpeedUp, SpeedDown};

public class RandomPickupScript : MonoBehaviour
{
    [Header("Pickup Parameters")]
    [SerializeField] private float scoreUpPercent = 10f;
    [SerializeField] private float scoreDownPercent = 10f;
    [SerializeField] private float speedUpPercent = 10f;
    [SerializeField] private float speedDownPercent = 10f;
    [Header("Model Prefabs")]
    [SerializeField] private GameObject scoreUpPrefab;
    [SerializeField] private GameObject scoreDownPrefab;
    [SerializeField] private GameObject speedUpPrefab;
    [SerializeField] private GameObject speedDownPrefab;

    private ScoreSystem scoreSystem;
    private PickupType pickupType;
    private GameObject pickupModel;
    private PickupGroup pickupGroup;

    // Start is called before the first frame update
    void Start()
    {
        scoreSystem = FindObjectOfType<ScoreSystem>();
        pickupGroup = GetComponentInParent<PickupGroup>();
    }

    private void OnEnable()
    {
        SetPickupType();
    }

    private void SetPickupType()
    {
        pickupType = (PickupType) Random.Range(0, 4);
        GameObject.Destroy(pickupModel);
        pickupModel = Instantiate(GetPickupPrefab(),transform.position,Quaternion.identity,this.transform);
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

    public void CollectPickup(Car player)
    {
        switch (pickupType)
        {
            case PickupType.ScoreUp:
                SoundManager.Instance.PlayScorePickupSound();
                scoreSystem.ScoreUpPickup(scoreUpPercent);
                break;
            case PickupType.ScoreDown:
                SoundManager.Instance.PlayScorePickupSound();
                scoreSystem.ScoreDownPickup(scoreDownPercent);
                break;
            case PickupType.SpeedUp:
                SoundManager.Instance.PlaySpeedUpPickupSound();
                player.SpeedUpPickup(speedUpPercent);
                break;
            case PickupType.SpeedDown:
                SoundManager.Instance.PlaySpeedDownPickupSound();
                player.SpeedDownPickup(speedDownPercent);
                break;
            default:
                throw new ArgumentOutOfRangeException();
            
        }
        pickupGroup.Reset();
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        gameObject.SetActive(false);
    }
}
