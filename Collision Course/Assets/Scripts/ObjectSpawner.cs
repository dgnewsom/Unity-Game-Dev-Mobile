using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Asteroid spawn parameters")]
    [SerializeField] private Transform AsteroidPoolParent;
    [SerializeField] private Transform activeAsteroidsParent;
    [SerializeField] private Vector2 asteroidSpawnTimeRange;
    [SerializeField] private Vector2 asteroidSpeedRange;

    [Header("Collectible spawn Parameters")]
    [SerializeField] private GameObject collectiblePrefab;
    [SerializeField] private Vector2 collectibleSpawnTimeRange;
    [SerializeField] private Vector2 collectibleSpeedRange;

    [Header("Bypass need for countdown asteroidSpawnTimer in menu")]
    [SerializeField] private bool isMenuScene;

    [Header("Testing")] 
    [SerializeField] private bool asteroidsOn = true;
    [SerializeField] private bool collectiblesOn = true;


    private GameObject[] asteroidPool;
    private Camera mainCamera;
    private float asteroidSpawnTimer = 0f;
    private float collectibleSpawnTimer = 0f;
    private Vector2 spawnForceDirection = Vector2.zero;
    private Vector3 worldSpawnPoint;


    private void Start()
    {
        ResetCollectibleSpawnTimer();
        mainCamera = Camera.main;
        PopulateAsteroidPool();
    }

    private void PopulateAsteroidPool()
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (Transform asteroid in AsteroidPoolParent)
        {
            temp.Add(asteroid.gameObject);
            asteroid.gameObject.SetActive(false);
        }

        asteroidPool = temp.ToArray();
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

            if (collectibleSpawnTimer <= 0 && collectiblesOn)
            {
                SpawnCollectible();
            }
            else if (asteroidSpawnTimer <= 0 && asteroidsOn)
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
        if(AsteroidPoolParent.childCount <= 0){return;}
        ResetAsteroidSpawnTimer();
        SetSpawnPointAndDirection();
        GameObject asteroid = GetAsteroidFromPool();
        asteroid.transform.parent = activeAsteroidsParent;
        asteroid.transform.position = worldSpawnPoint;
        asteroid.SetActive(true);
        Rigidbody rb = asteroid.GetComponent<Rigidbody>();
        rb.velocity = spawnForceDirection.normalized * Random.Range(asteroidSpeedRange.x, asteroidSpeedRange.y);
    }

    private GameObject GetAsteroidFromPool()
    {
        GameObject toReturn = null;
        while (toReturn == null || toReturn.activeInHierarchy)
        {
            toReturn = asteroidPool[Random.Range(0,asteroidPool.Length)] ;
        }

        return toReturn;
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
                if (spawnPoint.y > 0.5f)
                {
                    spawnForceDirection = new Vector2(1f, Random.Range(0, -1f));
                }
                else
                {
                    spawnForceDirection = new Vector2(1f, Random.Range(0, 1f));
                }
                break;
            //Top
            case 1:
                spawnPoint.x = Random.value;
                spawnPoint.y = 1f;
                if (spawnPoint.x > 0.5f)
                {
                    spawnForceDirection = new Vector2(Random.Range(-1f, 0f), -1f);
                }
                else
                {
                    spawnForceDirection = new Vector2(Random.Range(1f, 0f), -1f);
                }
                break;
            //Right
            case 2:
                spawnPoint.x = 1f;
                spawnPoint.y = Random.value;
                if (spawnPoint.y > 0.5f)
                {
                    spawnForceDirection = new Vector2(-1f, Random.Range(0f, -1f));
                }
                else
                {
                    spawnForceDirection = new Vector2(-1f, Random.Range(0f, 1f));
                }
                break;
            //Bottom
            case 3:
                spawnPoint.x = Random.value;
                spawnPoint.y = 0f;
                if (spawnPoint.x > 0.5f)
                {
                    spawnForceDirection = new Vector2(Random.Range(-1f, 0f), 1f);
                }
                else
                {
                    spawnForceDirection = new Vector2(Random.Range(1f, 0f), 1f);
                }
                break;
            default:
                break;
        }
        worldSpawnPoint = mainCamera.ViewportToWorldPoint(spawnPoint);
        worldSpawnPoint.z = 0f;
    }

    private CollectibleType GetPickupToSpawn()
    {
        float random = Random.Range(0f,100f);

        if (random > 99)
        {
            return CollectibleType.Continue;
        }
        else
        {
            return (CollectibleType)Random.Range(0, System.Enum.GetValues(typeof(CollectibleType)).Length-1);
        }
    }

    public void LevelUp()
    {
        asteroidSpawnTimeRange.y = Mathf.Clamp(asteroidSpawnTimeRange.y -= 0.1f,asteroidSpawnTimeRange.x,asteroidSpawnTimeRange.y);
        asteroidSpeedRange.x = Mathf.Clamp(asteroidSpeedRange.x += 0.1f, 0, 5);
        asteroidSpeedRange.y = asteroidSpeedRange.y += 0.1f;
    }
}
