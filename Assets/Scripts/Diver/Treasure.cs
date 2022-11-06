using System.Collections;
using UnityEngine;

public class Treasure : Collectible
{
    public GameObject hookPrefab;
    
    public override void Collect()
    {
        StartCoroutine(CollectionAnimation());
    }

    private IEnumerator CollectionAnimation()
    {
        var hookInstance = Instantiate(hookPrefab);
        var startPos = transform.position + Vector3.up * 5f;
        var targetPos = transform.position + Vector3.up * .5f;
        
        hookInstance.transform.position = startPos;

        var hookingTime = 1f;
        var elapsedTime = 0f;
        while (hookingTime - elapsedTime > 0f)
        {
            elapsedTime += Time.deltaTime;
            hookInstance.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / hookingTime);
            yield return null;
        }

        transform.SetParent(hookInstance.transform);

        hookingTime = 3f;
        elapsedTime = 0f;
        while (hookingTime - elapsedTime > 0f)
        {
            elapsedTime += Time.deltaTime;
            hookInstance.transform.position = Vector3.Lerp(targetPos, startPos, elapsedTime / hookingTime);
            yield return null;
        }
        
        Destroy(gameObject);
        Destroy(hookInstance.gameObject);

        NetworkSyncer.Get().CollectTreasureServerRpc(transform.position);
    }
}
