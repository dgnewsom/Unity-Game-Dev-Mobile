using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Collectible : MonoBehaviour
{
    [SerializeField] private CollectibleType collectibleType;
    [SerializeField] private Material[] forceFieldMaterials;
    [SerializeField] private readonly int[] percentages = new[] {10, 15, 20, 25, 30, 35, 40, 50};
    [SerializeField] private readonly int[] multipliers = new[] {2, 3, 4, 5, 10, 15, 20, 25};
    [SerializeField] private readonly int[] effectTimes = new[] {10, 15, 20, 30, 45, 60};

    private UIScript uiScript;
    private Image collectibleIconImage;
    private int collectibleAmount = 0;
    private TMP_Text amountText;
    private float effectTime = 15f;
    private MeshRenderer forceFieldMeshRenderer;
    private Camera mainCamera;
    private CollectibleIndicatorSpawner indicatorSpawner;
    

    private void Start()
    {
        collectibleIconImage = GetComponentInChildren<Image>();
        amountText = GetComponentInChildren<TMP_Text>();
        forceFieldMeshRenderer = GetComponentInChildren<MeshRenderer>();
        uiScript = FindObjectOfType<UIScript>();
        mainCamera = Camera.main;
        indicatorSpawner = FindObjectOfType<CollectibleIndicatorSpawner>();

        //For Testing
        InitialiseCollectible();
    }
    


    public void SetCollectibleType(CollectibleType collectibleType)
    {
        this.collectibleType = collectibleType;
        InitialiseCollectible();
    }

    private void InitialiseCollectible()
    {
        SetImage();
        SetColour();
        SetAmount();
        SetEffectTime();
        SetText();

    }

    private void SetImage()
    {
        collectibleIconImage.sprite = CollectibleTypes.GetIconFromCollectibleType(collectibleType);
    }

    private void SetAmount()
    {
        switch (collectibleType)
        {
            case CollectibleType.ScoreUp:
                collectibleAmount = GetRandomAmountFromArrayWithProbability(percentages);
                break;
            case CollectibleType.ScoreDown:
                collectibleAmount = GetRandomAmountFromArrayWithProbability(percentages);
                break;
            case CollectibleType.ScoreMultiply:
                collectibleAmount = GetRandomAmountFromArrayWithProbability(multipliers);
                break;
            case CollectibleType.HealthUp:
                collectibleAmount = GetRandomAmountFromArrayWithProbability(percentages);
                break;
            default:
                collectibleAmount = 0;
                break;
        }
    }

    private int GetRandomAmountFromArrayWithProbability(int[] intArray)
    {
        float randomfloat = Random.value;
        if (randomfloat <= 0.8f)
        {
            return intArray[Random.Range(0, Mathf.FloorToInt(intArray.Length / 2))];
            
        }

        if (randomfloat <= 0.9f)
        {
            return intArray[Random.Range(Mathf.CeilToInt(intArray.Length / 2), Mathf.FloorToInt((intArray.Length / 4) * 3 ))];
        }

        return intArray[Random.Range(Mathf.CeilToInt((intArray.Length / 4) * 3 ), intArray.Length-1)];
    }

    private void SetColour()
    {
        if (collectibleType.Equals(CollectibleType.Lasers) || collectibleType.Equals(CollectibleType.Shield))
        {
            forceFieldMeshRenderer.material = forceFieldMaterials[0];
            collectibleIconImage.color = Color.white;
            return;
        }
        Color colour = CollectibleTypes.GetColourFromPickupType(collectibleType);
        collectibleIconImage.color = colour;
        if (colour.Equals(Color.green))
        {
            forceFieldMeshRenderer.material = forceFieldMaterials[0];
        }

        if (colour.Equals(Color.red))
        {
            forceFieldMeshRenderer.material = forceFieldMaterials[1];
        }

        if (colour.Equals(Color.yellow))
        {
            forceFieldMeshRenderer.material = forceFieldMaterials[2];
        }
    }

    private void SetEffectTime()
    {
        switch (collectibleType)
        {
            case CollectibleType.ScoreMultiply:
                effectTime = GetRandomAmountFromArrayWithProbability(effectTimes);
                break;
            case CollectibleType.Lasers:
                effectTime = GetRandomAmountFromArrayWithProbability(effectTimes);
                break;
            case CollectibleType.Shield:
                effectTime = GetRandomAmountFromArrayWithProbability(effectTimes);
                break;
            default:
                effectTime = 0;
                break;
        }
    }

    private void SetText()
    {
        switch (collectibleType)
        {
            case CollectibleType.ScoreUp:
                amountText.text = $"{collectibleAmount}%";
                break;
            case CollectibleType.ScoreDown:
                amountText.text = $"{collectibleAmount}%";
                break;
            case CollectibleType.ScoreMultiply:
                amountText.text = $"x{collectibleAmount}\n{effectTime}s";
                break;
            case CollectibleType.HealthUp:
                amountText.text = $"{collectibleAmount}%";
                break;
            case CollectibleType.Lasers:
                amountText.text = $"{effectTime}s";
                break;
            case CollectibleType.Shield:
                amountText.text = $"{effectTime}s";
                break;
            case CollectibleType.Continue:
                amountText.text = $"x1";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print($"{other.gameObject}");
        if (other.CompareTag("Player"))
        {
            print($"Collectible hit");
            ActivateCollectible();
        }
    }

    private void ActivateCollectible()
    {
        Vector3 indicatorPosition;
        switch (collectibleType)
        {
            case CollectibleType.ScoreUp:
                FindObjectOfType<Scorer>().IncreaseScorePercentage(collectibleAmount);
                indicatorPosition = mainCamera.WorldToScreenPoint(this.transform.position);
                indicatorSpawner.SpawnIndicator(collectibleType,$"{collectibleAmount}%",indicatorPosition);
                Destroy(gameObject);
                break;
            case CollectibleType.ScoreDown:
                FindObjectOfType<Scorer>().DecreaseScorePercentage(collectibleAmount);
                indicatorPosition = mainCamera.WorldToScreenPoint(this.transform.position);
                indicatorSpawner.SpawnIndicator(collectibleType,$"{collectibleAmount}%",indicatorPosition);
                Destroy(gameObject);
                break;
            case CollectibleType.ScoreMultiply:
                if (!uiScript.MultiplierActive)
                {
                    uiScript.StartMultiplier(collectibleAmount, effectTime);
                    indicatorPosition = mainCamera.WorldToScreenPoint(this.transform.position);
                    indicatorSpawner.SpawnIndicator(collectibleType,$"x{collectibleAmount}",indicatorPosition);
                    Destroy(gameObject);
                }
                break;
            case CollectibleType.HealthUp:
                FindObjectOfType<PlayerHealth>().IncreaseHealthPercentage(collectibleAmount);
                indicatorPosition = mainCamera.WorldToScreenPoint(this.transform.position);
                indicatorSpawner.SpawnIndicator(collectibleType,$"{collectibleAmount}%",indicatorPosition);
                Destroy(gameObject);
                break;
            case CollectibleType.Lasers:
                if (!uiScript.LasersActive)
                {
                    uiScript.StartLasers(effectTime);
                    indicatorPosition = mainCamera.WorldToScreenPoint(this.transform.position);
                    indicatorSpawner.SpawnIndicator(collectibleType,$"",indicatorPosition);
                    Destroy(gameObject);
                }
                break;
            case CollectibleType.Shield:
                if (!uiScript.ShieldActive)
                {
                    uiScript.StartShield(effectTime);
                    indicatorPosition = mainCamera.WorldToScreenPoint(this.transform.position);
                    indicatorSpawner.SpawnIndicator(collectibleType,$"",indicatorPosition);
                    Destroy(gameObject);
                }
                break;
            case CollectibleType.Continue:
                Scorer.IncreaseContinues();
                indicatorPosition = mainCamera.WorldToScreenPoint(this.transform.position);
                indicatorSpawner.SpawnIndicator(collectibleType,$"",indicatorPosition);
                Destroy(gameObject);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
}
