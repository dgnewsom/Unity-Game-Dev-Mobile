using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource rocketBoostSound;
    [SerializeField] private AudioSource lasersSound;

    [SerializeField] private AudioClip playerExplosionSound;
    [SerializeField] private AudioClip asteroidExplosionSound;
    [SerializeField] private AudioClip positivePickupSound;
    [SerializeField] private AudioClip negativePickupSound;
    [SerializeField] private AudioClip shieldPickupSound;
    [SerializeField] private AudioClip shieldOffSound;
    [SerializeField] private AudioClip continuePickupSound;
    [SerializeField] private AudioClip countdownBeepSound;
    [SerializeField] private AudioClip startBeepSound;
    [SerializeField] private AudioClip laserChargeSound;

    private int playBGM = 1;
    private int playSFX = 1;
    private float BGMVolume = 1f;
    private float SFXVolume = 1f;

    public const string PlayBGMKey = "BGM";
    public const string PlaySFXKey = "SFX";
    public const string BGMVolumeKey = "BGMVolume";
    public const string SFXVolumeKey = "SFXVolume";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            playBGM = PlayerPrefs.GetInt(PlayBGMKey, 1);
            playSFX = PlayerPrefs.GetInt(PlaySFXKey, 1);
            BGMVolume = Mathf.Clamp01(PlayerPrefs.GetFloat(BGMVolumeKey, 1f));
            backgroundMusic.volume = BGMVolume;
            SFXVolume = Mathf.Clamp01(PlayerPrefs.GetFloat(SFXVolumeKey, 1f));
            if (playBGM == 1)
            {
                SetBackgroundMusic();
            }
        }
    }

    public int ToggleBGM()
    {
        playBGM = PlayerPrefs.GetInt(PlayBGMKey);

        if (playBGM == 1)
        {
            playBGM = 0;
        }
        else
        {
            playBGM = 1;
        }
        SetBackgroundMusic();
        PlayerPrefs.SetInt(PlayBGMKey, playBGM);
        return playBGM;
    }

    public int ToggleSFX()
    {
        playSFX = PlayerPrefs.GetInt(PlaySFXKey);

        if (playSFX == 1)
        {
            playSFX = 0;
        }
        else
        {
            playSFX = 1;
        }
        PlayerPrefs.SetInt(PlaySFXKey,playSFX);
        return playSFX;
    }
    public void SetBGMVolume(float volume)
    {
        BGMVolume = Mathf.Clamp01(volume);
        backgroundMusic.volume = BGMVolume;
        PlayerPrefs.SetFloat(BGMVolumeKey,BGMVolume);
        
            if (!backgroundMusic.isPlaying)
            {
                SetBackgroundMusic();
            }
    }

    public void SetSFXVolume(float volume)
    {
        SFXVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(SFXVolumeKey,SFXVolume);
    }

    public void SetBackgroundMusic()
    {
        if (!backgroundMusic.isPlaying && playBGM == 1)
        {
            backgroundMusic.Play();
        }
        else
        {
            StopBackgroundMusic();
        }
    }

    public void StopBackgroundMusic()
    {
        backgroundMusic.Stop();
    }

    public void PlayBoostSound()
    {
        if (!rocketBoostSound.isPlaying && playSFX.Equals(1))
        {
            rocketBoostSound.volume = SFXVolume / 2;
            rocketBoostSound.Play();
        }
    }

    public void StopBoostSound()
    {
        rocketBoostSound.Stop();
    }

    public void PlayLasersSound()
    {
        
        if (!lasersSound.isPlaying && playSFX.Equals(1))
        {
            lasersSound.volume = SFXVolume;
            lasersSound.Play();
        }
    }

    public void StopLasersSound()
    {
        lasersSound.Stop();
    }

    public void PlayPositivePickupSound()
    {

        if (playSFX == 1)
        {
            backgroundMusic.PlayOneShot(positivePickupSound, SFXVolume);
        }
    }

    public void PlayNegativePickupSound()
    {

        if (playSFX == 1)
        {
            backgroundMusic.PlayOneShot(negativePickupSound, SFXVolume);
        }
    }

    public void PlayShieldPickupSound()
    {

        if (playSFX == 1)
        {
            backgroundMusic.PlayOneShot(shieldPickupSound, SFXVolume);
        }
    }

    public void PlayShieldOffSound()
    {

        if (playSFX == 1)
        {
            backgroundMusic.PlayOneShot(shieldOffSound, SFXVolume);
        }
    }

    public void PlayContinuePickupSound()
    {

        if (playSFX == 1)
        {
            backgroundMusic.PlayOneShot(continuePickupSound, SFXVolume);
        }
    }

    public void PlayPlayerExplosionSound()
    {

        if (playSFX == 1)
        {
            backgroundMusic.PlayOneShot(playerExplosionSound, SFXVolume);
        }
    }

    public void PlayAsteroidExplosionSound()
    {

        if (playSFX == 1)
        {
            backgroundMusic.PlayOneShot(asteroidExplosionSound, SFXVolume);
        }
    }

    public void PlayCountdownBeepSound()
    {

        if (playSFX == 1)
        {
            backgroundMusic.PlayOneShot(countdownBeepSound, SFXVolume);
        }
    }

    public void PlayStartBeepSound()
    {

        if (playSFX == 1)
        {
            backgroundMusic.PlayOneShot(startBeepSound, SFXVolume);
        }
    }

    public void PlayLaserChargeSound()
    {

        if (playSFX == 1)
        {
            backgroundMusic.PlayOneShot(laserChargeSound, SFXVolume);
        }
    }
}
