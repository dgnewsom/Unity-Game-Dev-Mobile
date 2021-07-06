using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple class to store an image and title
/// </summary>
public class InstructionImage : MonoBehaviour
{
    [SerializeField] private string title;
    [SerializeField] private Sprite image;

    public string Title => title;
    public Sprite Image => image;
}
