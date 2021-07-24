using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleIndicatorSpawner : MonoBehaviour
{
    
    void Start()
    {
        //SpawnIndicator(CollectibleType.ScoreUp,"ScoreUp!",new Vector3(500,500,0));
        //SpawnIndicator(CollectibleType.ScoreDown,"ScoreDown!",new Vector3(500,500,0));
        //SpawnIndicator(CollectibleType.ScoreMultiply,"ScoreMultiply!",new Vector3(500,500,0));
        //SpawnIndicator(CollectibleType.HealthUp,"HealthUp!",new Vector3(500,500,0));
        //SpawnIndicator(CollectibleType.Shield,"Shield!",new Vector3(500,500,0));
        //SpawnIndicator(CollectibleType.Lasers,"Lasers!",new Vector3(500,500,0));
        //SpawnIndicator(CollectibleType.Continue,"Continue!",new Vector3(500,500,0));
    }

    public void SpawnIndicator(CollectibleType collectibleType, string collectibleAmount, Vector3 spawnPosition)
    {
        GameObject indicatorPrefab = Resources.Load<GameObject>("CollectibleIndicator");
        GameObject indicatorInstance = Instantiate(indicatorPrefab, transform.position, Quaternion.identity, this.transform);
        indicatorInstance.GetComponent<IndicatorScript>().SetIndicatorValues(collectibleType,collectibleAmount,new Vector2(spawnPosition.x,spawnPosition.y));
        indicatorInstance.GetComponent<IndicatorScript>().StartMoving();
    }
}
