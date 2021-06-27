using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;

public enum PickupType{Score, SpeedUp, SpeedDown};

public class PickupScript : MonoBehaviour
{
    [SerializeField] private GameObject scorePrefab;
    [SerializeField] private GameObject speedUpPrefab;
    [SerializeField] private GameObject speedDownPrefab;

    private PickupType pickupType;
    private GameObject pickupModel;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        SetPickupType();
    }

    private void SetPickupType()
    {
        pickupType = (PickupType) Random.Range(0, 3);
        GameObject.Destroy(pickupModel);
        pickupModel = Instantiate(GetPickupPrefab(),transform.position,Quaternion.identity,this.transform);
    }


    private GameObject GetPickupPrefab()
    {
        switch (pickupType)
        {
            case PickupType.Score:
                return scorePrefab;
            case PickupType.SpeedUp:
                return speedUpPrefab;
            case PickupType.SpeedDown:
                return speedDownPrefab;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
