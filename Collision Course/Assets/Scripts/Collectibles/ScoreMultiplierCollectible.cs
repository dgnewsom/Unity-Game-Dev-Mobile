using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMultiplierCollectible : Collectible
{
    /*private int[] multiplierAmounts = {2, 3, 4, 5, 10, 15, 20};

    protected override void SetAmountText()
    {
        base.SetAmountText();
        collectibleAmount = multiplierAmounts[Random.Range(0, multiplierAmounts.Length)];
        amountText.text = $"X{collectibleAmount}";
        collectibleName += $"\nX{collectibleAmount}";
    }

    protected override void ActivateCollectible()
    {
        base.ActivateCollectible();
        FindObjectOfType<Scorer>().SetScoreMultiplier(collectibleAmount);
    }

    protected override void ClearCollectible()
    {
        base.ClearCollectible();
        FindObjectOfType<Scorer>().ResetMultiplier();
    }*/
}
