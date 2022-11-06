using UnityEngine;

public class MoveState : State
{
    public float moveSpeed = 3f;
    public Vector3 target;
    public float arriveRadius = 7.5f;
    public float maxHeight = 10f;
    public float turnSpeedDegrees = 25f;
    public State idleState;

    private float timeout = 30f;
    private Vector3 lastTarget;
    private float timeSinceTargetChange;
    
    public override State Tick()
    {
        UpdateRotation();
        UpdatePosition();

        timeSinceTargetChange += Time.deltaTime;
        
        if (lastTarget != target)
        {
            lastTarget = target;
            timeSinceTargetChange = 0f;
        }

        if (timeSinceTargetChange > timeout)
        {
            timeSinceTargetChange = 0f;
            return idleState;
        }

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
        return diff < arriveRadius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(target, 1f);
    }

    public override string Description()
    {
        var distance = target - transform.position;
        return distance.magnitude.ToString();
    }
}
