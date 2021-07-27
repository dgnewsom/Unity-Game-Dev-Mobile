using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSoundTrigger : MonoBehaviour
{
    private ParticleSystem laserParticleSystem;
    private int currentParticles;
    
    private void OnParticleTrigger()
    {
        SoundManager.Instance.PlayLasersSound();
    }
    
}
