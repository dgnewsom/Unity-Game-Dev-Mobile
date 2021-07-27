using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TutorialVideo : MonoBehaviour
{
    [SerializeField] private VideoClip tutorialVideo;
    [SerializeField] private String title;

    public VideoClip Video => tutorialVideo;
    public string Title => title;
}
