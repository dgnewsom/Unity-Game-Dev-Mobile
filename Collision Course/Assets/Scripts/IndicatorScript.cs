using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorScript : MonoBehaviour
{
    private CollectibleType collectibleType;
    private RectTransform targetRectTransform;
    private RectTransform rectTransform;
    private bool moving = false;

    public void SetIndicatorValues(CollectibleType collectibleType, string collectibleAmount, Vector2 position)
    {
        this.collectibleType = collectibleType;
        rectTransform = GetComponent<RectTransform>();
        rectTransform.position = new Vector3(position.x,position.y,0);
        targetRectTransform = CollectibleTypes.GetUITargetFromCollectibleType(collectibleType);
        Image icon = GetComponentInChildren<Image>();
        icon.sprite = CollectibleTypes.GetIconFromCollectibleType(collectibleType);
        if (collectibleType != CollectibleType.Lasers && collectibleType != CollectibleType.Shield)
        {
            Color colour = CollectibleTypes.GetColourFromPickupType(collectibleType);
            icon.color = colour;
        }
        GetComponentInChildren<TMP_Text>().text = collectibleAmount;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (UIScript.IsRunning && moving)
        {
            rectTransform.position = Vector3.LerpUnclamped(rectTransform.position, targetRectTransform.position, Time.deltaTime);
            if (Vector3.Distance(rectTransform.position, targetRectTransform.position) <= 200)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void StartMoving()
    {
        moving = true;
    }
}
