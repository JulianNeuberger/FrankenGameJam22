using System;
using UnityEngine;

public class AttackState : State
{
    public GameObject player;
    public float attackRange = 4.5f;
    public IdleState idleState;
    
    public override State Tick()
    {
        var distanceToPlayer = (transform.position - player.transform.position).magnitude;
        if (distanceToPlayer > attackRange)
        {
            return idleState;
        }
        
        NetworkSyncer.Get().SetGameToLostServerRpc();
        
        return this;
    }

    public override string Description()
    {
        var distanceToPlayer = (transform.position - player.transform.position).magnitude;
        return distanceToPlayer.ToString();
    }
}
