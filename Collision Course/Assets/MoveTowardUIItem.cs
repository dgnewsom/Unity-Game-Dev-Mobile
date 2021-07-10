using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentageScoreIndicator : MonoBehaviour
{
    [SerializeField] private string TargetTag;
    private RectTransform rectTransform;
    private RectTransform scoreDisplayRectTransform;
    private bool moving = false;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        scoreDisplayRectTransform = GameObject.FindGameObjectWithTag(TargetTag).GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        if (UIScript.IsRunning && moving)
        {
            rectTransform.position = Vector3.LerpUnclamped(rectTransform.position, scoreDisplayRectTransform.position, Time.deltaTime * 2);
            if (Vector3.Distance(rectTransform.position, scoreDisplayRectTransform.position) <= 200)
            {
                Destroy(this.gameObject);
            }
        }
        
    }




}
