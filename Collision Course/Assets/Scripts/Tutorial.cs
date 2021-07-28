using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TutorialVideo[] InstructionImages;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;

    private int imageIndex = 0;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }


    private void OnEnable()
    {
        imageIndex = 0;
        videoPlayer.clip = InstructionImages[imageIndex].Video;
        titleText.text = InstructionImages[imageIndex].Title;
        animator.SetBool("ShowVideo",true);
        SetButtonsEnabled();
    }

    private void OnDisable()
    {
        animator.SetBool("ShowVideo",false);
        renderTexture.Release();
    }

    /// <summary>
    /// Move to the next image if available
    /// </summary>
    public void NextInstructionImage()
    {
        if (imageIndex < InstructionImages.Length - 1)
        {
            StartCoroutine(SwitchNextVideo(0.00f));
        }
    }

    IEnumerator SwitchNextVideo(float delay)
    {
        animator.SetBool("ShowVideo", false);
        yield return new WaitForSeconds(delay);
        renderTexture.Release();
        imageIndex++;
        videoPlayer.clip = InstructionImages[imageIndex].Video;
        titleText.text = InstructionImages[imageIndex].Title;
        SetButtonsEnabled();
        animator.SetBool("ShowVideo", true);
    }

    /// <summary>
    /// Move to previous image if available
    /// </summary>
    public void PreviousInstructionImage()
    {
        if (imageIndex > 0)
        {
            StartCoroutine(SwitchPrevVideo(0.00f));
        }
    }

    IEnumerator SwitchPrevVideo(float delay)
    {
        animator.SetBool("ShowVideo", false);
        yield return new WaitForSeconds(delay);
        renderTexture.Release();
        imageIndex--;
        videoPlayer.clip = InstructionImages[imageIndex].Video;
        titleText.text = InstructionImages[imageIndex].Title;
        SetButtonsEnabled();
        animator.SetBool("ShowVideo", true);
    }

    /// <summary>
    /// Set next and previous buttons active / inactive based on current index in images
    /// </summary>
    private void SetButtonsEnabled()
    {
        previousButton.interactable = !(imageIndex <= 0);
        SetButtonFontColour(previousButton);
        nextButton.interactable = !(imageIndex >= InstructionImages.Length - 1);
        SetButtonFontColour(nextButton);
    }

    private void SetButtonFontColour(Button button)
    {
        if (button.interactable)
        {
            button.GetComponentInChildren<TMP_Text>().color = Color.white;
        }
        else
        {
            button.GetComponentInChildren<TMP_Text>().color = Color.gray;
        }
    }
}
