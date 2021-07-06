using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to set Music / SFX on and off and update UI.
/// </summary>
public class SoundButtonsScript : MonoBehaviour
{
    [SerializeField] private Image MusicImage;
    [SerializeField] private Image SFXImage;
    [SerializeField] private Sprite[] MusicIcons;
    [SerializeField] private Sprite[] SFXIcons;

    private void Start()
    {
        SetMusicImage(PlayerPrefs.GetInt(SoundManager.PlayBGMKey));
        SetSFXImage(PlayerPrefs.GetInt(SoundManager.PlaySFXKey));
    }

    public void ToggleBGMButton()
    {
        SetMusicImage(SoundManager.Instance.ToggleBGM());
    }

    private void SetMusicImage(int musicIconIndex)
    {
        MusicImage.sprite = MusicIcons[musicIconIndex];
    }

    public void ToggleSFXButton()
    {
        SetSFXImage(SoundManager.Instance.ToggleSFX());
    }

    private void SetSFXImage(int SFXIconIndex)
    {
        SFXImage.sprite = SFXIcons[SFXIconIndex];
    }

}
