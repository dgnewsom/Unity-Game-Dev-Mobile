using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSoundTrigger : MonoBehaviour
{
    private ParticleSystem laserParticleSystem;
    private int currentParticles;
    private SoundManager soundManager;


    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void OnParticleTrigger()
    {
        soundManager.PlayLasersSound();
    }
    
}
