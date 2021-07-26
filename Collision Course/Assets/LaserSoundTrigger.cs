using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSoundTrigger : MonoBehaviour
{
    private ParticleSystem laserParticleSystem;
    private int currentParticles;
    private SoundManager soundManager;

    void OnEnable()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void OnParticleTrigger()
    {
        if (soundManager != null)
        {
            soundManager.PlayLasersSound();
        }
    }
    
}
