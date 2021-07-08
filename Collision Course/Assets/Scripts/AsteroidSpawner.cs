using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] asteroidPrefabs;
    [SerializeField] private float secondsBetweenAsteroids = 1f;
    [SerializeField] private Vector2 speedRange;
    [SerializeField] private bool isMenuScene;

    private Camera mainCamera;
    private float timer = 0f;

    private void Start()
    {
        timer = secondsBetweenAsteroids;
        mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        if (isMenuScene || UIScript.IsRunning)
        {

            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                SpawnAsteroid();
                timer += secondsBetweenAsteroids;
            }
        }
    }

    /// <summary>
    /// Choose random side, direction, and speed,
    /// then instantiate random asteroid and apply force.
    /// </summary>
    private void SpawnAsteroid()
    {
        int side = Random.Range(0, 4);

        Vector2 spawnPoint = Vector2.zero;
        Vector2 direction = Vector2.zero;

        switch (side)
        {
            //Left
            case 0:
                spawnPoint.x = 0f;
                spawnPoint.y = Random.value;
                direction = new Vector2(1f, Random.Range(-1f,1f));
                break;
            //Top
            case 1:
                spawnPoint.x = Random.value;
                spawnPoint.y = 1f;
                direction = new Vector2(Random.Range(-1f,1f),-1f);
                break;
            //Right
            case 2:
                spawnPoint.x = 1f;
                spawnPoint.y = Random.value;
                direction = new Vector2(-1f, Random.Range(-1f,1f));
                break;
            //Bottom
            case 3:
                spawnPoint.x = Random.value;
                spawnPoint.y = 0f;
                direction = new Vector2(Random.Range(-1f,1f),1f);
                break;
            default:
                break;
        }

        Vector3 worldSpawnPoint = mainCamera.ViewportToWorldPoint(spawnPoint);
        worldSpawnPoint.z = 0f;
        GameObject selectedAsteroid = asteroidPrefabs[Random.Range(0,asteroidPrefabs.Length)];
        
        GameObject asteroidInstance = Instantiate(
            selectedAsteroid, 
            worldSpawnPoint, 
            Quaternion.Euler(0f,0f,Random.Range(0f,360f)), 
            this.transform);

        Rigidbody rb = asteroidInstance.GetComponent<Rigidbody>();
        rb.velocity = direction.normalized * Random.Range(speedRange.x, speedRange.y);
    }
}
