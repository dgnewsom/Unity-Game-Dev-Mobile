using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType {ScoreUp, ScoreDown, ScoreMultiply, HealthUp, Lasers, Shield, Continue}
public static class CollectibleTypes
{
    public static Sprite GetIconFromCollectibleType(CollectibleType collectibleType)
    {
        switch (collectibleType)
        {
            case CollectibleType.ScoreUp:
                return Resources.Load<Sprite>("CollectibleIcons/ScoreUpIcon");
            case CollectibleType.ScoreDown:
                return Resources.Load<Sprite>("CollectibleIcons/ScoreDownIcon");
            case CollectibleType.ScoreMultiply:
                return Resources.Load<Sprite>("CollectibleIcons/ScoreMultiplyIcon");
            case CollectibleType.HealthUp:
                return Resources.Load<Sprite>("CollectibleIcons/HealthUpIcon");
            case CollectibleType.Lasers:
                return Resources.Load<Sprite>("CollectibleIcons/LasersIcon");
            case CollectibleType.Shield:
                return Resources.Load<Sprite>("CollectibleIcons/ShieldIcon");
            case CollectibleType.Continue:
                return Resources.Load<Sprite>("CollectibleIcons/ContinueIcon");
            default:
                throw new ArgumentOutOfRangeException(nameof(collectibleType), collectibleType, null);
        }
    }

    public static Color GetColourFromPickupType(CollectibleType collectibleType)
    {
        switch (collectibleType)
        {
            case CollectibleType.ScoreUp:
                return Color.green;
            case CollectibleType.ScoreDown:
                return Color.red;
            case CollectibleType.ScoreMultiply:
                return Color.green;
            case CollectibleType.HealthUp:
                return Color.green;
            case CollectibleType.Lasers:
                return Color.green;
            case CollectibleType.Shield:
                return Color.green;
            case CollectibleType.Continue:
                return Color.yellow;
            default:
                throw new ArgumentOutOfRangeException(nameof(collectibleType), collectibleType, null);
        }
    }

    public static RectTransform GetUITargetFromCollectibleType(CollectibleType collectibleType)
    {
        switch (collectibleType)
        {
            case CollectibleType.ScoreUp:
                return GameObject.FindGameObjectWithTag("ScoreDisplay").GetComponent<RectTransform>();
            case CollectibleType.ScoreDown:
                return GameObject.FindGameObjectWithTag("ScoreDisplay").GetComponent<RectTransform>();
            case CollectibleType.ScoreMultiply:
                return GameObject.FindGameObjectWithTag("MultiplierDisplay").GetComponent<RectTransform>();
            case CollectibleType.HealthUp:
                return GameObject.FindGameObjectWithTag("HealthDisplay").GetComponent<RectTransform>();
            case CollectibleType.Lasers:
                return GameObject.FindGameObjectWithTag("LaserDisplay").GetComponent<RectTransform>();
            case CollectibleType.Shield:
                return GameObject.FindGameObjectWithTag("ShieldDisplay").GetComponent<RectTransform>();
            case CollectibleType.Continue:
                return GameObject.FindGameObjectWithTag("HealthDisplay").GetComponent<RectTransform>();
            default:
                throw new ArgumentOutOfRangeException(nameof(collectibleType), collectibleType, null);
        }
    }
}
