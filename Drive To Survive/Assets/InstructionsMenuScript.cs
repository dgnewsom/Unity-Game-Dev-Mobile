using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsMenuScript : MonoBehaviour
{
    [SerializeField] private Sprite[] InstructionImages;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject previousButton;
    [SerializeField] private Image instructionImage;

    private int imageIndex = 0;

    private void OnEnable()
    {
        imageIndex = 0;
        instructionImage.sprite = InstructionImages[imageIndex];
        SetButtonsEnabled();
    }

    public void NextInstructionImage()
    {
        if (imageIndex < InstructionImages.Length - 1)
        {
            imageIndex++;
            instructionImage.sprite = InstructionImages[imageIndex];
            SetButtonsEnabled();
        }
    }

    public void PreviousInstructionImage()
    {
        if (imageIndex > 0)
        {
            imageIndex--;
            instructionImage.sprite = InstructionImages[imageIndex];
            SetButtonsEnabled();
        }
    }

    private void SetButtonsEnabled()
    {
        previousButton.SetActive(!(imageIndex <= 0));
        nextButton.SetActive(!(imageIndex >= InstructionImages.Length - 1));
    }
}
