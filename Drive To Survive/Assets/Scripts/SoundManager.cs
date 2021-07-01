using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource BackgroundMusic;
    [SerializeField] private AudioClip ScorePickupSound;
    [SerializeField] private AudioClip SpeedUpPickupSound;
    [SerializeField] private AudioClip SpeedDownPickupSound;

    private int playBGM = 1;
    private int playSFX = 1;
    public const string PlayBGMKey = "BGM";
    public const string PlaySFXKey = "SFX";

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
            if (playBGM == 1)
            {
                SetBackgroundMusic();
            }
        }
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

    public void PlayScorePickupSound()
    {
        if (playSFX == 1)
        {
            BackgroundMusic.PlayOneShot(ScorePickupSound);
        }
    }

    public void PlaySpeedUpPickupSound()
    {
        if (playSFX == 1)
        {
            BackgroundMusic.PlayOneShot(SpeedUpPickupSound);
        }
    }

    public void PlaySpeedDownPickupSound()
    {
        if (playSFX == 1)
        {
            BackgroundMusic.PlayOneShot(SpeedDownPickupSound);
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
