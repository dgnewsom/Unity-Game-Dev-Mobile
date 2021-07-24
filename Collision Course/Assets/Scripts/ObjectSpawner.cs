using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Asteroid spawn parameters")]
    [SerializeField] private GameObject[] asteroidPrefabs;
    [SerializeField] private Vector2 asteroidSpawnTimeRange;
    [SerializeField] private Vector2 asteroidSpeedRange;

    [Header("Collectible spawn Parameters")]
    [SerializeField] private GameObject collectiblePrefab;
    [SerializeField] private Vector2 collectibleSpawnTimeRange;
    [SerializeField] private Vector2 collectibleSpeedRange;

    [Header("Bypass need for countdown asteroidSpawnTimer in menu")]
    [SerializeField] private bool isMenuScene;

    private Camera mainCamera;
    private float asteroidSpawnTimer = 0f;
    private float collectibleSpawnTimer = 0f;
    private Vector2 spawnForceDirection = Vector2.zero;
    private Vector3 worldSpawnPoint;

    private void Start()
    {
        ResetAsteroidSpawnTimer();
        ResetCollectibleSpawnTimer();
        mainCamera = Camera.main;
    }

    private void ResetAsteroidSpawnTimer()
    {
        asteroidSpawnTimer = Random.Range(asteroidSpawnTimeRange.x, asteroidSpawnTimeRange.y);
    }

    private void ResetCollectibleSpawnTimer()
    {
        collectibleSpawnTimer = Random.Range(collectibleSpawnTimeRange.x, collectibleSpawnTimeRange.y);
    }

    void FixedUpdate()
    {
        if (isMenuScene || UIScript.IsRunning)
        {
            collectibleSpawnTimer -= Time.deltaTime;
            asteroidSpawnTimer -= Time.deltaTime;

            if (collectibleSpawnTimer <= 0)
            {
                SpawnCollectible();
            }
            else if (asteroidSpawnTimer <= 0)
            {
                SpawnAsteroid();
            }
        }
    }

    private void SpawnCollectible()
    {
        ResetCollectibleSpawnTimer();
        SetSpawnPointAndDirection();

        CollectibleType collectibleTypeToSpawn = GetPickupToSpawn();

        GameObject collectibleInstance = Instantiate(
            collectiblePrefab, 
            worldSpawnPoint, 
            Quaternion.identity, 
            this.transform);

        collectibleInstance.GetComponent<Collectible>().SetCollectibleType(collectibleTypeToSpawn);

        Rigidbody rb = collectibleInstance.GetComponent<Rigidbody>();
        rb.velocity = spawnForceDirection.normalized * Random.Range(collectibleSpeedRange.x, collectibleSpeedRange.y);

    }

    /// <summary>
    /// Choose random side, direction, and speed,
    /// then instantiate random asteroid and apply force.
    /// </summary>
    private void SpawnAsteroid()
    {
        ResetAsteroidSpawnTimer();
        SetSpawnPointAndDirection();
        GameObject asteroidInstance = Instantiate(
            asteroidPrefabs[Random.Range(0,asteroidPrefabs.Length)], 
            worldSpawnPoint, 
            Quaternion.Euler(0f,0f,Random.Range(0f, 360f)), 
            this.transform);

        Rigidbody rb = asteroidInstance.GetComponent<Rigidbody>();
        rb.velocity = spawnForceDirection.normalized * Random.Range(asteroidSpeedRange.x, asteroidSpeedRange.y);

    }

    private void SetSpawnPointAndDirection()
    {
        int side = Random.Range(0, 4);

        Vector2 spawnPoint = Vector2.zero;
        spawnForceDirection = Vector2.zero;

        switch (side)
        {
            //Left
            case 0:
                spawnPoint.x = 0f;
                spawnPoint.y = Random.value;
                spawnForceDirection = new Vector2(1f, Random.Range(-1f, 1f));
                break;
            //Top
            case 1:
                spawnPoint.x = Random.value;
                spawnPoint.y = 1f;
                spawnForceDirection = new Vector2(Random.Range(-1f, 1f), -1f);
                break;
            //Right
            case 2:
                spawnPoint.x = 1f;
                spawnPoint.y = Random.value;
                spawnForceDirection = new Vector2(-1f, Random.Range(-1f, 1f));
                break;
            //Bottom
            case 3:
                spawnPoint.x = Random.value;
                spawnPoint.y = 0f;
                spawnForceDirection = new Vector2(Random.Range(-1f, 1f), 1f);
                break;
            default:
                break;
        }
        worldSpawnPoint = mainCamera.ViewportToWorldPoint(spawnPoint);
        worldSpawnPoint.z = 0f;
    }

    private CollectibleType GetPickupToSpawn()
    {
        float random = Random.Range(0,500);

        if (random >= 475)
        {
            print($"random ({random}) - Continue ");
            return CollectibleType.Continue;
        }
        else
        {
            print("Other");
            return (CollectibleType)Random.Range(0, System.Enum.GetValues(typeof(CollectibleType)).Length-2);
        }
    }
}
