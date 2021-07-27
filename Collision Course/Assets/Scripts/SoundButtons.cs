using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButtons : MonoBehaviour
{
    [SerializeField] private Image MusicImage;
    [SerializeField] private Image SFXImage;
    [SerializeField] private Sprite[] MusicIcons;
    [SerializeField] private Sprite[] SFXIcons;

    void OnEnable()
    {
        SetMusicImage(PlayerPrefs.GetInt(SoundManager.PlayBGMKey,1));
        SetSFXImage(PlayerPrefs.GetInt(SoundManager.PlaySFXKey,1));
    }

    public void ToggleBGMButton()
    {
        SetMusicImage(SoundManager.Instance.ToggleBGM());
    }

    public void SetMusicImage(int musicIconIndex)
    {
        MusicImage.sprite = MusicIcons[musicIconIndex];
    }

    public void ToggleSFXButton()
    {
        print("Toggle SFX");
        SetSFXImage(SoundManager.Instance.ToggleSFX());
    }

    public void SetSFXImage(int SFXIconIndex)
    {
        SFXImage.sprite = SFXIcons[SFXIconIndex];
    }
}
