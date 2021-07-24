using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private Transform asteroidModel;
    [SerializeField] private Vector2 spinSpeedRange;
    [SerializeField] private float scoreAmount;

    private float spinSpeed;
    private Rigidbody rb;
    private CollectibleIndicatorSpawner indicatorSpawner;
    private Scorer scorer;
    private Camera mainCamera;

    private void Start()
    {
        spinSpeed = GetSpinSpeed();
        asteroidModel.rotation = Quaternion.Euler(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
        rb = GetComponent<Rigidbody>();
        indicatorSpawner = FindObjectOfType<CollectibleIndicatorSpawner>();
        scorer = FindObjectOfType<Scorer>();
        mainCamera = Camera.main;
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
        scorer.AddToScore(scoreAmount);
        Vector3 indicatorPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        indicatorSpawner.SpawnIndicator(CollectibleType.ScoreUp,$"+{scoreAmount}",indicatorPosition);
        Explode();

    }


    private void OnTriggerEnter(Collider other)
    {
        //print($"Collided with {other.gameObject}");
        if (other.CompareTag("Asteroid")||other.CompareTag("Collectible")){return;}

        if (other.CompareTag("Shield"))
        {
            scorer.AddToScore(scoreAmount);
            indicatorSpawner.SpawnIndicator(
                CollectibleType.ScoreUp, 
                $"-{scoreAmount}",
                FindObjectOfType<PlayerMovement>().GetPlayerScreenPosition()
            );
        }
        else if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
            scorer.RemoveFromScore(scoreAmount);
            indicatorSpawner.SpawnIndicator(
                                            CollectibleType.ScoreDown, 
                                            $"-{scoreAmount}",
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
        FindObjectOfType<SoundManager>().PlayAsteroidExplosionSound();
        EnableColliders(false);
        asteroidModel.gameObject.SetActive(false);
        rb.velocity /= 3;
        GetComponentInChildren<ParticleSystem>().Play();
        StartCoroutine(DisableAsteroid(5f));
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
        asteroidModel.gameObject.SetActive(true);
        EnableColliders(true);
        gameObject.SetActive(false);
    }
}
