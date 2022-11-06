using Unity.Netcode;
using UnityEngine;

public class NetworkSyncer : NetworkBehaviour
{
    private static NetworkSyncer inst;

    public NetworkVariable<Vector3> shipPosition = new();
    public NetworkVariable<Vector3> diverPosition = new();
    public NetworkVariable<float> diverTargetHeight = new();

    public NetworkVariable<Vector3> sharkPosition = new();
    public NetworkList<Vector3> treasurePositions = null;

    public NetworkVariable<int> numTreasuresCollected = new();

    public NetworkVariable<bool> gameLost = new();
    public NetworkVariable<bool> gameWon = new();

    private void Awake()
    {
        inst = this;
        shipPosition.Value = Vector3.zero;
        diverPosition.Value = Vector3.zero;
        diverTargetHeight.Value = 0f;

        sharkPosition.Value = Vector3.zero;
        treasurePositions = new NetworkList<Vector3>();

        numTreasuresCollected.Value = 0;

        gameLost.Value = false;
        gameLost.Value = false;
    }

    private void Start()
    {
        //TODO: Remove this when activating treasure functionality
        AddTreasurePositionServerRpc(new Vector3(40, 20, 100));
        AddTreasurePositionServerRpc(new Vector3(-30, 0, -30));
    }

    public static NetworkSyncer Get()
    {
        return inst;
    }


    [ServerRpc(RequireOwnership = false)]
    public void UpdateShipPositionServerRpc(Vector3 newPosition, ServerRpcParams serverRpcParams = default)
    {
        //Debug.Log($"Updating diver position from {diverPosition.Value} to {newPosition} by client {serverRpcParams.Receive.SenderClientId}");
        shipPosition.Value = newPosition;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateDiverPositionServerRpc(Vector3 newPosition, ServerRpcParams serverRpcParams = default)
    {
        //Debug.Log($"Updating diver position from {diverPosition.Value} to {newPosition} by client {serverRpcParams.Receive.SenderClientId}");
        diverPosition.Value = newPosition;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateDiverTargetHeightServerRpc(float newHeight, ServerRpcParams serverRpcParams = default)
    {
        //Debug.Log($"Updating diver target height from {diverTargetHeight.Value} to {newHeight} by client {serverRpcParams.Receive.SenderClientId}");
        diverTargetHeight.Value = newHeight;
    }



    [ServerRpc(RequireOwnership = false)]
    public void UpdateSharkPositionServerRpc(Vector3 newPosition, ServerRpcParams serverRpcParams = default)
    {
        //Debug.Log($"Updating shark position from {diverPosition.Value} to {newPosition} by client {serverRpcParams.Receive.SenderClientId}");
        sharkPosition.Value = newPosition;
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddTreasurePositionServerRpc(Vector3 treasurePosition, ServerRpcParams serverRpcParams = default)
    {
        treasurePositions.Add(treasurePosition);
    }

    [ServerRpc(RequireOwnership = false)]
    public void CollectTreasureServerRpc(Vector3 treasurePosition, ServerRpcParams serverRpcParams = default)
    {
        treasurePositions.Remove(treasurePosition);
        numTreasuresCollected.Value = numTreasuresCollected.Value + 1;
    }


    [ServerRpc(RequireOwnership = false)]
    public void SetGameToLostServerRpc(ServerRpcParams serverRpcParams = default)
    {
        Debug.Log($"Setting game to lost by client {serverRpcParams.Receive.SenderClientId}");
        gameLost.Value = true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetGameToWonServerRpc(ServerRpcParams serverRpcParams = default)
    {
        Debug.Log($"Setting game to won by client {serverRpcParams.Receive.SenderClientId}");
        gameWon.Value = true;
    }
}
