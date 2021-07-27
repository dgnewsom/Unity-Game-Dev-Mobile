using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private SoundButtons soundButtons;

    void Awake()
    {
        BGMSlider.value = PlayerPrefs.GetFloat(SoundManager.BGMVolumeKey);
        SFXSlider.value = PlayerPrefs.GetFloat(SoundManager.SFXVolumeKey);
    }
    
    public void SetBGMVolume()
    {
        SoundManager.Instance.SetBGMVolume(BGMSlider.value);
    }

    public void SetSFXVolume()
    {
        SoundManager.Instance.SetSFXVolume(SFXSlider.value);
    }


}
