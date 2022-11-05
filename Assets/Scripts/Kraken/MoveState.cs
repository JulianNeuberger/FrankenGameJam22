using UnityEngine;

public class MoveState : State
{
    public float moveSpeed = 3f;
    public Vector3 target;
    public float maxHeight = 10f;
    public float turnSpeedDegrees = 25f;
    public State idleState;

    public override State Tick()
    {
        UpdateRotation();
        UpdatePosition();
        
        if (IsAtTarget())
        {
            return idleState;
        }

        return this;
    }
    
    private void UpdatePosition()
    {
        var vectorToTarget = target - transform.position;
        var angleToTarget = Vector3.SignedAngle(transform.forward, vectorToTarget, Vector3.up);
        angleToTarget *= Mathf.Deg2Rad;
        var moveAmount = Mathf.Clamp(Mathf.Cos(angleToTarget), 0f, 1f);
        transform.position += transform.forward * (moveSpeed * moveAmount * Time.deltaTime);
    }
    
    private void UpdateRotation()
    {
        var targetRot = Quaternion.LookRotation(target - transform.position, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime);
    }

    private bool IsAtTarget()
    {
        var diff = (target - transform.position).magnitude;
        var radius = 6f;
        return diff < radius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(target, 1f);
    }
}
