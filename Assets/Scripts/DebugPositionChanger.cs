using Unity.Netcode;
using UnityEngine;

public class DebugPositionChanger : MonoBehaviour
{
    private float lastChanged = 0f;
    private float changeEvery = 1f;

    private void Awake()
    {
    }

    void Update()
    {
        if (!NetworkManager.Singleton.IsHost)
        {
            return;
        }
        if (Time.time - lastChanged > changeEvery)
        {
            Debug.Log("Changing var");
            lastChanged = Time.time;
            NetworkSyncer.Get().diverPosition.Value = new Vector3(
                (Random.value - 0.5f) * 2,
                (Random.value - 0.5f) * 2,
                (Random.value - 0.5f) * 2);
        }
    }
}
