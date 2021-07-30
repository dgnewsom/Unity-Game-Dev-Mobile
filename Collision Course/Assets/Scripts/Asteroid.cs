using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private AsteroidType asteroidType;
    [SerializeField] private int damageAmount;
    [SerializeField] private Transform asteroidModel;
    [SerializeField] private Vector2 spinSpeedRange;
    [SerializeField] private float scoreAmount;
    [SerializeField] private GameObject[] asteroidModels;
    
    private float spinSpeed;
    private Rigidbody rb;
    private CollectibleIndicatorSpawner indicatorSpawner;
    private Scorer scorer;
    private Camera mainCamera;
    private Transform asteroidPool;

    private void Start()
    {
        CreateAsteroidModel();
        SetAsteroidType();
        spinSpeed = GetSpinSpeed();
        asteroidModel.rotation = Quaternion.Euler(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
        rb = GetComponent<Rigidbody>();
        indicatorSpawner = FindObjectOfType<CollectibleIndicatorSpawner>();
        scorer = FindObjectOfType<Scorer>();
        mainCamera = Camera.main;
        asteroidPool = GameObject.Find("Asteroid Pool").transform;

    }

    private void SetAsteroidType()
    {
        asteroidModel.GetComponent<AsteroidModel>().SetMaterial(asteroidType);
        damageAmount = (int) asteroidType + 1;
        scoreAmount = ((int) asteroidType + 1) * 1000;
    }

    private void CreateAsteroidModel()
    {
        asteroidModel = Instantiate(asteroidModels[Random.Range(0, asteroidModels.Length)], transform.position, Quaternion.identity, transform).transform;
    }

    private float GetSpinSpeed()
    {
        float speed = 0;
        while (speed > -1 && speed < 1)
        {
            speed = Random.Range(spinSpeedRange.x, spinSpeedRange.y);
        }

        return speed;
    }

    private void Update()
    {
        ApplySpinToAsteroid();
        CheckAsteroidOnScreen();
    }

    void OnEnable()
    {
        if (asteroidModel != null)
        {
            spinSpeed = GetSpinSpeed();
            asteroidModel.rotation = Quaternion.Euler(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
            asteroidModel.GetComponent<AsteroidModel>().Reset();
        }
    }

    /// <summary>
    /// Apply spin to asteroid at speed set in start.
    /// </summary>
    private void ApplySpinToAsteroid()
    {
        transform.Rotate(new Vector3(0f, 0f, 1f), spinSpeed);
    }

    private void CheckAsteroidOnScreen()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        if (viewportPosition.x > 1.2 || viewportPosition.x < -0.2 || viewportPosition.y > 1.2 || viewportPosition.y < -0.2)
        {
            StartCoroutine(DisableAsteroid(0.2f));
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        scorer.AddToScore(scoreAmount * scorer.GetCurrentMultiplier());
        Vector3 indicatorPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        indicatorSpawner.SpawnIndicator(CollectibleType.ScoreUp,$"{scoreAmount * scorer.GetCurrentMultiplier()}",indicatorPosition);
        Explode();

    }


    private void OnTriggerEnter(Collider other)
    {
        //print($"Collided with {other.gameObject}");
        if (other.CompareTag("Asteroid")||other.CompareTag("Collectible")){return;}

        if (other.CompareTag("Shield"))
        {
            scorer.AddToScore(scoreAmount * scorer.GetCurrentMultiplier());
            indicatorSpawner.SpawnIndicator(
                CollectibleType.ScoreUp, 
                $"{scoreAmount * scorer.GetCurrentMultiplier()}",
                FindObjectOfType<PlayerMovement>().GetPlayerScreenPosition()
            );
        }
        else if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
            scorer.RemoveFromScore(scoreAmount * scorer.GetCurrentMultiplier());
            indicatorSpawner.SpawnIndicator(
                                            CollectibleType.ScoreDown, 
                                            $"{scoreAmount * scorer.GetCurrentMultiplier()}",
                                            FindObjectOfType<PlayerMovement>().GetPlayerScreenPosition()
                                            );
        }

        Explode();
    }

    /// <summary>
    /// Disable model and collider, slow velocity by 2/3, start explosion effect then destroy after delay.
    /// </summary>
    private void Explode()
    {
        SoundManager.Instance.PlayAsteroidExplosionSound();
        EnableColliders(false);
        //asteroidModel.gameObject.SetActive(false);
        rb.velocity /= 3;
        asteroidModel.GetComponent<AsteroidModel>().Explode();
        GetComponentInChildren<ParticleSystem>().Play();
        StartCoroutine(DisableAsteroid(1.1f));
    }

    private void EnableColliders(bool enabled)
    {
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = enabled;
        }

    }

    IEnumerator DisableAsteroid(float delay)
    {
        yield return new WaitForSeconds(delay);
        //asteroidModel.gameObject.SetActive(true);
        EnableColliders(true);
        transform.parent = asteroidPool;
        gameObject.SetActive(false);
    }
}
