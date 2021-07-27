using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Collectible : MonoBehaviour
{
    [SerializeField] private CollectibleType collectibleType;
    [SerializeField] private Material[] forceFieldMaterials;
    [SerializeField] private Material[] particleMaterials;
    [SerializeField] private GameObject pickupModel;
    [SerializeField] private ParticleSystem pickupEffect;
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
    

    private void Awake()
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

    public void FixedUpdate()
    {
        CheckCollectibleOnScreen();
    }

    private void CheckCollectibleOnScreen()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        if (viewportPosition.x > 1.2 || viewportPosition.x < -0.2 || viewportPosition.y > 1.2 || viewportPosition.y < -0.2)
        {
            Destroy(gameObject);
        }
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
        collectibleAmount = collectibleType switch
        {
            CollectibleType.ScoreUp => GetRandomAmountFromArrayWithProbability(percentages),
            CollectibleType.ScoreDown => GetRandomAmountFromArrayWithProbability(percentages),
            CollectibleType.ScoreMultiply => GetRandomAmountFromArrayWithProbability(multipliers),
            CollectibleType.HealthUp => GetRandomAmountFromArrayWithProbability(percentages),
            _ => 0
        };
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
            pickupEffect.GetComponent<Renderer>().material = particleMaterials[0];
            collectibleIconImage.color = Color.white;
            return;
        }
        Color colour = CollectibleTypes.GetColourFromPickupType(collectibleType);
        collectibleIconImage.color = colour;
        if (colour.Equals(Color.green))
        {
            forceFieldMeshRenderer.material = forceFieldMaterials[0];
            pickupEffect.GetComponent<Renderer>().material = particleMaterials[0];
        }

        if (colour.Equals(Color.red))
        {
            forceFieldMeshRenderer.material = forceFieldMaterials[1];
            pickupEffect.GetComponent<Renderer>().material = particleMaterials[1];
        }

        if (colour.Equals(Color.yellow))
        {
            forceFieldMeshRenderer.material = forceFieldMaterials[2];
            pickupEffect.GetComponent<Renderer>().material = particleMaterials[2];
        }
    }

    private void SetEffectTime()
    {
        effectTime = collectibleType switch
        {
            CollectibleType.ScoreMultiply => GetRandomAmountFromArrayWithProbability(effectTimes),
            CollectibleType.Lasers => GetRandomAmountFromArrayWithProbability(effectTimes),
            CollectibleType.Shield => GetRandomAmountFromArrayWithProbability(effectTimes),
            _ => 0
        };
    }

    private void SetText()
    {
        amountText.text = collectibleType switch
        {
            CollectibleType.ScoreUp => $"{collectibleAmount}%",
            CollectibleType.ScoreDown => $"{collectibleAmount}%",
            CollectibleType.ScoreMultiply => $"x{collectibleAmount}\n{effectTime}s",
            CollectibleType.HealthUp => $"{collectibleAmount}%",
            CollectibleType.Lasers => $"{effectTime}s",
            CollectibleType.Shield => $"{effectTime}s",
            CollectibleType.Continue => $"x1",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        //print($"{other.gameObject}");
        if (other.CompareTag("Player"))
        {
            print($"Collectible hit");
            ActivateCollectible();
        }
    }

    private void ActivateCollectible()
    {
        Vector3 indicatorPosition = FindObjectOfType<PlayerMovement>().GetPlayerScreenPosition();
        switch (collectibleType)
        {
            case CollectibleType.ScoreUp:
                SoundManager.Instance.PlayPositivePickupSound();
                FindObjectOfType<Scorer>().IncreaseScorePercentage(collectibleAmount);
                indicatorSpawner.SpawnIndicator(collectibleType,$"{collectibleAmount}%",indicatorPosition);
                break;
            case CollectibleType.ScoreDown:
                SoundManager.Instance.PlayNegativePickupSound();
                FindObjectOfType<Scorer>().DecreaseScorePercentage(collectibleAmount);
                indicatorSpawner.SpawnIndicator(collectibleType,$"{collectibleAmount}%",indicatorPosition);
                break;
            case CollectibleType.ScoreMultiply:
                SoundManager.Instance.PlayPositivePickupSound();
                uiScript.StartMultiplier(collectibleAmount, effectTime);
                indicatorSpawner.SpawnIndicator(collectibleType,$"x{collectibleAmount}",indicatorPosition);
                break;
            case CollectibleType.HealthUp:
                SoundManager.Instance.PlayPositivePickupSound();
                FindObjectOfType<PlayerHealth>().IncreaseHealthPercentage(collectibleAmount);
                indicatorSpawner.SpawnIndicator(collectibleType,$"{collectibleAmount}%",indicatorPosition);
                break;
            case CollectibleType.Lasers:
                uiScript.StartLasers(effectTime);
                indicatorSpawner.SpawnIndicator(collectibleType,$"",indicatorPosition);
                break;
            case CollectibleType.Shield:
                uiScript.StartShield(effectTime);
                indicatorSpawner.SpawnIndicator(collectibleType,$"",indicatorPosition);
                break;
            case CollectibleType.Continue:
                SoundManager.Instance.PlayContinuePickupSound();
                Scorer.IncreaseContinues();
                indicatorSpawner.SpawnIndicator(collectibleType,$"",indicatorPosition);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        PickupCollectible();
    }

    private void PickupCollectible()
    {
        pickupEffect.Play();
        pickupModel.SetActive(false);
        DisableColliders();
        Destroy(gameObject,1f);
    }

    private void DisableColliders()
    {
        GetComponent<Collider>().enabled = false;
        GetComponentInChildren<Collider>().enabled = false;
    }
}
