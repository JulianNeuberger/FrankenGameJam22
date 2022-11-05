using System;
using UnityEngine;

public class AttackState : State
{
    public GameObject player;
    public float attackRange = 1f;
    public IdleState idleState;

    public override State Tick()
    {
        var distanceToPlayer = (transform.position - player.transform.position).magnitude;
        if (distanceToPlayer > attackRange)
        {
            return idleState;
        }
        Debug.Log("Attacking!");
        // TODO: deaggro with some probability
        return this;
    }
}
