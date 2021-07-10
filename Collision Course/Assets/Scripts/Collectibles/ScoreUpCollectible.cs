using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUpCollectible : Collectible
{

    /*
    protected override void SetAmountText()
    {
        base.SetAmountText();
        collectibleAmount = Mathf.CeilToInt(Random.Range(0f, 4.5f)) * 5;
        amountText.text = $"{collectibleAmount}%";
        collectibleName += $"\n{collectibleAmount}%";
    }

    protected override void ActivateCollectible()
    {
        base.ActivateCollectible();
        FindObjectOfType<Scorer>().IncreaseScorePercentage(collectibleAmount);
    }
    */

}
