using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource BackgroundMusic;
    [SerializeField] private AudioSource RocketBoostSound;

    [SerializeField] private AudioClip playerExplosionSound;
    [SerializeField] private AudioClip asteroidExplosionSound;
    [SerializeField] private AudioClip ScoreUpPickupSound;
    [SerializeField] private AudioClip ScoreDownPickupSound;
    [SerializeField] private AudioClip LasersPickupSound;
    [SerializeField] private AudioClip ShieldPickupSound;
    [SerializeField] private AudioClip continuePickupSound;

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
            BackgroundMusic = GetComponent<AudioSource>();
            playBGM = PlayerPrefs.GetInt(PlayBGMKey, 1);
            playSFX = PlayerPrefs.GetInt(PlaySFXKey, 1);
            BGMVolume = PlayerPrefs.GetFloat(BGMVolumeKey, 1f);
            BackgroundMusic.volume = BGMVolume;
            SFXVolume = Mathf.Clamp01(PlayerPrefs.GetFloat(SFXVolumeKey, 1));
            if (playBGM == 1)
            {
                SetBackgroundMusic();
            }
        }
    }

    private void SetBGMVolume(float volume)
    {
        PlayerPrefs.SetFloat(BGMVolumeKey,BGMVolume);
        BackgroundMusic.volume = BGMVolume;
    }
    
    private void SetSFXVolume(float volume)
    {
        SFXVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(SFXVolumeKey,SFXVolume);
    }
    
    public void SetBackgroundMusic()
    {
        if (!BackgroundMusic.isPlaying && playBGM == 1)
        {
            BackgroundMusic.Play();
        }
        else
        {
            StopBackgroundMusic();
        }
    }

    public void StopBackgroundMusic()
    {
        BackgroundMusic.Stop();
    }

    public void PlayBoostSound()
    {
        if (!RocketBoostSound.isPlaying && playSFX.Equals(1))
        {
            RocketBoostSound.volume = SFXVolume;
            RocketBoostSound.Play();
        }
    }

    public void StopBoostSound()
    {
        RocketBoostSound.Stop();
    }

    public void PlayScoreUpPickupSound()
    {

        if (playSFX == 1)
        {
            BackgroundMusic.PlayOneShot(ScoreUpPickupSound,1);
        }
    }

    public void PlayScoreDownPickupSound()
    {

        if (playSFX == 1)
        {
            BackgroundMusic.PlayOneShot(ScoreDownPickupSound, SFXVolume);
        }
    }

    public void PlayLasersPickupSound()
    {

        if (playSFX == 1)
        {
            BackgroundMusic.PlayOneShot(LasersPickupSound, SFXVolume);
        }
    }

    public void PlayShieldPickupSound()
    {

        if (playSFX == 1)
        {
            BackgroundMusic.PlayOneShot(ShieldPickupSound, SFXVolume);
        }
    }
    
    public void PlayContinuePickupSound()
    {

        if (playSFX == 1)
        {
            BackgroundMusic.PlayOneShot(continuePickupSound, SFXVolume);
        }
    }
    public void PlayPlayerExplosionSound()
    {

        if (playSFX == 1)
        {
            BackgroundMusic.PlayOneShot(playerExplosionSound, SFXVolume);
        }
    }

    public void PlayAsteroidExplosionSound()
    {

        if (playSFX == 1)
        {
            BackgroundMusic.PlayOneShot(asteroidExplosionSound, SFXVolume);
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
        PlayerPrefs.SetInt(PlayBGMKey,playBGM);
        SetBackgroundMusic();
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
        PlayerPrefs.SetInt(PlaySFXKey, playSFX);
        return playSFX;
    }
}
