using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private string collectibleName;
    [SerializeField] private Sprite collectibleIcon;
    [SerializeField] private float effectTime = 15f;
    [SerializeField] private GameObject CollectibleModel;

    private UIScript uiScript;
    private bool isActive = false;
    private float timer;

    private void Start()
    {
        uiScript = FindObjectOfType<UIScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null && !UIScript.CollectibleActive)
        {
            ActivateCollectible();
        }
    }

    public void ActivateCollectible()
    {
        isActive = true;
        timer = effectTime;
        uiScript.SetCollectibleDisplay(collectibleIcon, collectibleName);
        CollectibleModel.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (isActive)
        {
            timer -= Time.deltaTime;
            uiScript.SetCollectibleTimerDisplay(timer);
            if (timer <= 0)
            {
                ClearCollectible();
                isActive = false;
            }
        }
    }

    private void ClearCollectible()
    {
        uiScript.ClearCollectibleDisplay();
    }
}
