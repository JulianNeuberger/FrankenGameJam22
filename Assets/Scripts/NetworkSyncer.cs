using Unity.Netcode;
using UnityEngine;

public class NetworkSyncer : NetworkBehaviour
{
    private static NetworkSyncer inst;

    public NetworkVariable<Vector3> diverPosition = new();
    public NetworkVariable<float> diverTargetHeight = new();

    private void Awake()
    {
        inst = this;
        diverPosition.Value = Vector3.zero;
    }

    public static NetworkSyncer Get()
    {
        return inst;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateDiverPositionServerRpc(Vector3 newPosition, ServerRpcParams serverRpcParams = default)
    {
        Debug.Log($"Updating diver position from {diverPosition.Value} to {newPosition} by client {serverRpcParams.Receive.SenderClientId}");
        diverPosition.Value = newPosition;
    }


    [ServerRpc(RequireOwnership = false)]
    public void UpdateDiverTargetHeightServerRpc(float newHeight, ServerRpcParams serverRpcParams = default)
    {
        Debug.Log($"Updating diver target height from {diverTargetHeight.Value} to {newHeight} by client {serverRpcParams.Receive.SenderClientId}");
        diverTargetHeight.Value = newHeight;
    }
}
