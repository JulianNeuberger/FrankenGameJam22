using UnityEngine;

[RequireComponent(typeof(MoveState), 
    typeof(StateMachine),
    typeof(IdleState))]
public class SharkController : MonoBehaviour
{
    private MoveState _moveState;
    private StateMachine _stateMachine;
    private IdleState _idleState;
    
    // Start is called before the first frame update
    void Start()
    {
        _moveState = GetComponent<MoveState>();
        _stateMachine = GetComponent<StateMachine>();
        _idleState = GetComponent<IdleState>();
    }

    public void Scare(Vector3 from)
    {
        var dir = (transform.position - from).normalized;
        var fleeTo = dir * 35f;
        _stateMachine.currentState = _moveState;
        _moveState.target = fleeTo;
        _idleState.moveAnchor = fleeTo;
    }
}
