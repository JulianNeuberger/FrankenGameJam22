using Unity.Netcode;
using UnityEngine;

public class NetworkSyncer : NetworkBehaviour
{
    private static NetworkSyncer inst;

    public NetworkVariable<Vector3> diverPosition = new();

    private void Awake()
    {
        inst = this;
        diverPosition.Value = Vector3.zero;
    }

    public static NetworkSyncer Get()
    {
        return inst;
    }
}
